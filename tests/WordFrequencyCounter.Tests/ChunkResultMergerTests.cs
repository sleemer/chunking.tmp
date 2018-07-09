using System;
using System.Collections.Generic;
using System.Linq;
using WordFrequencyCounter.ChunkProcessing;
using Xunit;

namespace WordFrequencyCounter.Tests
{
    public class ChunkResultMergerTests
    {
        [Theory]
        [InlineData("   ", "")]
        [InlineData("aaa  ", "aaa:1")]
        [InlineData("aaa bbb:1,ccc:2 ccc; aaa:2 ccc", "aaa:3,bbb:1,ccc:4")]
        [InlineData("aaa bbb:1,ccc:2 cc;c aaa:2 ccc", "aaa:3,bbb:1,ccc:4")]
        [InlineData("aAa bbb:1,ccc:2 ZZ;z aaa:2 cCc", "aaa:3,bbb:1,ccc:3,zzz:1")]
        public void ShouldMerge(string input, string strExpected)
        {
            // arrange
            var expected = strExpected.ToDictionary();
            var chunkResults = input.ToChunkResultArray();
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