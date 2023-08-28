
using System.Diagnostics;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<FileHashBenchmark>();
return;

var sw = new Stopwatch();
var benchmark = new FileHashBenchmark();
benchmark.IterationSetup();
sw.Start();
benchmark.Benchmark1();
sw.Stop();
benchmark.Cleanup();
Console.WriteLine(sw.Elapsed);