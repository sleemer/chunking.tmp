using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;

namespace WordFrequencyCounter.ChunkProcessing
{
    public sealed class MemoryMappedFileChunkSplitter : IChunkFileSplitter
    {
        private const long DefaultChunkSize = 4 * 1024 * 1024;

        public IChunk[] SplitIntoChunks(string fileName) => SplitIntoChunks(fileName, DefaultChunkSize);
        public IChunk[] SplitIntoChunks(string fileName, long chunkSize)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));
            if (!File.Exists(fileName)) throw new FileNotFoundException("File not found", fileName);

            var fileSize = new FileInfo(fileName).Length;

            chunkSize = fileSize < chunkSize ? fileSize : chunkSize;
            var chunkCount = (int)(fileSize / chunkSize);

            return Enumerable.Range(0, chunkCount)
                             .Select(i => new MemoryMappedFileChunk(
                                 fileName: fileName,
                                 offset: i * chunkSize,
                                 size: i == chunkCount - 1 ? fileSize - i * chunkSize : chunkSize))
                             .ToArray();
        }

        private sealed class MemoryMappedFileChunk : IChunk
        {
            private readonly string _fileName;
            private readonly long _offset;
            private readonly long _size;

            public MemoryMappedFileChunk(string fileName, long offset, long size)
            {
                _fileName = fileName;
                _offset = offset;
                _size = size;
            }

            public IEnumerable<string[]> ReadLines()
            {
                // detect if the previous chunk was incomplete, otherwise add empty line as a chunk separator
                if (_offset > 0)
                {
                    using (var mmf = MemoryMappedFile.CreateFromFile(_fileName))
                    using (var accessor = mmf.CreateViewAccessor(_offset - 1, 1))
                    {
                        var prevByte = accessor.ReadByte(0);
                        if (prevByte == 10) yield return Array.Empty<string>();
                    }
                }

                using (var mmf = MemoryMappedFile.CreateFromFile(_fileName))
                using (var stream = mmf.CreateViewStream(_offset, _size))
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        yield return line.ToLower().Split(' ', StringSplitOptions.None);
                    }
                }
            }
        }
    }
}