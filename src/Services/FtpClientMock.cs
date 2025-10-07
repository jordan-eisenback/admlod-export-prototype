//FtpClientMock.cs
using System;
using System.IO;
using System.Threading.Tasks;
using AdmLodPrototype.Interfaces;
using AdmLodExportSimulator.Logging;

namespace AdmLodPrototype.Services
{
    public class FtpClientMock : IFtpUploader
    {
        private readonly string _mockDirectory;

        public FtpClientMock(string? mockDirectory = null)
        {
            _mockDirectory = mockDirectory ?? "MockFiles";
        }


    public void Upload(string localFilePath, string remoteFilePath)
    {
        if (!File.Exists(localFilePath))
            throw new FileNotFoundException($"File not found: {localFilePath}");

        Directory.CreateDirectory(_mockDirectory);
        File.Copy(localFilePath, Path.Combine(_mockDirectory, Path.GetFileName(remoteFilePath)), true);
        Logger.Log($"[FTP] Uploaded {localFilePath} to {remoteFilePath}", "INFO");
    }


        public async Task UploadFileAsync(string localFilePath, string remoteFilePath)
        {
            Directory.CreateDirectory(_mockDirectory);
            await File.WriteAllTextAsync(
                Path.Combine(_mockDirectory, Path.GetFileName(remoteFilePath)),
                await File.ReadAllTextAsync(localFilePath));
            Logger.Log($"[FTP] Uploaded {localFilePath} to {remoteFilePath}", "INFO");
        }
    }
}
