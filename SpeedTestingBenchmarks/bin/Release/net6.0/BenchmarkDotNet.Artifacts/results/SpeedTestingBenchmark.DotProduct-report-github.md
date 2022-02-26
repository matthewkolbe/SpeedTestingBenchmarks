``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT  [AttachedDebugger]
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|      Method |      Mean |    Error |   StdDev | Allocated |
|------------ |----------:|---------:|---------:|----------:|
|         Avx |  73.13 ns | 0.120 ns | 0.112 ns |         - |
| AvxUnrolled |  67.76 ns | 0.184 ns | 0.172 ns |         - |
|      Accord | 300.51 ns | 1.811 ns | 1.694 ns |         - |
|     MathNet | 305.34 ns | 2.032 ns | 1.901 ns |         - |
