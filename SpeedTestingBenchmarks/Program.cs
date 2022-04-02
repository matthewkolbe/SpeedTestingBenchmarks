using BenchmarkDotNet.Running;
using SpeedTestingBenchmark;

//var dp = new DotProduct();
//dp.Setup();
 
//var avx = dp.Avx();
//var unrolled = dp.AvxUnrolled();
//var accord = dp.Accord();
//var mathnet = dp.MathNet();

//var erl = new ExpRangeLookup();
//erl.Setup();

//var find_hashed_fast = erl.FindHashedFast(1.15);
//var find_hashed = erl.FindHashed(1.15);
//var find_binary_search = erl.FindBinarySearch(0.15);
//var fast_log_cache = erl.FastLogCache(0.05);
//var log = erl.RegualarLog(0.05);
//var fast_log_formula = erl.FastLogFormula(0.05);

//var mod8 = new Mod8Test();
//mod8.Setup();

//var baseline_uint = mod8.BaselineUInt();
//var and_op_uint = mod8.TestAndOpModUInt();
//var baseline_int = mod8.BaselineInt();
//var and_op_int = mod8.TestAndOpModInt();

//Console.WriteLine($"DotProduct {avx}, {unrolled}, {accord}, {mathnet}");
//Console.WriteLine($"ExpRangeLookup {find_hashed_fast}, {find_hashed}, {find_binary_search}, {fast_log_cache}, {log}, {fast_log_formula}");
//Console.WriteLine($"Mod8Test {baseline_uint}, {and_op_uint}, {baseline_int}, {and_op_int}");

//var summary = BenchmarkRunner.Run<ExpRangeLookup>();
//var summary = BenchmarkRunner.Run<DotProduct>();
//var summary = BenchmarkRunner.Run<InterfaceCost>();
//var summary = BenchmarkRunner.Run<Mod8Test>();
//var summary = BenchmarkRunner.Run<Alloc>();
var summary = BenchmarkRunner.Run<AllocNoZero>();
//var summary = BenchmarkRunner.Run<ArrayMemberAccess>();
//var summary = BenchmarkRunner.Run<Inlining>();
//var summary = BenchmarkRunner.Run<Erf>();

Console.ReadLine();