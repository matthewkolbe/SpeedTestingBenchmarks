``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT  [AttachedDebugger]
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|               Method |         Mean |      Error |     StdDev |  Gen 0 |  Gen 1 | Allocated |
|--------------------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
|      SmallStackAlloc |     6.461 ns |  0.0194 ns |  0.0181 ns |      - |      - |         - |
|           SmallAlloc |    11.686 ns |  0.0843 ns |  0.0789 ns | 0.0220 |      - |     368 B |
| SmallObjectPoolAlloc |    11.977 ns |  0.0232 ns |  0.0193 ns |      - |      - |         - |
|        MidStackAlloc |   126.761 ns |  0.7280 ns |  0.6810 ns |      - |      - |         - |
|             MidAlloc |   152.378 ns |  2.8296 ns |  2.5084 ns | 0.4809 | 0.0072 |   8,048 B |
|   MidObjectPoolAlloc |    12.788 ns |  0.0427 ns |  0.0333 ns |      - |      - |         - |
|        BigStackAlloc | 4,181.058 ns |  8.5916 ns |  7.1744 ns |      - |      - |         - |
|             BigAlloc | 1,891.351 ns | 18.1355 ns | 16.9640 ns | 9.5215 |      - | 160,048 B |
|   BigObjectPoolAlloc |    11.340 ns |  0.0142 ns |  0.0126 ns |      - |      - |         - |
