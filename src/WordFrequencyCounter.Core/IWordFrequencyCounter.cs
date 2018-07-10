using System.Collections.Generic;

namespace WordFrequencyCounter.Core
{
    /// <summary>
    /// An algorithm to calculate word frequency list for a file.
    /// </summary>
    public interface IWordFrequencyCounter
    {
        /// <summary>
        /// Calculates word frequency list for a file.
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>Word frequency list as <see cref="IDictionary{string, int}"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the filePath is null</exception>
        /// <exception cref="FileNotFoundException">Thrown when the file not found</exception>
        /// <exception cref="InvalidOperationException">Thrown when the error ocurres during processing</exception>
        IDictionary<string, int> Process(string filePath);
    }
}