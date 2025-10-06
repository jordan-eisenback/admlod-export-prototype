using Xunit;
using Moq;
using AdmLodPrototype.Services;
using AdmLodPrototype.Services.Interfaces;

public class CliTests
{
    [Fact]
    public void RunExport_ShouldHandleSimulateSuccessFlagCorrectly()
    {
        var exportService = new ExportService(
            new Mock<IProductRepository>().Object,
            new Mock<IProductGroupRepository>().Object,
            new Mock<IResponseGenerator>().Object,
            new Mock<IFtpUploader>().Object);

        exportService.RunExport(simulateSuccess: true);
        exportService.RunExport(simulateSuccess: false);

        // Validate behavior based on flag
    }
}
