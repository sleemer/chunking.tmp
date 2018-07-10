using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Moq;
using WordFrequencyCounter.Core.ChunkProcessing;
using Xunit;

namespace WordFrequencyCounter.Tests
{
    public class ChunkWordFrequencyCounterTests
    {
        [Theory]
        [InlineData("../../../../../samples/aaa200.txt", "aaa:200")]
        [InlineData("../../../../../samples/aaa100bbb50ccc10.txt", "aaa:100,bbb:50,ccc:10")]
        public void ShouldProcessFile(string inputFile, string expectedData)
        {
            // arrange
            var expected       = expectedData.ToDictionary();
            var chunkReader    = new SimpleChunkFileReader();
            var chunkProcessor = new ChunkProcessor();
            var chunkMerger    = new ChunkResultMerger();
            var wordCounter    = new ChunkWordFrequencyCounter(chunkReader, chunkProcessor, chunkMerger);

            // act
            var actual = wordCounter.Process(inputFile);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldThrow_WhenInputFileNotFound()
        {
            // arrange
            var chunkReader    = new SimpleChunkFileReader();
            var chunkProcessor = new ChunkProcessor();
            var chunkMerger    = new ChunkResultMerger();
            var wordCounter    = new ChunkWordFrequencyCounter(chunkReader, chunkProcessor, chunkMerger);
            
            // act & assert
            Assert.Throws<FileNotFoundException>(() => wordCounter.Process("not.existed")); 
        }
    }
}
