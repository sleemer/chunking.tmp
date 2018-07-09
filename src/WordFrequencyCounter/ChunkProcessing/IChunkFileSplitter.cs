namespace WordFrequencyCounter.ChunkProcessing
{
    /// <summary>
    /// A file splitter.
    /// </summary>
    public interface IChunkFileSplitter
    {
        /// <summary>
        /// Split file into chunks.
        /// </summary>
        /// <param name="filePath">A path to the file</param>
        /// <returns>An array of <see cref="IChunk"/> chunks</returns>
        /// <exception cref="ArgumentNullException">Thrown when the filePath is null</exception>
        /// <exception cref="FileNotFouneException">Thrown when the file not found</exception>
        IChunk[] SplitIntoChunks(string filePath);
        /// <summary>
        /// Split file into chunks.
        /// </summary>
        /// <param name="filePath">A path to the file</param>
        /// <param name="chunkSize">Size of the chunk</param>
        /// <returns>An array of <see cref="IChunk"/> chunks</returns>
        /// <exception cref="ArgumentNullException">Thrown when the filePath is null</exception>
        /// <exception cref="FileNotFouneException">Thrown when the file not found</exception>
        IChunk[] SplitIntoChunks(string filePath, long chunkSize);
    }
}