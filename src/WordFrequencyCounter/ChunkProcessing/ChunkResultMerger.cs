using System;
using System.Collections.Generic;
using System.Linq;

namespace WordFrequencyCounter.ChunkProcessing
{
    public sealed class ChunkResultMerger : IChunkResultMerger
    {
        public IDictionary<string, int> Merge(ChunkResult[] chunkResults)
        {
            if (chunkResults == null) throw new ArgumentNullException(nameof(chunkResults));
            if (chunkResults.Length == 0) return new Dictionary<string, int>();

            var dictionary = new Dictionary<string, int>();

            var words = chunkResults.SelectMany(x => x.Words)
                                    .Concat(StitchChunksEdges(chunkResults));

            foreach (var word in words)
            {
                if (dictionary.ContainsKey(word.Key)) dictionary[word.Key] += word.Value;
                else dictionary.Add(word.Key, word.Value);
            }

            return dictionary;
        }

        private static IEnumerable<KeyValuePair<string, int>> StitchChunksEdges(ChunkResult[] chunkResults)
        {
            if (!string.IsNullOrEmpty(chunkResults.First().FirstWord)) yield return new KeyValuePair<string, int>(chunkResults.First().FirstWord, 1);
            if (!string.IsNullOrEmpty(chunkResults.Last().LastWord)) yield return new KeyValuePair<string, int>(chunkResults.Last().LastWord, 1);
            for (var i = 1; i < chunkResults.Length; i++)
            {
                var word = $"{chunkResults[i - 1].LastWord}{chunkResults[i].FirstWord}".Trim();
                if (!string.IsNullOrEmpty(word)) yield return new KeyValuePair<string, int>(word, 1);
            }
        }
    }
}