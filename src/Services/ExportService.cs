using AdmLodExportSimulator.Logging;
public class ExportService
{
    private readonly IProductRepository _productRepo;
    private readonly IProductGroupRepository _groupRepo;
    private readonly IResponseGenerator _responseGen;

    public ExportService(IProductRepository productRepo, IProductGroupRepository groupRepo, IResponseGenerator responseGen)
    {
        _productRepo = productRepo;
        _groupRepo = groupRepo;
        _responseGen = responseGen;
    }

    public void RunExport(bool simulateSuccess)
    {
        Logger.Info("Generating ADMLOD export file...");

        var products = _productRepo.GetProducts();
        var groups = _groupRepo.GetProductGroups();
        var assignments = _groupRepo.GetProductGroupAssignments();

        var exportContent = AdmLodFileGenerator.Generate(products, groups, assignments);
        var uploadSuccess = FtpClientMock.UploadExportFile(exportContent, !simulateSuccess);

        if (!uploadSuccess)
        {
            Logger.Error("Simulated FTP upload failed.");
            return;
        }

        Logger.Success("Simulated FTP upload completed.");

        var response = _responseGen.GenerateResponse(exportContent);
        var result = ResponseParser.ParseResponse(response);

        Logger.Info($"✅ Total Successes: {result.SuccessCount}");
        Logger.Warn($"❌ Total Errors: {result.ErrorCount}");
    }
}