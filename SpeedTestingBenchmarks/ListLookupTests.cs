using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTestingBenchmark
{
    public class ExpRangeLookup
    {
        //|           Method |    x |       Mean |     Error |    StdDev |
        //|----------------- |----- |-----------:|----------:|----------:|
        //|   FindHashedFast | 1.15 |  3.4917 ns | 0.0484 ns | 0.0429 ns |
        //|       FindHashed | 1.15 |  8.1438 ns | 0.0102 ns | 0.0096 ns |
        //| FindBinarySearch | 1.15 | 26.8559 ns | 0.0304 ns | 0.0254 ns |
        //|          FastLog | 1.15 |  0.8595 ns | 0.0031 ns | 0.0028 ns |
        //|      RegualarLog | 1.15 |  4.6736 ns | 0.0240 ns | 0.0213 ns |

        [GlobalSetup]
        public void Setup()
        {
            Grid.Init();
        }

        [Benchmark]
        [ArgumentsSource(nameof(Numbers))]
        public uint FindHashedFast(double x)
        {
            double value = 0.0;
            uint index = 0;

            Grid.GetClosestNode(x, ref index, ref value);

            return index;
        }

        [Benchmark]
        [ArgumentsSource(nameof(Numbers))]
        public uint FindHashed(double x)
        {
            double value = 0.0;
            uint index = 0;

            Grid.GetClosestNode2(x, ref index, ref value);

            return index;
        }

        [Benchmark]
        [ArgumentsSource(nameof(Numbers))]
        public int FindBinarySearch(double x)
        {
            return Grid.cache.BinarySearchClosest(x);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Numbers))]
        public double FastLogCache(double x)
        {
            return Grid.MyLog(x);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Numbers))]
        public double RegualarLog(double x)
        {
            return Grid.MyLog2(x);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Numbers))]
        public double FastLogFormula(double x)
        {
            return Grid.MyLog3(x);
        }

        public IEnumerable<object[]> Numbers() // for multiple arguments it's an IEnumerable of array of objects (object[])
        {
            yield return new object[] { 1.15 };
        }
    }

    public static class Ext
    {
        public unsafe static int BinarySearchClosest(in double* values, int n, in double key)
        {
            var lower_bound = 0;
            var upper_bound = n - 1;

            if (upper_bound < lower_bound)
                return -1; // empty list

            var initial_lower_test = values[lower_bound].CompareTo(key);
            if (initial_lower_test > 0)
                return lower_bound; // item below lower bound of array
            else if (initial_lower_test == 0)
                return lower_bound; // item matches lower bound of array

            var initial_upper_test = values[upper_bound].CompareTo(key);
            if (initial_upper_test < 0)
                return upper_bound; // item above upper bound of array
            else if (initial_upper_test == 0)
                return upper_bound; // item matches upper bound of array

            while (lower_bound < upper_bound - 1)
            {
                var test_index = lower_bound + (upper_bound - lower_bound) / 2;

                var test_result = values[test_index].CompareTo(key);

                if (test_result == 0)
                {
                    return test_index;
                }
                else if (test_result > 0)
                {
                    upper_bound = test_index;
                }
                else if (test_result < 0)
                {
                    lower_bound = test_index;
                }
            }

            if (values[lower_bound].CompareTo(key) == 0)
                return lower_bound;
            else if (Math.Abs(values[lower_bound] - key) < Math.Abs(values[upper_bound] - key))
                return lower_bound;
            else
                return upper_bound;
        }

        public static int BinarySearchClosest(this double[] values, double key)
        {
            var n = values.Length;
            var lower_bound = 0;
            var upper_bound = n - 1;

            if (upper_bound < lower_bound)
                return -1; // empty list

            var initial_lower_test = values[lower_bound].CompareTo(key);
            if (initial_lower_test > 0)
                return lower_bound; // item below lower bound of array
            else if (initial_lower_test == 0)
                return lower_bound; // item matches lower bound of array

            var initial_upper_test = values[upper_bound].CompareTo(key);
            if (initial_upper_test < 0)
                return upper_bound; // item above upper bound of array
            else if (initial_upper_test == 0)
                return upper_bound; // item matches upper bound of array

            while (lower_bound < upper_bound - 1)
            {
                var test_index = lower_bound + (upper_bound - lower_bound) / 2;

                var test_result = values[test_index].CompareTo(key);

                if (test_result == 0)
                {
                    return test_index;
                }
                else if (test_result > 0)
                {
                    upper_bound = test_index;
                }
                else if (test_result < 0)
                {
                    lower_bound = test_index;
                }
            }

            if (values[lower_bound].CompareTo(key) == 0)
                return lower_bound;
            else if (Math.Abs(values[lower_bound] - key) < Math.Abs(values[upper_bound] - key))
                return lower_bound;
            else
                return upper_bound;
        }
    }

    public class Grid
    {
        // This requires a bit of an explanation.
        //
        // In order to have searches be extremely fast, we create a list where index i->f(i)=x, 
        // and f^-1(x) is known. So that f((int)f^-1(x)) is a very close value to the one we're
        // searching for and thus (int)f^-1(x) is the index of the key-value pair of information
        // we need to access.

        public const int DOWN_VALUE_COUNT = 247;
        const int LOG_CACHE_SIZE = 10000;
        const int UP_VALUE_COUNT = 300;
        public const int ALL_VALUE_COUNT = 548; // DOWN_VALUE_COUNT + UP_VALUE_COUNT + 1

        public static readonly double[] cache = new double[ALL_VALUE_COUNT];
        public static readonly double[] logcache = new double[LOG_CACHE_SIZE];

        public static void Init()
        {
            for (int ii = -DOWN_VALUE_COUNT; ii <= UP_VALUE_COUNT; ++ii)
                cache[ii + DOWN_VALUE_COUNT] = Node(ii);

            for (int ii = 0; ii < LOG_CACHE_SIZE; ++ii)
            {
                // divide by 5000
                logcache[ii] = Math.Log(0.01 * (ii + 1) + 1.0, 1.016); // i.e. Math.Log(50 * ii / 1000 + 1.0, 1.016)
            }
        }

        public static void GetClosestNode(double y, ref uint index, ref double value)
        {
            var yy = y - 1.0;

            if (yy < 0)
            {
                var x = (uint)(MyLog(-yy)+0.5);
                x = Math.Min(x, 300U);
                value = 1.0 - cache[x + 247];
                index = Math.Max(247U - x, 0U);
            }
            else
            {
                var x = (uint)(MyLog(yy)+0.5);
                x = Math.Min(x, 300U);
                value = 1.0 + cache[x + 247];
                index = 247 + x;
            }
        }

        public static void GetClosestNode2(double y, ref uint index, ref double value)
        {
            var yy = y - 1.0;

            if (yy < 0)
            {
                var x = (uint)(MyLog2(-yy)+0.5);
                x = Math.Min(x, 300U);
                value = 1.0 - cache[x + 247];
                index = Math.Max(247U - x, 0U);
            }
            else
            {
                var x = (uint)(MyLog2(yy)+0.5);
                x = Math.Min(x, 300U);
                value = 1.0 + cache[x + 247];
                index = 247 + x;
            }
        }

        public static double MyLog(double x)
        {
            var dn = (int)(x * 5000);
            dn = dn > LOG_CACHE_SIZE - 1 ? LOG_CACHE_SIZE - 1 : dn;

            return logcache[dn];
        }

        public static double MyLog2(double x)
        {
            return Math.Log(50 * x + 1.0, 1.016);
        }

        public static double MyLog3(double x)
        {
            return fast_log(50 * (float)x + 1.0f) * 62.99867723f;
        }

        static double Node(int ii)
        {
            if (ii < 0)
                return -(Math.Pow(1.016, -ii) - 1.0) / 50.0;
            else
                return (Math.Pow(1.016, ii) - 1.0) / 50.0;
        }

        public static unsafe void GetN(int n, double mean, double stdev, ref double* values, ref uint* indexes)
        {
            int n2 = n / 2;
            for (int i = -n2; i < n2 + 1; ++i)
                GetClosestNode(mean + i * stdev, ref indexes[i + n2], ref values[i + n2]);
        }

        public static unsafe float fast_log(float val)
        {
            int* exp_ptr = ((int*)(void*)&val);
            int x = *exp_ptr;
            int log_2 = ((x >> 23) &255) -128;
            x &= ~(255 << 23);
            x += 127 << 23;
            *exp_ptr = x;

            val = ((-1.0f / 3) * val + 2) * val - 2.0f / 3;   // (1)

            return (val + log_2) * 0.69314718f;
        }
    }
}
