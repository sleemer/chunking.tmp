using System.Collections.Generic;

namespace WordFrequencyCounter.Chunks
{
    public interface IChunkMerger
    {
        IDictionary<string, int> Merge(IEnumerable<IDictionary<string, int>> chunkResults);
    }
}