using AdmLodExportSimulator.Logging;
using AdmLodPrototype.Interfaces;
using AdmLodPrototype.Models;
using System.IO;
using System.Linq;

namespace AdmLodPrototype.Services
{
    public class ExportService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IResponseGenerator _responseGenerator;
        private readonly IFtpUploader _ftpUploader;

        public ExportService(
            IProductRepository productRepository,
            IProductGroupRepository productGroupRepository,
            IResponseGenerator responseGenerator,
            IFtpUploader ftpUploader)
        {
            _productRepository = productRepository;
            _productGroupRepository = productGroupRepository;
            _responseGenerator = responseGenerator;
            _ftpUploader = ftpUploader;
        }

        public void RunExport(
            bool simulateSuccess,
            string exportFilePath,
            string responseSuccessPath,
            string responseErrorPath)
        {
            Logger.Info("Generating ADMLOD export file...");

            var products = _productRepository.GetProducts();
            var productGroups = _productGroupRepository.GetProductGroups();
            var productGroupAssignments = _productGroupRepository.GetProductGroupAssignments();

            var groupNames = productGroups.Select(g => g.Name);
            var assignments = productGroupAssignments.ToDictionary(a => a.GroupName, a => a.ProductCodes);

            var exportContent = AdmLodFileGenerator.Generate(products, groupNames, assignments);
            AdmLodFileWriter.WriteExportFile(exportContent, exportFilePath);
            _ftpUploader.Upload(exportFilePath, "adm_export.txt");

            var uploadSuccess = simulateSuccess;

            // Handle simulated FTP failure
            if (!uploadSuccess)
            {
                Logger.Error("Simulated FTP upload failed.");

                var errorResponse = _responseGenerator.GenerateResponse(exportContent);
                if (!string.IsNullOrWhiteSpace(responseErrorPath))
                {
                    File.WriteAllText(responseErrorPath, errorResponse);
                }

                return;
            }

            Logger.Success("Simulated FTP upload completed.");

            var response = _responseGenerator.GenerateResponse(exportContent);

            if (!string.IsNullOrWhiteSpace(response))
            {
                var result = ResponseParser.ParseResponse(response);
                Logger.Info($"Total Successes: {result.SuccessCount}");
                Logger.Warn($"Total Errors: {result.ErrorCount}");
            }
            else
            {
                Logger.Warn("No response generated to parse.");
            }

            if (simulateSuccess && !string.IsNullOrWhiteSpace(responseSuccessPath))
            {
                File.WriteAllText(responseSuccessPath, response);
            }
        }
    }
}