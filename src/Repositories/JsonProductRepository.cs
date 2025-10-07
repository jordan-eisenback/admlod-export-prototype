using System.Text.Json;

public class JsonProductRepository : IProductRepository
{
    private readonly string _filePath;

    public JsonProductRepository(string relativePath)
    {
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var fallbackPath = Path.GetFullPath(Path.Combine(basePath, "..")); // one level up from test project

        var primary = Path.Combine(basePath, relativePath);
        var fallback = Path.Combine(fallbackPath, relativePath);

        _filePath = File.Exists(primary) ? primary :
                    File.Exists(fallback) ? fallback :
                    throw new FileNotFoundException($"Could not find products.json at either '{primary}' or '{fallback}'");
}


    public IEnumerable<Product> GetProducts()
    {
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
    }
}