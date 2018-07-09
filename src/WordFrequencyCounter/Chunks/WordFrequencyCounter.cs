using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WordFrequencyCounter.Chunks
{
    public sealed class WordFrequencyCounter : IWordFrequencyCounter
    {
        private readonly IFileChunkReader _fileChunkReader;
        private readonly IChunkProcessor _chunkProcessor;
        private readonly IChunkMerger _chunkResultMerger;

        public WordFrequencyCounter(IFileChunkReader fileChunkReader,
                                    IChunkProcessor chunkProcessor,
                                    IChunkMerger chunkResultMerger)
        {
            _fileChunkReader   = fileChunkReader   ?? throw new ArgumentNullException(nameof(fileChunkReader));
            _chunkProcessor    = chunkProcessor    ?? throw new ArgumentNullException(nameof(chunkProcessor));
            _chunkResultMerger = chunkResultMerger ?? throw new ArgumentNullException(nameof(chunkResultMerger));
        }

        public IDictionary<string, int> Process(string filePath)
        {
            try
            {
                var parallelism = 8;
                var chunks = new BlockingCollection<string[]>();
                IDictionary<string, int>[] chunkResults = new Dictionary<string, int>[parallelism];
                var readerTask = Task.Run(()=> _fileChunkReader.ReadAll(filePath, chunks));
                var processingTasks = Enumerable.Range(0, parallelism)
                                                .Select(index => Task.Run(()=>chunkResults[index] = _chunkProcessor.Process(chunks)));
                Task.WaitAll(processingTasks.Concat(new Task[]{readerTask}).ToArray());
                return _chunkResultMerger.Merge(chunkResults);
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