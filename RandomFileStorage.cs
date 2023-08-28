public static class RandomFileStorage
{
    public static readonly string BaseDir = 
        Path.Combine(Path.GetTempPath(), "parallel-file-hash-benchmark");

    public static string GetRandomFile(string id, int n) =>
        Path.Combine(BaseDir, id, $"{n}.dat");

    public static string? CreateRandomFiles(string id, int fileSize, int fileCount)
    {
        var dirName = Path.GetDirectoryName(GetRandomFile(id, 0));
        if (!string.IsNullOrEmpty(dirName))
            Directory.CreateDirectory(dirName);

        for (int i = 0; i < fileCount; i++)
        {
            CreateRandomFile(id, i, fileSize);
        }

        return dirName;
    }

    public static void CreateRandomFile(string id, int n, int fileSize)
    {
        using var fs = File.Create(GetRandomFile(id, n));

        var buffer = new byte[1024*256];
        var written = 0;
        while (written < fileSize)
        {
            Random.Shared.NextBytes(buffer);
            fs.Write(buffer, 0, Math.Min(buffer.Length, fileSize - written));
            written += buffer.Length;
        }
    }
}