using BenchmarkDotNet.Attributes;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTestingBenchmark
{
    public class Erf
    {
        double[] a, b;
        int N = 2048;

        [GlobalSetup]
        public void SetUp()
        {
            a = new double[N];
            b = new double[N];

            for (int i = 0; i < N; i++)
                a[i] = i * 0.01;
        }

        [Benchmark]
        public void BuiltInExp()
        {
            for (int i = 0; i < N; i++)
                a[i] = Math.Exp(b[i]);
        }

        [Benchmark]
        public unsafe void AvxExp()
        {
            fixed (double* aa = a) fixed (double* bb = b)
                for (int i = 0; i < N - 3; i += 4)
                    ExpDouble(aa + i, bb + i);
        }

        [Benchmark]
        public void MathNetErf()
        {
            for (int i = 0; i < N; i++)
                b[i] = SpecialFunctions.Erf(a[i]);
        }

        [Benchmark]
        public unsafe void AvxErf()
        {
            fixed (double* aa = a) fixed (double* bb = b)
            {
                for (int i = 0; i < N - 3; i += 4)
                    ErfValue(aa + i, bb + i);

                var n = (N >> 2) << 2;
                if (n != N)
                {
                    var tmpa = stackalloc double[4];
                    var tmpb = stackalloc double[4];
                    for (int i = n; i < N; ++i)
                        tmpa[i - n] = a[i - n];

                    ErfValue(tmpa, tmpb);

                    for (int i = n; i < N; ++i)
                        aa[i] = tmpb[i - n];
                }
            }
        }

        [Benchmark]
        public unsafe void AvxSqrt()
        {
            fixed (double* aa = a) fixed (double* bb = b)
                for (int i = 0; i < N - 3; i += 4)
                    Avx.Store(bb + i, Avx.Sqrt(Avx.LoadVector256(aa + i)));

        }

        [Benchmark]
        public void BuiltInSqrt()
        {
            for (int i = 0; i < N; i++)
                b[i] = Math.Sqrt(a[i]);
        }

        static readonly Vector256<double> one = Vector256.Create(1.0);
        static readonly Vector256<double> negone = Vector256.Create(-1.0);
        static readonly Vector256<double> a1 = Vector256.Create(0.254829592);
        static readonly Vector256<double> a2 = Vector256.Create(-0.284496736);
        static readonly Vector256<double> a3 = Vector256.Create(1.421413741);
        static readonly Vector256<double> a4 = Vector256.Create(-1.453152027);
        static readonly Vector256<double> a5 = Vector256.Create(1.061405429);
        static readonly Vector256<double> p = Vector256.Create(0.3275911);
        static readonly Vector256<double> sign_bit = Vector256.Create(-0.0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ErfValue(double* xx, double* yy)
        {       
            var x = Avx.LoadVector256(xx);
            var sign = Avx.And(sign_bit, x);
            sign = Avx.Or(sign, one);
            x = Avx.AndNot(sign_bit, x);

            // A&S formula 7.1.26
            var tu = Avx.Multiply(p, x);
            tu = Avx.Add(one, tu);
            var t = Avx.Divide(one, tu);


            //double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);
            var y = Avx.Multiply(a5, t);
            y = Avx.Add(y, a4);
            y = Avx.Multiply(y, t);
            y = Avx.Add(y, a3);
            y = Avx.Multiply(y, t);
            y = Avx.Add(y, a2);
            y = Avx.Multiply(y, t);
            y = Avx.Add(y, a1);
            y = Avx.Multiply(y, t);

            var exsq = Avx.Multiply(x, x);
            exsq = Avx.Multiply(exsq, negone);

            ExpDouble(ref exsq, out Vector256<double> expd);

            y = Avx.Multiply(y, expd);
            y = Avx.Subtract(one, y);
            y = Avx.Multiply(y, sign);

            Avx.Store(yy, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ExpDouble(double* xx, double* yy)
        {
            var x = Avx.LoadVector256(xx);
            ExpDouble(ref x, out Vector256<double> ret);

            Avx.Store(yy, ret);
        }

        static readonly Vector256<double> exp_hi = Vector256.Create(709.0);
        static readonly Vector256<double> exp_lo = Vector256.Create(-709.0);
        static readonly Vector256<double> cephes_LOG2EF = Vector256.Create(1.44269504088896341);
        static readonly Vector256<double> inv_LOG2EF = Vector256.Create(0.693147180559945);
        static readonly Vector256<double> p1 = Vector256.Create(1.3981999507E-3);
        static readonly Vector256<double> p2 = Vector256.Create(8.3334519073E-3);
        static readonly Vector256<double> p3 = Vector256.Create(4.1665795894E-2);
        static readonly Vector256<double> p4 = Vector256.Create(1.6666665459E-1);
        static readonly Vector256<double> p5 = Vector256.Create(5.0000001201E-1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ExpDouble(ref Vector256<double> x, out Vector256<double> y)
        {
            y = Vector256.Create(1.9875691500E-4);
            x = Avx.Min(x, exp_hi);
            x = Avx.Max(x, exp_lo);

            var fx = Avx.Multiply(x, cephes_LOG2EF);
            fx = Avx.RoundToNearestInteger(fx);
            var z = Avx.Multiply(fx, inv_LOG2EF);
            x = Avx.Subtract(x, z);
            z = Avx.Multiply(x, x);

            y = Avx.Multiply(y, x);
            y = Avx.Add(y, p1);
            y = Avx.Multiply(y, x);
            y = Avx.Add(y, p2);
            y = Avx.Multiply(y, x);
            y = Avx.Add(y, p3);
            y = Avx.Multiply(y, x);
            y = Avx.Add(y, p4);
            y = Avx.Multiply(y, x);
            y = Avx.Add(y, p5);
            y = Avx.Multiply(y, z);
            y = Avx.Add(y, x);
            y = Avx.Add(y, one);

            var fxint = Avx.ConvertToVector128Int32(fx);
            var fxlong = Avx2.ConvertToVector256Int64(fxint);
            var full_exponent = Vector256.Create(0x3ffL);
            fxlong = Avx2.Add(fxlong, full_exponent);
            fxlong = Avx2.ShiftLeftLogical(fxlong, 52);
            var pow2n = Vector256.AsDouble(fxlong);
            y = Avx.Multiply(y, pow2n);
        }
    }
}
