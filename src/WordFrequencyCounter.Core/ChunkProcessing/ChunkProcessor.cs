using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace WordFrequencyCounter.Core.ChunkProcessing
{
    public sealed class ChunkProcessor : IChunkProcessor
    {
        private static readonly string[] Separetors = new[] { " ", Environment.NewLine };

        public ChunkResult Process(BlockingCollection<string> chunks)
        {
            if (chunks == null) throw new ArgumentNullException(nameof(chunks));

            var dictionary = new Dictionary<string, int>();
            foreach (var chunk in chunks.GetConsumingEnumerable())
            {
                ProcessChunk(chunk, dictionary);
            }
            return new ChunkResult(dictionary);
        }

        private static void ProcessChunk(string chunk, IDictionary<string, int> dictionary)
        {
            foreach (var word in chunk.ToLower().Split(Separetors, StringSplitOptions.RemoveEmptyEntries))
            {
                if (dictionary.ContainsKey(word)) dictionary[word]++;
                else dictionary.Add(word, 1);
            }
        }
    }
}