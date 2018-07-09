using System.Collections.Concurrent;

namespace WordFrequencyCounter.Chunks
{
    public interface IFileChunkReader
    {
        void ReadAll(string fileName, BlockingCollection<string[]> chunks);
    }
}