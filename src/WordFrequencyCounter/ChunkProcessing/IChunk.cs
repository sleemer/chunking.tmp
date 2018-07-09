using System.Collections.Generic;

namespace WordFrequencyCounter.ChunkProcessing
{
    /// <summary>
    /// A chunk of file.
    /// </summary>
    public interface IChunk
    {
        /// <summary>
        /// Reads the lines of a chunk.
        /// </summary>
        IEnumerable<string[]> ReadLines();
    }
}