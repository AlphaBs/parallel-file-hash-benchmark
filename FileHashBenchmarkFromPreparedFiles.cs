using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

[SimpleJob(RunStrategy.Monitoring, iterationCount: 1)]
public class FileHashBenchmarkFromPreparedFiles
{
    public static int FileSize { get; set; } = 1024 * 64;
    public static int FileCount { get; set; } = 1024 * 8;

    [Params(1, 4, 8, 16)]
    public int Parallelism { get; set; } = 1;

    [IterationSetup]
    public void IterationSetup()
    {
        for (int i = 0; i < FileCount; i++)
        {
            var file = RandomFileStorage.GetRandomFile(Parallelism.ToString(), i);
            if (!File.Exists(file))
                throw new InvalidOperationException($"{file} does not exist. ");
        }
    }

    [Benchmark]
    public void Benchmark()
    {
        Executor.Execute(Parallelism.ToString(), FileCount, Parallelism, "XXH");
    }

    [IterationCleanup]
    public void Cleanup()
    {
        Executor.Cleanup(Parallelism.ToString(), FileCount);
    }
}