``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|      Method |        Mean |    Error |   StdDev |
|------------ |------------:|---------:|---------:|
|  BuiltInExp |  6,943.7 ns | 17.72 ns | 15.70 ns |
|      AvxExp |  2,326.9 ns | 19.28 ns | 18.03 ns |
|  MathNetErf | 32,594.3 ns | 94.13 ns | 88.05 ns |
|      AvxErf |  4,254.2 ns |  6.82 ns |  6.38 ns |
|     AvxSqrt |    947.2 ns |  1.08 ns |  1.01 ns |
| BuiltInSqrt |  8,569.7 ns |  6.61 ns |  5.86 ns |
