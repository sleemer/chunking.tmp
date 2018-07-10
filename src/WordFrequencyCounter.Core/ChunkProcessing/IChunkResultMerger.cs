using System.Collections.Generic;

namespace WordFrequencyCounter.Core.ChunkProcessing
{
    /// <summary>
    /// A chunk results processor.
    /// </summary>
    public interface IChunkResultMerger
    {
        /// <summary>
        /// Merges chunks of word frequency stats.
        /// </summary>
        /// <param name="chunkResults">An array of <see cref="ChunkResult"/> chunk processing results</param>
        /// <returns>Word frequency list as an <see cref="IDictionary{string, int}"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the chunkResults is null</exception>
        IDictionary<string, int> Merge(ChunkResult[] chunkResults);
    }
}