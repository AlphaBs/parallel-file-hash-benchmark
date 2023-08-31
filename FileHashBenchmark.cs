using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

[SimpleJob(RunStrategy.Monitoring, iterationCount: 5)]
public class FileHashBenchmark
{
    public static string? Dummy;

    public string Id = "default";
    public int FileSize = 1024 * 64;
    public int FileCount = 1024 * 8;

    [Params(1, 4, 8, 16)]
    public int Parallelism { get; set; } = 1;

    [IterationSetup]
    public void IterationSetup()
    {
        RandomFileStorage.CreateRandomFiles(Id, FileSize, FileCount);
    }

    [Benchmark]
    public void BenchmarkXXH()
    {
        Dummy = Executor.Execute(Id, FileCount, Parallelism, "XXH");
    }

    [Benchmark(Baseline = true)]
    public void BenchmarkMD5()
    {
        Dummy = Executor.Execute(Id, FileCount, Parallelism, "MD5");
    }

    [IterationCleanup]
    public void Cleanup()
    {
        Executor.Cleanup(Id, FileCount);
    }
}