``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT  [AttachedDebugger]
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|             Method |  a |      Mean |     Error |    StdDev |
|------------------- |--- |----------:|----------:|----------:|
|         Interfaced | 30 | 0.4328 ns | 0.0094 ns | 0.0088 ns |
|             Direct | 30 | 0.0088 ns | 0.0020 ns | 0.0019 ns |
| NoInlineInterfaced | 30 | 0.4298 ns | 0.0049 ns | 0.0043 ns |
|     NoInlineDirect | 30 | 0.8524 ns | 0.0098 ns | 0.0091 ns |
