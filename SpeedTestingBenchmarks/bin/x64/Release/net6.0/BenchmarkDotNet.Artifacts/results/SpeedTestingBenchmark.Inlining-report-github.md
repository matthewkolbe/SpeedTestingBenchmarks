``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|        Method |      Mean |     Error |    StdDev |
|-------------- |----------:|----------:|----------:|
|   InlineShort | 0.0000 ns | 0.0000 ns | 0.0000 ns |
| NoInlineShort | 0.4343 ns | 0.0013 ns | 0.0012 ns |
|    InlineLong | 0.2319 ns | 0.0017 ns | 0.0016 ns |
|  NoInlineLong | 0.4147 ns | 0.0016 ns | 0.0014 ns |
