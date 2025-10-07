using Xunit;
using AdmLodPrototype.Models;
using System.Collections.Generic;

public class ProductGroupAssignmentTests
{
    [Fact]
    public void ProductGroupAssignment_ShouldThrowIfInvalid()
    {
        var assignment = new ProductGroupAssignment
        {
            GroupName = "",
            ProductCodes = new List<string>()
        };

        Assert.Throws<ArgumentException>(() => assignment.Validate());
    }
}
