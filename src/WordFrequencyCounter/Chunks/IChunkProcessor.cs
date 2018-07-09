using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WordFrequencyCounter.Chunks
{
    public interface IChunkProcessor
    {
        IDictionary<string, int> Process(BlockingCollection<string[]> chunks);
    }
}