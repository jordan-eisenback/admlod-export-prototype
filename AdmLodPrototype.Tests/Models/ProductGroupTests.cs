using Xunit;
using AdmLodPrototype.Models;

public class ProductGroupTests
{
    [Fact]
    public void ProductGroup_ShouldStoreName()
    {
        var group = new ProductGroup
        {
            Name = "DIESEL"
        };

        Assert.Equal("DIESEL", group.Name);
    }

    [Fact]
    public void ProductGroup_ShouldThrowIfNameIsEmpty()
    {
        var group = new ProductGroup { Name = "" };
        Assert.Throws<ArgumentException>(() => group.Validate());
    }
}