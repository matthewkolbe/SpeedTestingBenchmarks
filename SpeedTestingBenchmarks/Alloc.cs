using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTestingBenchmark
{
    [MemoryDiagnoser]
    public unsafe class Alloc
    {
        //|               Method |         Mean |      Error |     StdDev |  Gen 0 |  Gen 1 | Allocated |
        //|--------------------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
        //|      SmallStackAlloc |     6.591 ns |  0.0928 ns |  0.0868 ns |      - |      - |         - |
        //|           SmallAlloc |    11.729 ns |  0.2497 ns |  0.2452 ns | 0.0220 |      - |     368 B |
        //| SmallObjectPoolAlloc |    15.290 ns |  0.0354 ns |  0.0331 ns |      - |      - |         - |
        //|        MidStackAlloc |   128.600 ns |  0.6952 ns |  0.6163 ns |      - |      - |         - |
        //|             MidAlloc |   150.519 ns |  2.6957 ns |  2.5215 ns | 0.4809 | 0.0072 |   8,048 B |
        //|   MidObjectPoolAlloc |    15.356 ns |  0.0625 ns |  0.0584 ns |      - |      - |         - |
        //|        BigStackAlloc | 4,231.779 ns | 22.2348 ns | 20.7985 ns |      - |      - |         - |
        //|             BigAlloc | 1,983.190 ns | 39.1362 ns | 36.6080 ns | 9.5215 |      - | 160,048 B |
        //|   BigObjectPoolAlloc |    15.279 ns |  0.0584 ns |  0.0517 ns |      - |      - |         - |


        const int BIGN = 10000;
        const int MIDN = 500;
        const int SMALLN = 20;
        DoubleArrayObjectPool big_pool, mid_pool, small_pool;

        [GlobalSetup]
        public void Setup()
        {
            big_pool = new DoubleArrayObjectPool(BIGN, 2);
            mid_pool = new DoubleArrayObjectPool(MIDN, 2);
            small_pool = new DoubleArrayObjectPool(SMALLN, 2);
        }


        [Benchmark]
        public unsafe double SmallStackAlloc()
        {
            double* a = stackalloc double[SMALLN];
            double* b = stackalloc double[SMALLN];

            return a[0] + b[0] + a[SMALLN - 1] + b[SMALLN - 1];
        }

        [Benchmark]
        public unsafe double SmallAlloc()
        {
            double[] a = new double[SMALLN];
            double[] b = new double[SMALLN];

            return a[0] + b[0] + a[SMALLN - 1] + b[SMALLN - 1];
        }

        [Benchmark]
        public double SmallObjectPoolAlloc()
        {
            small_pool.Get(out double[] a);
            small_pool.Get(out double[] b);

            double r = a[0] + b[0] + a[SMALLN - 1] + b[SMALLN - 1];

            small_pool.Add(a);
            small_pool.Add(b);

            return r;
        }

        [Benchmark]
        public unsafe double MidStackAlloc()
        {
            double* a = stackalloc double[MIDN];
            double* b = stackalloc double[MIDN];

            return a[0] + b[0] + a[MIDN - 1] + b[MIDN - 1];
        }

        [Benchmark]
        public double MidAlloc()
        {
            double[] a = new double[MIDN];
            double[] b = new double[MIDN];

            return a[0] + b[0] + a[MIDN - 1] + b[MIDN - 1];
        }

        [Benchmark]
        public double MidObjectPoolAlloc()
        {
            mid_pool.Get(out double[] a);
            mid_pool.Get(out double[] b);

            double r = a[0] + b[0] + a[MIDN - 1] + b[MIDN - 1];

            mid_pool.Add(a);
            mid_pool.Add(b);

            return r;
        }

        [Benchmark]
        public unsafe double BigStackAlloc()
        {
            double* a = stackalloc double[BIGN];
            double* b = stackalloc double[BIGN];

            return a[0] + b[0] + a[BIGN - 1] + b[BIGN - 1];
        }

        [Benchmark]
        public double BigAlloc()
        {
            double[] a = new double[BIGN];
            double[] b = new double[BIGN];

            return a[0] + b[0] + a[BIGN - 1] + b[BIGN - 1];
        }

        [Benchmark]
        public double BigObjectPoolAlloc()
        {
            big_pool.Get(out double[] a);
            big_pool.Get(out double[] b);

            double r = a[0] + b[0] + a[BIGN - 1] + b[BIGN - 1];

            big_pool.Add(a);
            big_pool.Add(b);

            return r;
        }
    }

    public class ArrayMemberAccess
    {
        // for n=8
        //|       Method |     Mean |     Error |    StdDev |
        //|------------- |---------:|----------:|----------:|
        //| StructAccess | 1.274 ns | 0.0076 ns | 0.0071 ns |
        //|  ClassAccess | 2.324 ns | 0.0163 ns | 0.0136 ns |


        FixedBuffer a;
        HeapBuffer b = new HeapBuffer();

        [Benchmark]
        public unsafe double StructAccess()
        {
            double r = 0.0;

            fixed (double* x = a.X) fixed (double* y = a.Y)
                r = PointerOperators.DotAvx(x, y, 8);

            return r;
        }

        [Benchmark]
        public unsafe double ClassAccess()
        {
            double r = 0.0;

            fixed (double* x = b.X) fixed (double* y = b.Y)
                r = PointerOperators.DotAvx(x, y, 8);

            return r;
        }
    }

    public unsafe struct FixedBuffer
    {
        public fixed double X[8];
        public fixed double Y[8];
    }

    public class HeapBuffer
    {
        public double[] X = new double[8];
        public double[] Y = new double[8];
    }

    public class ObjectPool<T> where T : class
    {
        private readonly Stack<T> objects;

        public ObjectPool()
        {
            this.objects = new Stack<T>();
        }

        public ObjectPool(IEnumerable<T> initial_items)
        {
            this.objects = new Stack<T>(initial_items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet(out T obj)
        {
            if (!this.objects.TryPop(out T inner))
            {
                obj = null;
                return false;
            }
            else
            {
                obj = inner;
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item) => this.objects.Push(item);
    }

    public class DoubleArrayObjectPool
    {
        ObjectPool<double[]> pool;
        int size;
        public DoubleArrayObjectPool(int size, int init_count)
        {
            this.size = size;
            this.pool = new ObjectPool<double[]>();

            for (int i = 0; i < init_count; i++)
                this.pool.Add(new double[size]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Get(out double[] obj)
        {
            if (!this.pool.TryGet(out double[] inner))
                obj = new double[this.size];
            else
                obj = inner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(double[] item) => this.pool.Add(item);
    }

    public static class PointerOperators
    {
        const uint MAX_MINUS_FOUR = 0b_1111_1111_1111_1111_1111_1111_1111_1100;
        const uint MAX_MINUS_EIGHT = 0b_1111_1111_1111_1111_1111_1111_1111_1000;
        const uint MAX_MINUS_SIXTEEN = 0b_1111_1111_1111_1111_1111_1111_1111_0000;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ApplyAvx(double* v, uint n, double value)
        {
            for (int i = 0; i < (n - 3); i += 4)
            {
                var x = Avx.BroadcastScalarToVector256(&value);
                Avx.Store(v + i, x);
            }

            // gets the closest power of 4 below n
            var nn = ((n >> 2) << 2);

            // clean up the residual
            for (uint i = nn; i < n; i++)
                v[i] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double DotAvx(double* x, double* y, uint n)
        {
            var vresult = Vector256<double>.Zero;

            for (int i = 0; i < (n - 2); i += 4)
            {
                var xx = Avx.LoadVector256(x + i);
                var yy = Avx.LoadVector256(y + i);
                var rr = Avx.Multiply(xx, yy);
                vresult = Avx.Add(rr, vresult);
            }

            vresult = Avx.HorizontalAdd(vresult, vresult);
            vresult = Avx.HorizontalAdd(vresult, vresult);

            var r = vresult.GetElement(0);
            // gets the closest power of 4 below n
            var nn = n & MAX_MINUS_FOUR; // ((n >> 2) << 2);

            // clean up the residual
            for (uint i = nn; i < n; i++)
                r += x[i] * y[i];

            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double DotAvxUnrolled(double* x, double* y, uint n)
        {
            var vresult = Vector256<double>.Zero;

            for (int i = 0; i < (n - 14); i += 16)
            {
                var xx = Avx.LoadVector256(x + i);
                var yy = Avx.LoadVector256(y + i);
                var rr = Avx.Multiply(xx, yy);
                vresult = Avx.Add(rr, vresult);

                xx = Avx.LoadVector256(x + i + 4);
                yy = Avx.LoadVector256(y + i + 4);
                rr = Avx.Multiply(xx, yy);
                vresult = Avx.Add(rr, vresult);

                xx = Avx.LoadVector256(x + i + 8);
                yy = Avx.LoadVector256(y + i + 8);
                rr = Avx.Multiply(xx, yy);
                vresult = Avx.Add(rr, vresult);

                xx = Avx.LoadVector256(x + i + 12);
                yy = Avx.LoadVector256(y + i + 12);
                rr = Avx.Multiply(xx, yy);
                vresult = Avx.Add(rr, vresult);
            }

            for (uint i = n & MAX_MINUS_SIXTEEN; i < (n - 2); i += 4)
            {
                var xx = Avx.LoadVector256(x + i);
                var yy = Avx.LoadVector256(y + i);
                var rr = Avx.Multiply(xx, yy);
                vresult = Avx.Add(rr, vresult);
            }

            vresult = Avx.HorizontalAdd(vresult, vresult);
            vresult = Avx.HorizontalAdd(vresult, vresult);

            var r = vresult.GetElement(0);

            // gets the closest power of 4 below n
            var nn = n & MAX_MINUS_FOUR; // ((n >> 2) << 2);

            // clean up the residual
            for (uint i = nn; i < n; i++)
                r += x[i] * y[i];

            return r;
        }
    }
}
