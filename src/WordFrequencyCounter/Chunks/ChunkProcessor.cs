using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace WordFrequencyCounter.Chunks
{
    public sealed class ChunkProcessor : IChunkProcessor
    {
        public IDictionary<string, int> Process(BlockingCollection<string[]> chunks)
        {
            if (chunks == null) throw new ArgumentNullException(nameof(chunks));

            var dictionary = new Dictionary<string, int>();
            foreach (var chunk in chunks.GetConsumingEnumerable())
            {
                if (chunk == null) break;
                ProcessChunk(chunk, dictionary);
            }
            return dictionary;
        }

        public static void ProcessChunk(string[] chunk, IDictionary<string, int> dictionary)
        {
            foreach (var word in chunk.SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)))
            {
                if (dictionary.ContainsKey(word)) dictionary[word]++;
                else dictionary.Add(word, 1);
            }
        }
    }
}