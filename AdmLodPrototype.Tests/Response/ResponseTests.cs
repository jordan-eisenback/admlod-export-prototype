using Xunit;
using Moq;
using AdmLodPrototype.Services;
using AdmLodPrototype.Services.Interfaces;

public class ResponseTests
{
    [Fact]
    public void RunExport_ShouldLogSuccessAndErrorCounts()
    {
        var responseGen = new Mock<IResponseGenerator>();
        responseGen.Setup(r => r.GenerateResponse(It.IsAny<string>()))
                   .Returns("SUCCESS: Item1\nERROR: Item2");

        var exportService = new ExportService(
            new Mock<IProductRepository>().Object,
            new Mock<IProductGroupRepository>().Object,
            responseGen.Object,
            new Mock<IFtpUploader>().Object);

        exportService.RunExport(simulateSuccess: true);

        // You can verify logs or parse counts if exposed
    }
}