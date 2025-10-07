using Xunit;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class ProgramTest
{
    [Theory]
    [InlineData("export", "--success")]
    [InlineData("export", "--error")]
    [InlineData("export", "--success", "--logfile")]
    public void Main_WithValidArgs_ShouldGenerateExpectedFiles(params string[] baseArgs)
    {
        Directory.CreateDirectory("MockFiles");

        // Generate unique file names
        string exportPath = $"MockFiles/adm_export_{Guid.NewGuid()}.txt";
        string responseSuccessPath = $"MockFiles/adm_response_success_{Guid.NewGuid()}.txt";
        string responseErrorPath = $"MockFiles/adm_response_error_{Guid.NewGuid()}.txt";
        string logPath = $"MockFiles/test_log_{Guid.NewGuid()}.txt";

        // Build full args array
        var args = new List<string>(baseArgs)
        {
            $"--exportPath={exportPath}",
            $"--responseSuccessPath={responseSuccessPath}",
            $"--responseErrorPath={responseErrorPath}"
        };

        if (baseArgs.Contains("--logfile"))
        {
            args.Add($"--logfile={logPath}");
        }

        // Clean up before test
        if (File.Exists(exportPath)) File.Delete(exportPath);
        if (File.Exists(responseSuccessPath)) File.Delete(responseSuccessPath);
        if (File.Exists(responseErrorPath)) File.Delete(responseErrorPath);
        if (File.Exists(logPath)) File.Delete(logPath);

        // Act
        Program.Main(args.ToArray());

        // Assert
        Assert.True(File.Exists(exportPath));

        string expectedResponsePath = baseArgs.Contains("--error") ? responseErrorPath : responseSuccessPath;
        Assert.True(File.Exists(expectedResponsePath));

        var exportContent = File.ReadAllText(exportPath);
        var responseContent = File.ReadAllText(expectedResponsePath);

        Assert.Contains("VERSION;2.4;", exportContent);
        Assert.Contains("EXPORT;PGRP;;Y;", exportContent);
        Assert.Contains("MERGE;PRODUCT;", exportContent);

        if (baseArgs.Contains("--error"))
            Assert.Contains("Error", responseContent);
        else
            Assert.Contains("Success", responseContent);

        if (baseArgs.Contains("--logfile"))
        {
            Assert.True(File.Exists(logPath));
            var logContent = File.ReadAllText(logPath);
            Assert.Contains("Starting ADMLOD Export Prototype", logContent);
        }
    }
}