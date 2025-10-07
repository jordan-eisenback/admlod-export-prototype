using Xunit;
using AdmLodPrototype.Models;

public class ProductTests
{
    [Fact]
    public void Product_ShouldStorePropertiesCorrectly()
    {
        var product = new Product
        {
            Code = "D01",
            Name = "Diesel #1",
            Family = "D"
        };

        Assert.Equal("D01", product.Code);
        Assert.Equal("Diesel #1", product.Name);
        Assert.Equal("D", product.Family);
    }

    [Fact]
    public void Product_ShouldAllowNullValues()
    {
        var product = new Product
        {
            Code = null,
            Name = null,
            Family = null
        };

        Assert.Null(product.Code);
        Assert.Null(product.Name);
        Assert.Null(product.Family);
    }
}