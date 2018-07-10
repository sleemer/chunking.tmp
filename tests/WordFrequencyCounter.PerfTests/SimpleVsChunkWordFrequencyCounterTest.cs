using BenchmarkDotNet.Attributes;
using WordFrequencyCounter.Core.ChunkProcessing;

namespace WordFrequencyCounter.PerfTests
{
    [MemoryDiagnoser]
    public class SimpleVsChunkWordFrequencyCounterTest
    {
        public const string InputFilePath = "/Users/sleemer/Projects/dotnet/broadridge.job.coding.tasks/samples/source.txt";

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
                new SimpleChunkFileReader(), 
                new ChunkProcessor(), 
                new ChunkResultMerger());
            wordCounter.Process(InputFilePath); 
        }

        [Benchmark]
        public void BufferedChunkWordFrequencyCounterTest()
        {
            var wordCounter = new ChunkWordFrequencyCounter(
                new BufferedChunkFileReader(), 
                new ChunkProcessor(), 
                new ChunkResultMerger());
            wordCounter.Process(InputFilePath); 
        }
    }
}