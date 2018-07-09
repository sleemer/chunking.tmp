using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WordFrequencyCounter.ChunkProcessing;

namespace WordFrequencyCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args?.Length != 2)
            {
                Console.WriteLine("There are should be two arguments:");
                Console.WriteLine("- first  : name of the input file");
                Console.WriteLine("- second : name of the output file");
                return;
            }

            var inputFile = args[0];
            var outputFile = args[1];

            var dictionary = CountWordFrequency(inputFile);
            var sortedWordFrequencies = SortResults(dictionary);
            SaveResults(outputFile, sortedWordFrequencies);
        }

        private static IDictionary<string, int> CountWordFrequency(string filePath)
        {
            // in real world program we should use DI instead
            // var wordCounter = new ChunkWordFrequencyCounter(
            //     new MemoryMappedFileChunkSplitter(), 
            //     new ChunkProcessor(), 
            //     new ChunkResultMerger());
            // return wordCounter.Process(filePath);

            // var wordCounter = new WordFrequencyCounter.Chunks.WordFrequencyCounter(
            //     new WordFrequencyCounter.Chunks.FileChunkReader(),
            //     new WordFrequencyCounter.Chunks.ChunkProcessor(),
            //     new WordFrequencyCounter.Chunks.ChunkMerger());
            // return wordCounter.Process(filePath);

            // var reader = new WordFrequencyCounter.Chunks.FileChunkReader();
            // var chunks = new BlockingCollection<string[]>();
            // var timer = Stopwatch.StartNew();
            // reader.ReadAll(filePath, chunks);
            // timer.Stop();
            // Console.WriteLine($"Chunks: reading of {filePath} took {timer.ElapsedMilliseconds} ms");

            // var chunksCount = chunks.Count;
            // Console.WriteLine($"Chunks: there are {chunksCount} chunks");

            // var dictionary = new Dictionary<string, int>();
            // timer.Restart();
            // foreach (var chunk in chunks.GetConsumingEnumerable())
            // {
            //     WordFrequencyCounter.Chunks.ChunkProcessor.ProcessChunk(chunk, dictionary);
            // }
            // timer.Stop();
            // Console.WriteLine($"Chunks: processing of {chunksCount} took {timer.Elapsed}");
            // Console.WriteLine($"Chunks: processing of one chunk took around {(timer.Elapsed / chunksCount).TotalMilliseconds} ms");

            // return dictionary;

            return new SimpleWordFrequencyCounter().Process(filePath);
        }

        private static IEnumerable<KeyValuePair<string, int>> SortResults(IDictionary<string, int> dictionary)
        {
            return dictionary.OrderByDescending(x => x.Value)
                             .ThenBy(x => x.Key)
                             .ToArray();
        }

        private static void SaveResults(string filePath, IEnumerable<KeyValuePair<string, int>> wordFrequencies)
        {
            using (var stream = File.OpenWrite(filePath))
            using (var writer = new StreamWriter(stream))
            {
                foreach (var frequency in wordFrequencies)
                {
                    writer.WriteLine($"{frequency.Key},{frequency.Value}");
                }
            }
        }
    }
}
