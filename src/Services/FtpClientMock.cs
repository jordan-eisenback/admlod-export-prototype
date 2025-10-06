//FtpClientMock.cs
using System;
using System.IO;
using AdmLodExportSimulator.Logging;

public static class FtpClientMock
{
    private const string ExportFileName = "adm_export.txt";
    private const string MockFilesDirectory = "MockFiles";

    public static bool UploadExportFile(string content, bool simulateFailure)
    {
        string filePath = Path.Combine(MockFilesDirectory, ExportFileName);

        try
        {
            if (simulateFailure)
            {
                Logger.Warn("Simulating FTP failure...");
                throw new IOException("Simulated FTP upload failure.");
            }

            Directory.CreateDirectory(MockFilesDirectory);
            File.WriteAllText(filePath, content);
            Logger.Success($"Export file successfully written to {filePath}");
            return true;
        }
        catch (Exception ex)
        {
            Logger.Error($"FTP upload failed: {ex.Message}");
            return false;
        }
    }
}