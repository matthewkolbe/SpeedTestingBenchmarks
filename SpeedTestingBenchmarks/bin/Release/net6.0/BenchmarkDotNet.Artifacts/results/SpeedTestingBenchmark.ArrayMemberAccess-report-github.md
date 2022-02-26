``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT  [AttachedDebugger]
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|       Method |     Mean |     Error |    StdDev |
|------------- |---------:|----------:|----------:|
| StructAccess | 1.274 ns | 0.0076 ns | 0.0071 ns |
|  ClassAccess | 2.324 ns | 0.0163 ns | 0.0136 ns |
