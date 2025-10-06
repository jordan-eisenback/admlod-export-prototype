//FtpClientMock.cs
using System;
using System.IO;
using AdmLodExportSimulator.Logging;


using AdmLodPrototype.Services;
using AdmLodExportSimulator.Logging;

namespace AdmLodPrototype.Services
{
    public class FtpClientMock : IFtpUploader
    {
        public void Upload(string fileContent, string fileName)
        {
            File.WriteAllText($"MockFiles/{fileName}", fileContent);
            Logger.Log($"[FTP] Uploaded {fileName} to MockFiles/");
        }
    }
}