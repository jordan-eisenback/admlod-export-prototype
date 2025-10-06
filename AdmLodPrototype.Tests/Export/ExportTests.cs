public class ExportTests
{
    [Fact]
    public void RunExport_ShouldGenerateExportFile_WhenSuccessScenario()
    {
        // Arrange
        var productRepo = new Mock<IProductRepository>();
        var groupRepo = new Mock<IProductGroupRepository>();
        var responseGen = new Mock<IResponseGenerator>();
        var ftpUploader = new Mock<IFtpUploader>();

        var exportService = new ExportService(productRepo.Object, groupRepo.Object, responseGen.Object, ftpUploader.Object);

        // Act
        exportService.RunExport(simulateSuccess: true);

        // Assert
        ftpUploader.Verify(u => u.Upload(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}