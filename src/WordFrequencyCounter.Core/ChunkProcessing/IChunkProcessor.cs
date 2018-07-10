using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WordFrequencyCounter.Core.ChunkProcessing
{
    /// <summary>
    /// A word frequency processor for a chunk of text.
    /// </summary>
    public interface IChunkProcessor
    {
        /// <summary>
        /// Calculates word frequency stats for a group of chunks.
        /// </summary>
        /// <param name="chunks">A <see cref="BlockingCollection{string}"/> of text chunks to subscribe to</param>
        /// <returns>Chunk processing results <see cref="ChunkResult"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the chunks is null</exception>
        ChunkResult Process(BlockingCollection<string> chunks);
    }
}