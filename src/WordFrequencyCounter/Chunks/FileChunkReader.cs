using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace WordFrequencyCounter.Chunks
{
    public sealed class FileChunkReader : IFileChunkReader
    {
        private static int BufferSize = 1024 * 8 * 16;
        private static readonly byte[] Separetors = { (byte)' ', 13, 10 };

        public void ReadAll(string fileName, BlockingCollection<string[]> chunks)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));
            if (!File.Exists(fileName)) throw new FileNotFoundException("File not found", fileName);
            if (chunks == null) throw new ArgumentNullException(nameof(chunks));

            try
            {
                using (var fs = File.OpenRead(fileName))
                {
                    var buffer = new byte[BufferSize];
                    var position = 0;

                    while (true)
                    {
                        var dataLengthToRead = BufferSize - position;
                        var readCount = fs.Read(buffer, position, dataLengthToRead);

                        if (readCount < dataLengthToRead)
                        {
                            using (var stream = new MemoryStream(buffer, 0, readCount + position))
                            using (var reader = new StreamReader(stream))
                            {
                                var lastChunk = reader.ReadToEnd().ToLower().Split(Environment.NewLine).ToArray();
                                chunks.Add(lastChunk);
                            }
                            break;
                        }

                        position = BufferSize - 1;
                        while (!Separetors.Contains(buffer[position]) && --position > 0) { }

                        using (var stream = new MemoryStream(buffer, 0, position))
                        using (var reader = new StreamReader(stream))
                        {
                            var chunk = reader.ReadToEnd().ToLower().Split(Environment.NewLine).ToArray();
                            chunks.Add(chunk);
                        }

                        if (position < BufferSize - 1)
                        {
                            Array.Copy(buffer, position + 1, buffer, 0, BufferSize - position - 1);
                            position = BufferSize - position - 1;
                        }
                        else
                        {
                            position = 0;
                        }
                    }
                }
            }
            finally
            {
                chunks.CompleteAdding();
            }
        }
    }
}