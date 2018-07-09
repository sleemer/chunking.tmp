using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using WordFrequencyCounter.ChunkProcessing;

namespace WordFrequencyCounter.PerfTests
{
    [MemoryDiagnoser]
    public class WordFrequencyCounterPerfTests
    {
        public const string InputFilePath = "/Users/sleemer/Projects/dotnet/broadridge/samples/source.txt";

        [Benchmark(Baseline = true)]
        public void SimpleWordFrequencyCounterTest()
        {
            var wordCounter = new SimpleWordFrequencyCounter();
            wordCounter.Process(InputFilePath);
        }

        [Benchmark]
        public void ChunkWordFrequencyCounterTest()
        {
            var wordCounter = new ChunkWordFrequencyCounter(
                new MemoryMappedFileChunkSplitter(), 
                new ChunkProcessor(), 
                new ChunkResultMerger());
            wordCounter.Process(InputFilePath);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<WordFrequencyCounterPerfTests>();
        }
    }
}
