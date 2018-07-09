using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using WordFrequencyCounter.ChunkProcessing;
using Xunit;

namespace WordFrequencyCounter.Tests
{
    public class ChunkProcessorTests
    {
        [Theory]
        [InlineData("aaa ccc", "aaa", "ccc", "")]
        [InlineData(" aaa ccc ", "", "", "aaa:1;ccc:1")]
        [InlineData("aaa;bbb ccc", "aaa", "ccc", "bbb:1")]
        [InlineData(" aaa Aaa AAA bbb;Bbba AAA bbb ccc", "", "ccc", "aaa:4;bbb:2;bbba:1")]
        public void ShouldProcess(string lines, string expectedFirstWord, string expectedLastWord, string expectedWords)
        {
            // arrange
            var expected = new ChunkResult(
                expectedFirstWord,
                expectedLastWord,
                string.IsNullOrWhiteSpace(expectedWords)
                    ? new Dictionary<string, int>()
                    : expectedWords.Split(';').Select(x => x.Split(':')).ToDictionary(x => x[0], x => int.Parse(x[1])));
            var chunk = new Mock<IChunk>();
            chunk.Setup(x => x.ReadLines()).Returns(lines.Split(';').Select(line => line.ToLower().Split(' ')));
            var chunkProcessor = new ChunkProcessor();

            // act
            var actual = chunkProcessor.Process(chunk.Object);

            // assert
            Assert.Equal(expected.FirstWord, actual.FirstWord);
            Assert.Equal(expected.LastWord, actual.LastWord);
            Assert.Equal(expected.Words, actual.Words);
        }

        [Fact]
        public void ShouldThrow_WhenChunkIsNull()
        {
            // arrange
            var chunkProcessor = new ChunkProcessor();

            // act & assert
            Assert.Throws<ArgumentNullException>(() => chunkProcessor.Process(chunk: null));
        }

        [Fact]
        public void ShouldThrow_WhenChunkReadLinesReturnsNull()
        {
            // arrange
            var chunk = new Mock<IChunk>();
            chunk.Setup(x => x.ReadLines()).Returns<IEnumerable<string>>(null);
            var chunkProcessor = new ChunkProcessor();

            // act & assert
            var actual = Assert.Throws<ArgumentException>(() => chunkProcessor.Process(chunk.Object));
            Assert.Equal("Chunk's ReadLines shouldn't return null!\nParameter name: chunk", actual.Message);
        }
    }
}