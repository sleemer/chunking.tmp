using System;
using System.Collections.Generic;

namespace WordFrequencyCounter.Core.ChunkProcessing
{
    /// <summary>
    /// A result of processing a chunk of text in a file.
    /// </summary>
    public struct ChunkResult
    {
        public ChunkResult(IDictionary<string, int> words)
        {
            if(words == null) throw new ArgumentNullException(nameof(words)); 

            WordFrequencies = words;
        }

        /// <summary>
        /// Contains word frequencies for a chunk of text in a file.
        /// </summary>
        public readonly IDictionary<string, int> WordFrequencies;
    }
}