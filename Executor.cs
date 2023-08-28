
using Standart.Hash.xxHash;

public class Executor
{
    public string BaseDir { get; set; } = 
        Path.Combine(Path.GetTempPath(), "parallel-file-hash-benchmark");

    public int FileCount { get; set; }
    public int FileSize { get; set; }
    public int Parallelism { get; set; }
    public string HashMode { get; set; } = "XXH";

    private string getFilePath(int n) =>
        Path.Combine(BaseDir, $"{n}.dat");

    public void Setup()
    {
        Directory.CreateDirectory(BaseDir);
        for (int i = 0; i < FileCount; i++)
        {
            createRandomFile(getFilePath(i));
        }
    }

    private void createRandomFile(string path)
    {
        using var fs = File.Create(path);

        var buffer = new byte[1024*256];
        var written = 0;
        while (written < FileSize)
        {
            Random.Shared.NextBytes(buffer);
            fs.Write(buffer, 0, Math.Min(buffer.Length, FileSize - written));
            written += buffer.Length;
        }
    }

    public string? Benchmark()
    {
        string? dummy = null;
        Parallel.For(0, FileCount, new ParallelOptions
        {
            MaxDegreeOfParallelism = Parallelism
        }, i =>
        {
            using var fs = File.OpenRead(getFilePath(i));
            if (HashMode == "XXH")
                dummy = computeXXH(fs);
            else
                dummy = computeMD5(fs);
        });
        return dummy;
    }

    private string computeMD5(Stream stream)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var result = md5.ComputeHash(stream);
        return BitConverter.ToString(result);
    }

    private string computeXXH(Stream stream)
    {
        var result = xxHash64.ComputeHash(stream);
        return result.ToString();
    }

    public void Cleanup()
    {
        for (int i = 0; i < FileCount; i++)
        {
            try
            {
                File.Delete(getFilePath(i));
            }
            catch
            {

            }
        }
        Directory.Delete(BaseDir, true);
    }
}