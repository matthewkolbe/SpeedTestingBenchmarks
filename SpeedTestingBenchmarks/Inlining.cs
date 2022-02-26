using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTestingBenchmark
{
    public class Inlining
    {
        double a, b;

        [GlobalSetup]
        public void Setup()
        {
            this.a = 5;
            this.b = 2;
        }

        [Benchmark]
        public double InlineShort()
        {
            return InlineSum(this.a, this.b);
        }

        [Benchmark]
        public double NoInlineShort()
        {
            return Sum(this.a, this.b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double InlineSum(double x, double y)
        {
            return x + y;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static double Sum(double x, double y)
        {
            return x + y;
        }
    }
}
