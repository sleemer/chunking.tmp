using System;
using System.Collections.Generic;
using System.Linq;
using WordFrequencyCounter.Core.ChunkProcessing;
using Xunit;

namespace WordFrequencyCounter.Tests
{
    public class ChunkResultMergerTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("aaa:1", "aaa:1")]
        [InlineData("aaa:1,bbb:1,ccc:2 ccc:1 aaa:2,ccc:1", "aaa:3,bbb:1,ccc:4")]
        public void ShouldMerge(string input, string strExpected)
        {
            // arrange
            var chunkResults = input.ToChunkResultArray();
            var expected = strExpected.ToDictionary();
            
            var chunkResultMerger = new ChunkResultMerger();

            // act
            var actual = chunkResultMerger.Merge(chunkResults);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldThrow_WhenChunkResultsAreNull()
        {
            // arrange
            var chunkResultMerger = new ChunkResultMerger();

            // act & assert
            Assert.Throws<ArgumentNullException>(() => chunkResultMerger.Merge(chunkResults: null));
        }
    }
}