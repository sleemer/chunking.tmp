namespace WordFrequencyCounter.ChunkProcessing
{
    /// <summary>
    /// A chunk word processor.
    /// </summary>
    public interface IChunkProcessor
    {
        /// <summary>
        /// Calculates word frequency stats for a chunk.
        /// </summary>
        /// <param name="chunk">A <see cref="IChunk"/> chunk</param>
        /// <returns>Chunk processing results <see cref="ChunkResult"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the chunk is null</exception>
        ChunkResult Process(IChunk chunk);
    }
}