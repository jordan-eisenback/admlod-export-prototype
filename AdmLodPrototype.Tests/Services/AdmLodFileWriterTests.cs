using Xunit;
using System.IO;
using System.Text;

public class AdmLodFileWriterTests
{
    private readonly string _outputDir;
    private readonly string _outputFile;

    public AdmLodFileWriterTests()
    {
        _outputDir = Path.Combine(Path.GetTempPath(), $"AdmLodTest_{Guid.NewGuid()}");
        Directory.CreateDirectory(_outputDir);
        _outputFile = Path.Combine(_outputDir, "adm_export.txt");
    }

    [Fact]
    public void WriteExportFile_ShouldCreateFileWithExpectedContent()
    {
        // Arrange
        string expectedContent = new StringBuilder()
            .AppendLine("VERSION;2.4;")
            .AppendLine("EXPORT;PGRP;;Y;")
            .AppendLine("MERGE;PGRP;DIESEL;")
            .AppendLine("MERGE;PGRP;GASOLINE;")
            .AppendLine("MERGE;PRODUCT;D01;Diesel #1;D;")
            .AppendLine("MERGE;PRODUCT;G01;Gasoline Regular;G;")
            .ToString();

        // Act
        AdmLodFileWriter.WriteExportFile(expectedContent, _outputFile);

        // Assert
        Assert.True(File.Exists(_outputFile));
        string actualContent = File.ReadAllText(_outputFile);
        Assert.Equal(expectedContent, actualContent);
    }

    [Fact]
    public void WriteExportFile_ShouldThrowIfPathIsInvalid()
    {
        string invalidPath = Path.Combine(_outputDir, "invalid<>file.txt");
        string content = "EXPORT;PGRP;;Y;";

        var ex = Record.Exception(() => AdmLodFileWriter.WriteExportFile(content, invalidPath));
        Assert.NotNull(ex);
        Assert.IsType<IOException>(ex);
    }
}