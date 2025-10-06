using AdmLodExportSimulator.Logging;
public class ExportService
{
    private readonly IProductRepository _productRepo;
    private readonly IProductGroupRepository _groupRepo;
    private readonly IResponseGenerator _responseGen;

    private readonly IFtpUploader _ftpUploader;

    public ExportService(
        IProductRepository productRepo,
        IProductGroupRepository groupRepo,
        IResponseGenerator responseGenerator,
        IFtpUploader ftpUploader)
    {
        _productRepo = productRepo;
        _groupRepo = groupRepo;
        _responseGenerator = responseGenerator;
        _ftpUploader = ftpUploader;
    }

    public void RunExport(bool simulateSuccess)
    {
        Logger.Info("Generating ADMLOD export file...");

        var products = _productRepo.GetProducts();
        var groups = _groupRepo.GetProductGroups();
        var assignments = _groupRepo.GetProductGroupAssignments();

        var exportContent = AdmLodFileGenerator.Generate(products, groups, assignments);
        _ftpUploader.Upload(exportContent, "adm_export.txt");
        var uploadSuccess = true; // Assume success unless simulating failure
        if (!simulateSuccess) uploadSuccess = false; // Simulate failure if needed


        if (!uploadSuccess)
        {
            Logger.Error("Simulated FTP upload failed.");
            return;
        }

        Logger.Success("Simulated FTP upload completed.");

        var response = _responseGen.GenerateResponse(exportContent);
        var result = ResponseParser.ParseResponse(response);

        Logger.Info($"Total Successes: {result.SuccessCount}");
        Logger.Warn($"Total Errors: {result.ErrorCount}");
    }
}