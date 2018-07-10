using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordFrequencyCounter.Core.ChunkProcessing;

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
            var wordCounter = new Core.ChunkProcessing.ChunkWordFrequencyCounter(
                new SimpleChunkFileReader(), 
                new ChunkProcessor(), 
                new ChunkResultMerger());
            return wordCounter.Process(filePath);

            // return new SimpleWordFrequencyCounter().Process(filePath);
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
