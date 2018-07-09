using System;
using System.Collections.Generic;

namespace WordFrequencyCounter.ChunkProcessing
{
    public struct ChunkResult
    {
        public ChunkResult(string firstWord, string lastWord, IDictionary<string, int> words)
        {
            FirstWord = firstWord ?? throw new ArgumentNullException(nameof(firstWord));
            LastWord = lastWord ?? throw new ArgumentNullException(nameof(lastWord));
            Words = words ?? throw new ArgumentNullException(nameof(words));
        }

        public readonly string FirstWord;
        public readonly string LastWord;
        public readonly IDictionary<string, int> Words;
    }
}