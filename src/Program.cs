using Microsoft.Extensions.Configuration;
using AdmLodExportSimulator.Logging;
using AdmLodExportSimulator;

class Program
{
    static void Main(string[] args)
    {
        // Load configuration
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Ensure correct path
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var settings = new AppSettings();
        config.Bind(settings);

        // Initialize logging
        Logger.Initialize(settings.Logging.EnableFileLogging ? settings.Paths.LogFile : null);

        // Setup repositories and services
        var productRepo = new JsonProductRepository("src/MockFiles/products.json");
        var groupRepo = new JsonGroupRepository("src/MockFiles/productgroups.json");
        var responseGen = new MockResponseGenerator();

        var exportService = new ExportService(productRepo, groupRepo, responseGen);

        // Parse CLI args
        bool simulateSuccess = args.Contains("--success");
        bool simulateError = args.Contains("--error");

        if (simulateError) settings.FeatureFlags.SimulateFtpFailure = true;
        if (settings?.Paths?.ExportFile == null)
{
        Console.WriteLine("Configuration is missing or invalid.");
        return;
}

        exportService.RunExport(simulateSuccess: !settings.FeatureFlags.SimulateFtpFailure);
    }
}
