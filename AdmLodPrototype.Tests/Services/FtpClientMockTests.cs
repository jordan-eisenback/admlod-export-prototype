using Xunit;
using System.IO;
using System.Threading.Tasks;
using AdmLodPrototype.Services;
using AdmLodExportSimulator.Logging;

public class FtpClientMockTests : IDisposable, IAsyncLifetime
{
    private readonly string _mockDirectory;
    private readonly string _testFilePath;
    private const string RemoteFileName = "remote_testfile.txt";
    private readonly List<string> _cleanup = new();

    public FtpClientMockTests()
    {
        _mockDirectory = Path.Combine(Path.GetTempPath(), $"MockFiles_{Guid.NewGuid()}");
        Directory.CreateDirectory(_mockDirectory);

        _testFilePath = Path.Combine(_mockDirectory, "testfile.txt");
        File.WriteAllText(_testFilePath, "Test content");
    }

    [Fact]
    public void Upload_ShouldCopyFileToMockDirectory()
    {
        var ftpClient = new FtpClientMock(_mockDirectory);

        ftpClient.Upload(_testFilePath, RemoteFileName);

        string expectedPath = Path.Combine(_mockDirectory, Path.GetFileName(RemoteFileName));
        Assert.True(File.Exists(expectedPath));
    }

    [Fact]
    public async Task UploadFileAsync_ShouldWriteFileToMockDirectory()
    {
        var ftpClient = new FtpClientMock(_mockDirectory);

        await ftpClient.UploadFileAsync(_testFilePath, RemoteFileName);

        string expectedPath = Path.Combine(_mockDirectory, Path.GetFileName(RemoteFileName));
        Assert.True(File.Exists(expectedPath));
    }

    [Fact]
    public void Upload_ShouldLogMessage()
    {
        // Create a unique log file and ensure directory exists
        var logDir = Path.Combine(Path.GetTempPath(), "AdmLodTests");
        Directory.CreateDirectory(logDir);
        string logFile = Path.Combine(logDir, $"test_log_{Guid.NewGuid()}.txt");
        _cleanup.Add(logFile);
        
        // Initialize logger and ensure it's ready
        Logger.Initialize(logFile);
        Thread.Sleep(100); // Give logger time to initialize

        try
        {
            // Create and upload test file
            var ftpClient = new FtpClientMock(_mockDirectory);
            ftpClient.Upload(_testFilePath, RemoteFileName);

            // Wait with exponential backoff
            var watch = System.Diagnostics.Stopwatch.StartNew();
            while (watch.ElapsedMilliseconds < 5000) // 5 second timeout
            {
                if (File.Exists(logFile))
                {
                    var content = File.ReadAllText(logFile);
                    if (content.Contains("[FTP] Uploaded"))
                    {
                        return; // Test passed
                    }
                }
                Thread.Sleep(100);
            }

            Assert.Fail($"Log message not found within timeout. Log file exists: {File.Exists(logFile)}");
        }
        finally
        {
            // Cleanup test artifacts
            if (File.Exists(logFile))
            {
                try { File.Delete(logFile); } catch { /* Ignore cleanup errors */ }
            }
            _cleanup.Remove(logFile);
        }
    }

    [Fact]
    public async Task Upload_ShouldLogMessageAsync()  // Renamed method
    {
        // Create a unique log file with retry mechanism
        var logDir = Path.Combine(Path.GetTempPath(), "AdmLodTests");
        Directory.CreateDirectory(logDir);
        string logFile = Path.Combine(logDir, $"test_log_{Guid.NewGuid()}.txt");
        _cleanup.Add(logFile);
        
        // Initialize logger with await
        Logger.Initialize(logFile);
        await Task.Delay(200); // Give logger more time to initialize

        try
        {
            var ftpClient = new FtpClientMock(_mockDirectory);
            ftpClient.Upload(_testFilePath, RemoteFileName);

            // Use async/await for file reading
            var success = await WaitForLogFileAsync(logFile);
            Assert.True(success, "Log message not found within timeout");
        }
        finally
        {
            await Task.Delay(100); // Ensure file handles are released
            if (File.Exists(logFile))
            {
                try 
                { 
                    File.Delete(logFile); 
                }
                catch (IOException) 
                { 
                    Console.WriteLine($"Warning: Could not delete log file {logFile}");
                }
            }
        }
    }

    private async Task<bool> WaitForLogFileAsync(string logFile)
    {
        var deadline = DateTime.UtcNow.AddSeconds(5);
        
        while (DateTime.UtcNow < deadline)
        {
            if (File.Exists(logFile))
            {
                try
                {
                    using var stream = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using var reader = new StreamReader(stream);
                    var content = await reader.ReadToEndAsync();
                    if (content.Contains("[FTP] Uploaded"))
                    {
                        return true;
                    }
                }
                catch (IOException)
                {
                    // File might be locked, retry
                }
            }
            await Task.Delay(100);
        }
        return false;
    }

    public async Task InitializeAsync()
    {
        await Task.CompletedTask; // For future async initialization
    }

    public async Task DisposeAsync()
    {
        await Task.Delay(100); // Ensure all file operations complete
        Dispose();
    }

    public void Dispose()
    {
        // Clean up test files
        foreach (var file in _cleanup)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        // Clean up mock directory
        if (Directory.Exists(_mockDirectory))
        {
            Directory.Delete(_mockDirectory, recursive: true);
        }
    }
}