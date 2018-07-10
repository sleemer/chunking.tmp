using System;
using System.Collections.Generic;
using System.Linq;

namespace WordFrequencyCounter.Core.ChunkProcessing
{
    public sealed class ChunkResultMerger : IChunkResultMerger
    {
        public IDictionary<string, int> Merge(ChunkResult[] chunkResults)
        {
            if (chunkResults == null) throw new ArgumentNullException(nameof(chunkResults));
            if (chunkResults.Length == 0) return new Dictionary<string, int>();

            var dictionary = new Dictionary<string, int>();
            foreach (var wordResults in chunkResults.SelectMany(x => x.WordFrequencies))
            {
                if (dictionary.ContainsKey(wordResults.Key)) dictionary[wordResults.Key] += wordResults.Value;
                else dictionary.Add(wordResults.Key, wordResults.Value);
            }

            return dictionary;
        }
    }
}