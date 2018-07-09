using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordFrequencyCounter.Chunks
{
    public sealed class ChunkMerger : IChunkMerger
    {
        public IDictionary<string, int> Merge(IEnumerable<IDictionary<string, int>> chunkResults)
        {
            if (chunkResults == null) throw new ArgumentNullException(nameof(chunkResults));
            if (!chunkResults.Any()) return new Dictionary<string, int>();

            var dictionary = new Dictionary<string, int>(chunkResults.First(), StringComparer.InvariantCultureIgnoreCase);
            foreach (var pair in chunkResults.Skip(1).SelectMany(result => result))
            {
                if (dictionary.ContainsKey(pair.Key)) dictionary[pair.Key] += pair.Value;
                else dictionary.Add(pair.Key, pair.Value);
            }
            return dictionary;
        }
    }
}