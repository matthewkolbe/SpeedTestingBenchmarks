using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTestingBenchmark
{
    public class InterfaceCost
    {
        //|             Method |  a |      Mean |     Error |    StdDev |
        //|------------------- |--- |----------:|----------:|----------:|
        //|         Interfaced | 30 | 0.4314 ns | 0.0021 ns | 0.0020 ns |
        //|             Direct | 30 | 0.0111 ns | 0.0076 ns | 0.0071 ns |
        //| NoInlineInterfaced | 30 | 0.4250 ns | 0.0022 ns | 0.0019 ns |
        //|     NoInlineDirect | 30 | 0.8532 ns | 0.0026 ns | 0.0022 ns |

        IMult interfaced;
        Mult direct;

        [GlobalSetup]
        public void Setup()
        {
            this.interfaced = new Mult();
            this.direct = new Mult();
        }

        [Benchmark]
        [ArgumentsSource(nameof(Numbers))]
        public uint Interfaced(uint a)
        {
            return interfaced.Mult2(a);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Numbers))]
        public double Direct(uint a)
        {
            return direct.Mult2(a);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Numbers))]
        public uint NoInlineInterfaced(uint a)
        {
            return interfaced.NoInlineMult2(a);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Numbers))]
        public double NoInlineDirect(uint a)
        {
            return direct.NoInlineMult2(a);
        }

        public IEnumerable<object[]> Numbers() // for multiple arguments it's an IEnumerable of array of objects (object[])
        {
            yield return new object[] { 30U };
        }
    }

    public interface IMult
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        uint NoInlineMult2(uint x);

        uint Mult2(uint x);
    }

    public class Mult : IMult
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public uint NoInlineMult2(uint x)
        {
            return x << 1;
        }

        public uint Mult2(uint x)
        {
            return x << 1;
        }
    }
}
