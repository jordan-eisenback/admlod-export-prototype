using Xunit;
using Moq;
using System;
using System.IO;
using AdmLodPrototype.Services;
using AdmLodPrototype.Services.Interfaces;

public class FtpTests
{
    [Fact]
    public void RunExport_ShouldLogError_WhenFtpFails()
    {
        var ftpUploader = new Mock<IFtpUploader>();
        ftpUploader.Setup(u => u.Upload(It.IsAny<string>(), It.IsAny<string>()))
                   .Throws(new IOException("Simulated FTP failure"));

        var exportService = new ExportService(
            new Mock<IProductRepository>().Object,
            new Mock<IProductGroupRepository>().Object,
            new Mock<IResponseGenerator>().Object,
            ftpUploader.Object);

        var ex = Record.Exception(() => exportService.RunExport(simulateSuccess: false));
        Assert.NotNull(ex);
        Assert.IsType<IOException>(ex);
    }
}