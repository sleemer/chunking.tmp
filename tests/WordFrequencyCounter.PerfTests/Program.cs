using System;
using BenchmarkDotNet.Running;

namespace WordFrequencyCounter.PerfTests
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SimpleVsChunkWordFrequencyCounterTest>();
        }
    }
}
