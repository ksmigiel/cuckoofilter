using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace CuckooFilter.Benchmark
{
    public class Test
    {
        [Benchmark]
        public void Insertion()
        {
            var cf = new CuckooFilter();
            for (int i = 0; i < 1000000; i++)
            {
                cf.Insert(new byte[] { 0, 255, 0 });
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var results = BenchmarkRunner.Run<Test>();
        }
    }
}
