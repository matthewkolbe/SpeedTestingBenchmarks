using Accord.Math;
using BenchmarkDotNet.Attributes;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTestingBenchmark
{
    [MemoryDiagnoser]
    public class DotProduct
    {
        //|      Method |      Mean |    Error |   StdDev | Allocated |
        //|------------ |----------:|---------:|---------:|----------:|
        //|         Avx |  73.13 ns | 0.120 ns | 0.112 ns |         - |
        //| AvxUnrolled |  67.76 ns | 0.184 ns | 0.172 ns |         - |
        //|      Accord | 300.51 ns | 1.811 ns | 1.694 ns |         - |
        //|     MathNet | 305.34 ns | 2.032 ns | 1.901 ns |         - |

        double[] a, b;
        DenseVector av, bv;
        const int N = 500;


        [GlobalSetup]
        public void Setup()
        {
            a = Enumerable.Range(0, N)
                     .Select(x => Convert.ToDouble(x) / 10.0)
                     .ToArray();
            b = Enumerable.Range(0, N)
                     .Select(x => Convert.ToDouble(x) / 100.0)
                     .ToArray();

            av = new DenseVector(a);
            bv = new DenseVector(b);
        }

        [Benchmark]
        public unsafe double Avx()
        {
            double r = 0.0;

            fixed (double* x = a) fixed (double* y = b)
                r = PointerOperators.DotAvx(x, y, N);

            return r;
        }

        [Benchmark]
        public unsafe double AvxUnrolled()
        {
            double r = 0.0;

            fixed (double* x = a) fixed (double* y = b)
                r = PointerOperators.DotAvxUnrolled(x, y, N);

            return r;
        }

        [Benchmark]
        public double Accord() => a.Dot(b);

        [Benchmark]
        public double MathNet() =>  av.DotProduct(bv);

        
    }
}
