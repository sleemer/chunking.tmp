using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordFrequencyCounter.ChunkProcessing
{
    public sealed class ChunkWordFrequencyCounter : IWordFrequencyCounter
    {
        private readonly IChunkFileSplitter _chunkFileSplitter;
        private readonly IChunkProcessor _chunkProcessor;
        private readonly IChunkResultMerger _chunkMerger;

        public ChunkWordFrequencyCounter(IChunkFileSplitter chunkFileSplitter,
                                         IChunkProcessor chunkFileProcessor,
                                         IChunkResultMerger chunkMerger)
        {
            _chunkFileSplitter = chunkFileSplitter ?? throw new ArgumentNullException(nameof(chunkFileSplitter));
            _chunkProcessor = chunkFileProcessor ?? throw new ArgumentNullException(nameof(chunkFileProcessor));
            _chunkMerger = chunkMerger ?? throw new ArgumentNullException(nameof(chunkMerger));
        }

        public IDictionary<string, int> Process(string filePath)
        {
            try
            {
                var chunks = _chunkFileSplitter.SplitIntoChunks(filePath);

                var chunkResults = new ChunkResult[chunks.Length];
                Parallel.For(
                    0,
                    chunks.Length,
                    index => chunkResults[index] = _chunkProcessor.Process(chunks[index]));

                return _chunkMerger.Merge(chunkResults);
            }
            catch (ArgumentNullException) { throw; }
            catch (FileNotFoundException) { throw; }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"An error occured during calculating word frequency list for the file {filePath}", ex);
            }
        }
    }
}