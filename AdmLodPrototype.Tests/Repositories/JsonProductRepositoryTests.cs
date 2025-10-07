using Xunit;
using AdmLodPrototype.Models;
using AdmLodPrototype.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class JsonProductRepositoryTests
{
    private readonly string _mockFilePath;

    public JsonProductRepositoryTests()
    {
        _mockFilePath = Path.Combine(Path.GetTempPath(), $"products_{Guid.NewGuid()}.json");

        var mockProducts = new List<Product>
        {
            new() { Code = "D01", Name = "Diesel #1", Family = "D" },
            new() { Code = "G01", Name = "Gasoline Regular", Family = "G" }
        };

        File.WriteAllText(_mockFilePath, JsonSerializer.Serialize(mockProducts));
    }

    [Fact]
    public void GetProducts_ShouldReturnParsedProducts()
    {
        var repo = new JsonProductRepository(_mockFilePath);
        var products = repo.GetProducts();

        Assert.NotNull(products);
        Assert.Equal(2, products.Count());
        Assert.Contains(products, p => p.Code == "D01" && p.Name == "Diesel #1");
    }
}