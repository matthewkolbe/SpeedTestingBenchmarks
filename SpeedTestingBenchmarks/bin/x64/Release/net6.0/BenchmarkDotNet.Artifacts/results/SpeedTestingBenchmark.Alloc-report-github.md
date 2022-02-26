``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|               Method |         Mean |      Error |     StdDev |  Gen 0 |  Gen 1 | Allocated |
|--------------------- |-------------:|-----------:|-----------:|-------:|-------:|----------:|
|      SmallStackAlloc |     6.502 ns |  0.0331 ns |  0.0277 ns |      - |      - |         - |
|           SmallAlloc |    11.693 ns |  0.2366 ns |  0.2097 ns | 0.0220 |      - |     368 B |
| SmallObjectPoolAlloc |    15.294 ns |  0.0651 ns |  0.0609 ns |      - |      - |         - |
|        MidStackAlloc |   127.096 ns |  0.3453 ns |  0.3229 ns |      - |      - |         - |
|             MidAlloc |   147.084 ns |  1.5680 ns |  1.3900 ns | 0.4809 | 0.0072 |   8,048 B |
|   MidObjectPoolAlloc |    12.564 ns |  0.0907 ns |  0.0849 ns |      - |      - |         - |
|        BigStackAlloc | 4,215.842 ns |  9.9904 ns |  8.8563 ns |      - |      - |         - |
|             BigAlloc | 2,091.175 ns | 32.2896 ns | 30.2037 ns | 9.5215 |      - | 160,048 B |
|   BigObjectPoolAlloc |    11.400 ns |  0.0414 ns |  0.0367 ns |      - |      - |         - |
