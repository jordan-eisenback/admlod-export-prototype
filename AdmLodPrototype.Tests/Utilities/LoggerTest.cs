using Xunit;
using System;
using System.IO;
using AdmLodExportSimulator.Logging;

public class LoggerTests : IDisposable
{
    private const string LogFileName = "test_log.txt";

    public LoggerTests()
    {
        // Clean up before each test
        if (File.Exists(LogFileName))
            File.Delete(LogFileName);

        Logger.Initialize(LogFileName);
    }

    [Fact]
    public void Log_ShouldWriteInfoMessageToFile()
    {
        Logger.Info("This is an info message");

        string content = File.ReadAllText(LogFileName);
        Assert.Contains("[INFO] This is an info message", content);
    }

    private string GetUniqueLogFile() =>
        Path.Combine(Path.GetTempPath(), $"test_log_{Guid.NewGuid()}.txt");

    [Fact]
    public void Log_ShouldWriteErrorMessageToFile()
    {
        string logFile = GetUniqueLogFile();
        Logger.Initialize(logFile);

        Logger.Error("This is an error message");

        // Wait briefly to ensure the logger flushes and closes the file
        System.Threading.Thread.Sleep(100);

        string content = File.ReadAllText(logFile);
        Assert.Contains("[ERROR] This is an error message", content);
    }

    [Fact]
    public void Log_ShouldNotFailIfLogFileIsNull()
    {
        Logger.Initialize(null);

        // Should not throw
        Logger.Info("Message without file");
    }

    public void Dispose()
    {
        if (File.Exists(LogFileName))
            File.Delete(LogFileName);

        // Optionally delete all temp log files created during tests
        foreach (var file in Directory.GetFiles(Path.GetTempPath(), "test_log_*.txt"))
        {
            try { File.Delete(file); } catch { /* ignore if locked */ }
        }
    }
}