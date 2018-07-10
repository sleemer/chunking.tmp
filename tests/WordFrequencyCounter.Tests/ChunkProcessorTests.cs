using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using WordFrequencyCounter.Core.ChunkProcessing;
using Xunit;

namespace WordFrequencyCounter.Tests
{
    public class ChunkProcessorTests
    {
        [Theory]
        [InlineData("aaa ccc", "aaa:1,ccc:1")]
        [InlineData(" aaa ccc ", "aaa:1,ccc:1")]
        [InlineData("aaa\nbbb ccc", "aaa:1,ccc:1,bbb:1")]
        [InlineData(" aaa Aaa AAA bbb\nBbba AAA bbb ccc", "aaa:4,bbb:2,ccc:1,bbba:1")]
        public void ShouldProcess(string lines, string expectedWords)
        {
            // arrange
            var expected = expectedWords.ToChunkResult();
            var chunks = new BlockingCollection<string>();
            foreach (var line in lines.Split(Environment.NewLine, StringSplitOptions.None)) chunks.Add(line);
            chunks.CompleteAdding();
            var chunkProcessor = new ChunkProcessor();

            // act
            var actual = chunkProcessor.Process(chunks);

            // assert
            Assert.Equal(expected.WordFrequencies, actual.WordFrequencies);
        }

        [Fact]
        public void ShouldThrow_WhenChunkIsNull()
        {
            // arrange
            var chunkProcessor = new ChunkProcessor();

            // act & assert
            Assert.Throws<ArgumentNullException>(() => chunkProcessor.Process(chunks: null));
        }
    }
}