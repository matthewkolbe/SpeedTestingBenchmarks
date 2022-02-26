using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTestingBenchmark
{
    public class ModTest
    {
        int d0, d1, d2, d3, d4, d5, d6, d7;

        [Params(100)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            d0 = 0;
            d1 = N / 7;
            d2 = N / 6;
            d3 = N / 5;
            d4 = N / 4;
            d5 = N / 3;
            d6 = N / 2;
            d7 = N;
        }

        [Benchmark]
        public int Baseline()
        {
            int i = d0 % 100;
            i += d1 % 100;
            i += d2 % 100;
            i += d3 % 100;
            i += d4 % 100;
            i += d5 % 100;
            i += d6 % 100;
            i += d7 % 100;

            return i;
        }

        [Benchmark]
        public int TestIfProtectedMod()
        {
            int i = d0 < 100 ? d0 : d0 % 100;
            i += d1 < 100 ? d1 : d1 % 100;
            i += d2 < 100 ? d2 : d2 % 100;
            i += d3 < 100 ? d3 : d3 % 100; 
            i += d4 < 100 ? d4 : d4 % 100;
            i += d5 < 100 ? d5 : d5 % 100;
            i += d6 < 100 ? d6 : d6 % 100;
            i += d7 < 100 ? d7 : d7 % 100;

            return i;
        }
    }

    public class Mod8Test
    {
        uint d0,
             d1, 
             d2,
             d3,
             d4,
             d5,
             d6,
             d7;

        int id0, 
            id1, 
            id2, 
            id3, 
            id4, 
            id5, 
            id6, 
            id7;

        [Params(100)]
        public uint N;

        [GlobalSetup]
        public void Setup()
        {
            d0 = 0;
            d1 = N / 7;
            d2 = N / 6;
            d3 = N / 5;
            d4 = N / 4;
            d5 = N / 3;
            d6 = N / 2;
            d7 = N;

            id0 = (int)0;
            id1 = (int)N / 7;
            id2 = (int)N / 6;
            id3 = (int)N / 5;
            id4 = (int)N / 4;
            id5 = (int)N / 3;
            id6 = (int)N / 2;
            id7 = (int)N;
        }

        [Benchmark]
        public uint BaselineUInt()
        {
            uint i = d0 % 8;
            //i += d1 % 8;
            //i += d2 % 8;
            //i += d3 % 8;
            //i += d4 % 8;
            //i += d5 % 8;
            //i += d6 % 8;
            //i += d7 % 8;

            return i;
        }

        [Benchmark]
        public uint TestAndOpModUInt()
        {
            uint i = d0 & 7;
            //i += d1 & 7;
            //i += d2 & 7;
            //i += d3 & 7;
            //i += d4 & 7;
            //i += d5 & 7;
            //i += d6 & 7;
            //i += d7 & 7;

            return i;
        }

        [Benchmark]
        public int BaselineInt()
        {
            int i = id0 % 8;
            //i += id1 % 8;
            //i += id2 % 8;
            //i += id3 % 8;
            //i += id4 % 8;
            //i += id5 % 8;
            //i += id6 % 8;
            //i += id7 % 8;

            return i;
        }

        [Benchmark]
        public int TestAndOpModInt()
        {
            int i = id0 & 7;
            //i += id1 & 7;
            //i += id2 & 7;
            //i += id3 & 7;
            //i += id4 & 7;
            //i += id5 & 7;
            //i += id6 & 7;
            //i += id7 & 7;

            return i;
        }
    }
}
