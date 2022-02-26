``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT  [AttachedDebugger]
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|           Method |    x |       Mean |     Error |    StdDev |
|----------------- |----- |-----------:|----------:|----------:|
|   FindHashedFast | 1.15 |  3.0199 ns | 0.0074 ns | 0.0066 ns |
|       FindHashed | 1.15 |  7.9329 ns | 0.0076 ns | 0.0064 ns |
| FindBinarySearch | 1.15 | 25.7695 ns | 0.0248 ns | 0.0207 ns |
|     FastLogCache | 1.15 |  0.6425 ns | 0.0011 ns | 0.0010 ns |
|      RegualarLog | 1.15 |  4.4521 ns | 0.0273 ns | 0.0255 ns |
|   FastLogFormula | 1.15 |  2.3617 ns | 0.0114 ns | 0.0106 ns |
