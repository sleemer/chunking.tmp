using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace WordFrequencyCounter.Core.ChunkProcessing
{
    public sealed class SimpleChunkFileReader : IChunkFileReader
    {
        public void ReadAll(string fileName, BlockingCollection<string> chunks)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));
            if (!File.Exists(fileName)) throw new FileNotFoundException("File not found", fileName);
            if (chunks == null) throw new ArgumentNullException(nameof(chunks));

            foreach (var line in File.ReadLines(fileName))
            {
                chunks.Add(line);
            }
        }
    }
}