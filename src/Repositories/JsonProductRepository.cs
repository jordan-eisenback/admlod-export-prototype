using System.Text.Json;

public class JsonProductRepository : IProductRepository
{
    private readonly string _filePath;

    public JsonProductRepository(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<Product> GetProducts()
    {
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
    }
}