``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT  [AttachedDebugger]
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|           Method |   N |      Mean |     Error |    StdDev |
|----------------- |---- |----------:|----------:|----------:|
|     BaselineUInt | 100 | 0.1673 ns | 0.0019 ns | 0.0018 ns |
| TestAndOpModUInt | 100 | 0.2085 ns | 0.0032 ns | 0.0027 ns |
|      BaselineInt | 100 | 0.2436 ns | 0.0059 ns | 0.0052 ns |
|  TestAndOpModInt | 100 | 0.1718 ns | 0.0022 ns | 0.0018 ns |
