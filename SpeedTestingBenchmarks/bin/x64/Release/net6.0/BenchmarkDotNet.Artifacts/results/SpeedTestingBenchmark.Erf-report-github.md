``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|      Method |        Mean |     Error |    StdDev |
|------------ |------------:|----------:|----------:|
|  BuiltInExp |  6,534.7 ns |  11.34 ns |   8.85 ns |
|      AvxExp |  2,327.7 ns |   2.25 ns |   2.10 ns |
|  MathNetErf | 32,513.1 ns | 243.56 ns | 227.83 ns |
|      AvxErf |  4,672.7 ns |  38.56 ns |  36.07 ns |
|     AvxSqrt |    949.4 ns |   3.59 ns |   2.99 ns |
| BuiltInSqrt |  8,572.0 ns |  10.22 ns |   8.54 ns |
