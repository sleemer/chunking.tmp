using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WordFrequencyCounter.Core.ChunkProcessing
{
    public sealed class ChunkWordFrequencyCounter : IWordFrequencyCounter
    {
        private readonly IChunkFileReader _chunkFileReader;
        private readonly IChunkProcessor _chunkProcessor;
        private readonly IChunkResultMerger _chunkResultMerger;

        public ChunkWordFrequencyCounter(IChunkFileReader chunkFileReader,
                                         IChunkProcessor chunkProcessor,
                                         IChunkResultMerger chunkResultMerger)
        {
            if (chunkFileReader == null) throw new ArgumentNullException(nameof(chunkFileReader));
            if (chunkProcessor == null) throw new ArgumentNullException(nameof(chunkProcessor));
            if (chunkResultMerger == null) throw new ArgumentNullException(nameof(chunkResultMerger));

            _chunkFileReader = chunkFileReader;
            _chunkProcessor = chunkProcessor;
            _chunkResultMerger = chunkResultMerger;
        }

        public IDictionary<string, int> Process(string filePath)
        {
            try
            {
                var chunksCount = 8;
                var chunks = new BlockingCollection<string>();
                var chunkResults = new ChunkResult[chunksCount];
                var readerTask = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        _chunkFileReader.ReadAll(filePath, chunks);
                    }
                    finally
                    {
                        chunks.CompleteAdding();
                    }
                });
                Parallel.For(
                    0,
                    chunksCount,
                    index => chunkResults[index] = _chunkProcessor.Process(chunks));
                readerTask.Wait();
                return _chunkResultMerger.Merge(chunkResults);
            }
            catch (ArgumentNullException) { throw; }
            catch (FileNotFoundException) { throw; }
            catch (AggregateException ex) { throw ex.Flatten().InnerException; }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occured during calculating word frequency list for the file {filePath}", ex);
            }
        }
    }
}