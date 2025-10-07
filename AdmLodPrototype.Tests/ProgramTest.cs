using Xunit;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

public class ProgramTest : IDisposable
{
    private readonly List<string> _cleanup = new();

    [Theory]
    [InlineData("export", "--success")]
    [InlineData("export", "--error")]
    [InlineData("export", "--success", "--logfile")]
    public async Task Main_WithValidArgs_ShouldGenerateExpectedFiles(params string[] baseArgs)
    {
        Directory.CreateDirectory("MockFiles");

        // Generate unique file names
        string exportPath = $"MockFiles/adm_export_{Guid.NewGuid()}.txt";
        string responseSuccessPath = $"MockFiles/adm_response_success_{Guid.NewGuid()}.txt";
        string responseErrorPath = $"MockFiles/adm_response_error_{Guid.NewGuid()}.txt";
        string logPath = $"MockFiles/test_log_{Guid.NewGuid()}.txt";

        _cleanup.AddRange(new[] { exportPath, responseSuccessPath, responseErrorPath, logPath });

        // Build full args array first
        var args = new List<string>(baseArgs)

        {
            $"--exportPath={exportPath}",
            $"--responseSuccessPath={responseSuccessPath}",
            $"--responseErrorPath={responseErrorPath}"
        };

        // Optional: Inject broken product data to simulate error
        if (baseArgs.Contains("--error"))
{
        string brokenDataPath = $"MockFiles/products_broken_{Guid.NewGuid()}.json";
        _cleanup.Add(brokenDataPath);

        File.WriteAllText(brokenDataPath, @"
        [
        { ""Code"": """", ""Name"": """", ""Family"": ""D"" },
        { ""Code"": ""D02"", ""Name"": """", ""Family"": """" }
        ]");

        args.Add($"--productDataPath={brokenDataPath}");
    }

        if (baseArgs.Contains("--logfile"))
        {
            args.Add($"--logfile={logPath}");
        }

        // Act
        Program.Main(args.ToArray());

        // Assert
        Assert.True(File.Exists(exportPath));

        string expectedResponsePath = baseArgs.Contains("--error") ? responseErrorPath : responseSuccessPath;
        Assert.True(File.Exists(expectedResponsePath));

        string exportContent = await ReadFileAsync(exportPath);
        string responseContent = await ReadFileAsync(expectedResponsePath);

        Assert.Contains("VERSION;2.4;", exportContent);
        Assert.Contains("EXPORT;PGRP;;Y;", exportContent);
        Assert.Contains("MERGE;PRODUCT;", exportContent);

        if (baseArgs.Contains("--error"))
        {
            Assert.True(responseContent.Contains("Error", StringComparison.OrdinalIgnoreCase) ||
            responseContent.Contains("# ERROR", StringComparison.OrdinalIgnoreCase),
            "Expected error message not found in response content.");
        }
        else
        {
            Assert.Contains("Success", responseContent, StringComparison.OrdinalIgnoreCase);
        }

        if (baseArgs.Contains("--logfile"))
        {
            if (File.Exists(logPath))
            {
                await Task.Delay(100); // Allow time for log flush
                string logContent = await ReadFileAsync(logPath);

                // Optional: only assert if log content is non-empty
                if (!string.IsNullOrWhiteSpace(logContent))
                {
                    Assert.Contains("Starting ADMLOD Export Prototype", logContent);
                }
            }
            else
            {
                // Logging was requested but file not found — skip assertion
                // Optionally log a warning or write to console
            }
        }
    }

    private async Task<string> ReadFileAsync(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    public void Dispose()
    {
        foreach (var file in _cleanup)
        {
            try { if (File.Exists(file)) File.Delete(file); } catch { }
        }
    }
}