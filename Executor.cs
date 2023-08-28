using Standart.Hash.xxHash;

public static class Executor
{
    public static string? Execute(string id, int fileCount, int parallelism, string hashMode)
    {
        string? dummy = null;
        Parallel.For(0, fileCount, new ParallelOptions
        {
            MaxDegreeOfParallelism = parallelism
        }, i =>
        {
            using var fs = File.OpenRead(RandomFileStorage.GetRandomFile(id, i));
            if (hashMode == "XXH")
                dummy = computeXXH(fs);
            else
                dummy = computeMD5(fs);
        });
        return dummy;
    }

    private static string computeMD5(Stream stream)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var result = md5.ComputeHash(stream);
        return BitConverter.ToString(result);
    }

    private static string computeXXH(Stream stream)
    {
        var result = xxHash64.ComputeHash(stream);
        return result.ToString();
    }

    public static void Cleanup(string id, int fileCount)
    {
        for (int i = 0; i < fileCount; i++)
        {
            try
            {
                File.Delete(RandomFileStorage.GetRandomFile(id, i));
            }
            catch
            {

            }
        }
    }
}