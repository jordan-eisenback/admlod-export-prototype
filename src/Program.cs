using Microsoft.Extensions.Configuration;
using AdmLodExportSimulator.Logging;
using AdmLodPrototype.Services;
using AdmLodPrototype.Repositories;
using AdmLodPrototype.Models;
using System;
using System.IO;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        // Load configuration
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var settings = new AppSettings();
        config.Bind(settings);



        // Parse CLI args
        bool simulateSuccess = args.Contains("--success");
        bool simulateError = args.Contains("--error");

        // Extract dynamic paths from args
        string exportPathArg = args.FirstOrDefault(a => a.StartsWith("--exportPath="))?.Split("=")[1];
        string responseSuccessPathArg = args.FirstOrDefault(a => a.StartsWith("--responseSuccessPath="))?.Split("=")[1];
        string responseErrorPathArg = args.FirstOrDefault(a => a.StartsWith("--responseErrorPath="))?.Split("=")[1];
        string logPathArg = args.FirstOrDefault(a => a.StartsWith("--logfile="))?.Split("=")[1];

        // Override settings with dynamic paths if provided
        if (!string.IsNullOrWhiteSpace(exportPathArg)) settings.Paths.ExportFile = exportPathArg;
        if (!string.IsNullOrWhiteSpace(responseSuccessPathArg)) settings.Paths.ResponseSuccessFile = responseSuccessPathArg;
        if (!string.IsNullOrWhiteSpace(responseErrorPathArg)) settings.Paths.ResponseErrorFile = responseErrorPathArg;
        if (!string.IsNullOrWhiteSpace(logPathArg)) settings.Paths.LogFile = logPathArg;

        if (!string.IsNullOrWhiteSpace(settings.Paths.LogFile))
        {
            var logDir = Path.GetDirectoryName(settings.Paths.LogFile);
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
        }

        // Initialize logging
        Logger.Initialize(settings.Logging.EnableFileLogging ? settings.Paths.LogFile : null);

        Logger.Info("Starting ADMLOD Export Prototype");

        // Setup repositories and services
        var productRepo = new JsonProductRepository("src/MockFiles/products.json");
        var groupRepo = new JsonGroupRepository("src/MockFiles/productgroups.json");
        var responseGen = new MockResponseGenerator();
        var ftpClient = new FtpClientMock();

        var exportService = new ExportService(
            productRepo,
            groupRepo,
            responseGen,
            ftpClient);

        if (simulateError)
            settings.FeatureFlags.SimulateFtpFailure = true;

        // Validate required paths
        if (string.IsNullOrWhiteSpace(settings?.Paths?.ExportFile) ||
            string.IsNullOrWhiteSpace(settings?.Paths?.ResponseSuccessFile) ||
            string.IsNullOrWhiteSpace(settings?.Paths?.ResponseErrorFile))
        {
            Console.WriteLine("Missing required export or response file paths.");
            return;
        }

        // Run export
        exportService.RunExport(
            simulateSuccess: !settings.FeatureFlags.SimulateFtpFailure,
            exportFilePath: settings.Paths.ExportFile,
            responseSuccessPath: settings.Paths.ResponseSuccessFile,
            responseErrorPath: settings.Paths.ResponseErrorFile
        );
    }
}