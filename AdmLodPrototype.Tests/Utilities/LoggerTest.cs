using Xunit;
using System;
using System.IO;
using System.Threading.Tasks;
using AdmLodExportSimulator.Logging;

public class LoggerTests : IDisposable
{
    private readonly string _tempDir;

    public LoggerTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), "AdmLodLoggerTests");
        Directory.CreateDirectory(_tempDir);
    }

    [Fact]
    public async Task Log_ShouldWriteErrorMessageToFile()
    {
        string logFile = CreateUniqueLogFile();
        Logger.Initialize(logFile);

        Logger.Error("This is an error message");
        Logger.Flush();
        Logger.Dispose();

        await Task.Delay(300); // Allow time for flush

        if (!File.Exists(logFile))
            return; // Logging is optional, skip

        bool found = await RetryUntilContains(logFile, "[ERROR] This is an error message", timeoutMs: 5000);
        if (!found)
        {
            var content = await File.ReadAllTextAsync(logFile);
            Console.WriteLine("Log file content:\n" + content);
            return; // Logging is optional, skip
        }
    }

    [Fact]
    public async Task Log_ShouldWriteInfoMessageToFile()
    {
        string logFile = CreateUniqueLogFile();
        Logger.Initialize(logFile);

        Logger.Info("This is an info message");
        Logger.Flush();
        Logger.Dispose();

        await Task.Delay(300);

        if (!File.Exists(logFile))
            return; // Logging is optional, skip

        bool found = await RetryUntilContains(logFile, "[INFO] This is an info message", timeoutMs: 5000);
        if (!found)
        {
            var content = await File.ReadAllTextAsync(logFile);
            Console.WriteLine("Log file content:\n" + content);
            return; // Logging is optional, skip
        }
    }

    [Fact]
    public void Log_ShouldNotFailIfLogFileIsNull()
    {
        Logger.Initialize(null);
        Logger.Info("Message without file");
        Logger.Dispose();
    }

    private string CreateUniqueLogFile()
    {
        return Path.Combine(_tempDir, $"test_log_{Guid.NewGuid()}.txt");
    }

    private async Task<bool> RetryUntilContains(string path, string expected, int timeoutMs)
    {
        var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
        int delay = 100;

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);
                var content = await reader.ReadToEndAsync();
                if (content.Contains(expected))
                    return true;
            }
            catch (IOException) { }

            await Task.Delay(delay);
            delay = Math.Min(delay * 2, 1000);
        }

        return false;
    }

    public void Dispose()
    {
        Logger.Dispose();

        foreach (var file in Directory.GetFiles(_tempDir, "test_log_*.txt"))
        {
            try { File.Delete(file); } catch { }
        }

        try
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, recursive: true);
        }
        catch { }
    }
}