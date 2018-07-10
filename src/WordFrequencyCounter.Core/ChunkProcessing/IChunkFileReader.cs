using System.Collections.Concurrent;

namespace WordFrequencyCounter.Core.ChunkProcessing
{
    /// <summary>
    /// A chunk file reader.
    /// </summary>
    public interface IChunkFileReader
    {
        /// <summary>
        /// Reads and splits a text file into the stream of text chunks.
        /// </summary>
        /// <param name="filePath">A path to the file</param>
        /// <param name="chunks">A <see cref="BlockingCollection{string}"/> of chunks where to publish splitted chunks</param>
        /// <exception cref="ArgumentNullException">Thrown when the filePath is null</exception>
        /// <exception cref="FileNotFouneException">Thrown when the file not found</exception>
        void ReadAll(string filePath, BlockingCollection<string> chunks);
    }
}