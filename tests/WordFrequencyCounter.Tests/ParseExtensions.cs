using System;
using System.Collections.Generic;
using System.Linq;
using WordFrequencyCounter.Core.ChunkProcessing;
using Xunit;

namespace WordFrequencyCounter.Tests
{
    internal static class ParseExtensions
    {
        public static IDictionary<string, int> ToDictionary(this string input)
        {
            return string.IsNullOrWhiteSpace(input)
                ? new Dictionary<string, int>()
                : input.Split(',')
                       .Select(pair => pair.Split(':'))
                       .ToDictionary(pair => pair[0].ToLower(), pair => int.Parse(pair[1]));
        }

        public static ChunkResult ToChunkResult(this string input)
        {
            return new ChunkResult(input.ToDictionary());
        } 

        public static ChunkResult[] ToChunkResultArray(this string input)
        {
            return input.Split(' ')
                        .Select(line => line.ToChunkResult())
                        .ToArray();
        }
    }
}