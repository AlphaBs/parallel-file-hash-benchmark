
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

[SimpleJob(RunStrategy.Monitoring, iterationCount: 10)]
public class FileHashBenchmark
{
    public static string? Dummy;

    Executor executor = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        executor = new Executor();
        executor.FileSize = 1024 * 64;
        executor.FileCount = 1024 * 8;
        executor.Setup();
    }

    [Benchmark(Baseline = true)]
    public void BenchmarkMD5()
    {
        executor.Parallelism = 1;
        executor.HashMode = "MD5";
        Dummy = executor.Benchmark();
    }

    [Benchmark]
    public void Benchmark1()
    {
        executor.Parallelism = 1;
        Dummy = executor.Benchmark();
    }

    //[Benchmark]
    public void Benchmark4()
    {
        executor.Parallelism = 4;
        Dummy = executor.Benchmark();
    }

    //[Benchmark]
    public void Benchmark8()
    {
        executor.Parallelism = 8;
        Dummy = executor.Benchmark();
    }

    //[Benchmark]
    public void Benchmark16()
    {
        executor.Parallelism = 16;
        Dummy = executor.Benchmark();
    }

    [IterationCleanup]
    public void Cleanup()
    {
        executor.Cleanup();
    }
}