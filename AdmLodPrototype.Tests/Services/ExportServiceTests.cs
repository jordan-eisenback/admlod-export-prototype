using Xunit;
using Moq;
using AdmLodPrototype.Services;
using AdmLodPrototype.Interfaces;
using AdmLodPrototype.Models;
using System;
using System.Collections.Generic;
using AdmLodExportSimulator.Logging;

public class ExportServiceTests
{
    [Fact]
    public void RunExport_ShouldLogSuccess_WhenSimulationIsSuccessful()
    {
        // Arrange
        var mockProductRepo = new Mock<IProductRepository>();
        var mockGroupRepo = new Mock<IProductGroupRepository>();
        var mockResponseGen = new Mock<IResponseGenerator>();
        var mockFtpUploader = new Mock<IFtpUploader>();

        mockProductRepo.Setup(r => r.GetProducts()).Returns(new List<Product>
        {
            new() { Code = "D01", Name = "Diesel #1", Family = "D" }
        });

        mockGroupRepo.Setup(r => r.GetProductGroups()).Returns(new List<ProductGroup>
        {
            new() { Name = "DIESEL" }
        });

        mockGroupRepo.Setup(r => r.GetProductGroupAssignments()).Returns(new List<ProductGroupAssignment>
        {
            new() { GroupName = "DIESEL", ProductCodes = new List<string> { "D01" } }
        });

        mockResponseGen.Setup(r => r.GenerateResponse(It.IsAny<string>()))
            .Returns("Success: MERGE;PRODUCT;D01;Diesel #1;D");

        var service = new ExportService(
            mockProductRepo.Object,
            mockGroupRepo.Object,
            mockResponseGen.Object,
            mockFtpUploader.Object
        );

        // Act
        service.RunExport(
            simulateSuccess: true,
            exportFilePath: "MockFiles/test_export_success.txt",
            responseSuccessPath: "MockFiles/test_response_success.txt",
            responseErrorPath: "MockFiles/test_response_error.txt"
        );

        // Assert
        mockFtpUploader.Verify(u => u.Upload(It.IsAny<string>(), "adm_export.txt"), Times.Once);
        mockResponseGen.Verify(r => r.GenerateResponse(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void RunExport_ShouldLogError_WhenSimulationFails()
    {
        // Arrange
        var mockProductRepo = new Mock<IProductRepository>();
        var mockGroupRepo = new Mock<IProductGroupRepository>();
        var mockResponseGen = new Mock<IResponseGenerator>();
        var mockFtpUploader = new Mock<IFtpUploader>();

        mockProductRepo.Setup(r => r.GetProducts()).Returns(new List<Product>());
        mockGroupRepo.Setup(r => r.GetProductGroups()).Returns(new List<ProductGroup>());
        mockGroupRepo.Setup(r => r.GetProductGroupAssignments()).Returns(new List<ProductGroupAssignment>());
        mockResponseGen.Setup(r => r.GenerateResponse(It.IsAny<string>()))
            .Returns("Error: No products found");

        var service = new ExportService(
            mockProductRepo.Object,
            mockGroupRepo.Object,
            mockResponseGen.Object,
            mockFtpUploader.Object
        );

        var exportPath = $"MockFiles/test_export_{Guid.NewGuid()}.txt";
        var responseSuccessPath = $"MockFiles/test_response_success_{Guid.NewGuid()}.txt";
        var responseErrorPath = $"MockFiles/test_response_error_{Guid.NewGuid()}.txt";
        var logPath = $"MockFiles/test_log_{Guid.NewGuid()}.txt";

        Logger.Initialize(logPath); // ✅ Ensure logger is initialized

        // Act
        service.RunExport(
            simulateSuccess: false,
            exportFilePath: exportPath,
            responseSuccessPath: responseSuccessPath,
            responseErrorPath: responseErrorPath
        );

        Logger.Flush();
        Logger.Dispose();

        // Assert
        mockFtpUploader.Verify(u => u.Upload(It.IsAny<string>(), "adm_export.txt"), Times.Once);
        mockResponseGen.Verify(r => r.GenerateResponse(It.IsAny<string>()), Times.Once);
        Assert.True(File.Exists(responseErrorPath));
        var responseContent = File.ReadAllText(responseErrorPath);
        Assert.Contains("Error", responseContent);
    }
}