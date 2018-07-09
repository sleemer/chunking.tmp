using System;
using System.Collections.Generic;
using System.Linq;

namespace WordFrequencyCounter.ChunkProcessing
{
    public sealed class ChunkProcessor : IChunkProcessor
    {
        private static readonly string[] Separator = { " " };

        public ChunkResult Process(IChunk chunk)
        {
            if (chunk == null) throw new ArgumentNullException(nameof(chunk));
            var lines = chunk.ReadLines() ?? throw new ArgumentException("Chunk's ReadLines shouldn't return null!", nameof(chunk));

            var words = new Dictionary<string, int>();
            string firstWord = null;
            string lastWord = null;

            foreach (var word in lines.SelectMany(line => line))
            {
                if (firstWord == null)
                {
                    firstWord = word;
                    continue;
                }
                if (!string.IsNullOrWhiteSpace(lastWord))
                {
                    if (words.ContainsKey(lastWord)) words[lastWord]++;
                    else words.Add(lastWord, 1);
                }
                lastWord = word;
            }

            return new ChunkResult(firstWord, lastWord, words);
        }
    }
}