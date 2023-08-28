using System.Diagnostics;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

Summary startBenchmark()
{
    return BenchmarkRunner.Run<FileHashBenchmark>();
}

void prepareFiles()
{
    Console.WriteLine("Preparing files...");
    foreach (var id in new int[] { 1, 4, 8, 16 })
    {
        var dir = RandomFileStorage.CreateRandomFiles(
            id.ToString(),
            FileHashBenchmarkFromPreparedFiles.FileSize,
            FileHashBenchmarkFromPreparedFiles.FileCount);
        Console.WriteLine($"{FileHashBenchmarkFromPreparedFiles.FileCount} files were created in {dir}");
    }
}

Summary startFromPreparedFiles()
{
    return BenchmarkRunner.Run<FileHashBenchmarkFromPreparedFiles>();
}

void startOneIteration()
{
    var sw = new Stopwatch();
    var benchmark = new FileHashBenchmark();
    benchmark.IterationSetup();
    sw.Start();
    benchmark.BenchmarkXXH();
    sw.Stop();
    benchmark.Cleanup();
    Console.WriteLine(sw.Elapsed);
}

// A) Simple benchmark
startBenchmark();

// B) Clearing disk cache
// B-0) Make sure `startBenchmark()` is commented
// B-1) Uncomment `prepareFiles()` and run the program
//prepareFiles();
// B-2) Comment `prepareFiles()` again
// B-3) Clear disk cache: restart entire system or use RamMap tool
// B-3) Uncomment `startFromPreparedFiles()` code and run the benchmark
//startFromPreparedFiles();