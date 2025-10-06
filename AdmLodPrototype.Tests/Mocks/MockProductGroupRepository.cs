using AdmLodPrototype.Models;
using AdmLodPrototype.Services.Interfaces;
using System.Collections.Generic;

public class MockProductGroupRepository : IProductGroupRepository
{
    public IEnumerable<ProductGroup> GetProductGroups()
    {
        return new List<ProductGroup>
        {
            new ProductGroup { Id = "G001", Name = "GroupA" },
            new ProductGroup { Id = "G002", Name = "GroupB" }
        };
    }

    public IEnumerable<ProductGroupAssignment> GetProductGroupAssignments()
    {
        return new List<ProductGroupAssignment>
        {
            new ProductGroupAssignment { ProductId = "P001", GroupId = "G001" },
            new ProductGroupAssignment { ProductId = "P002", GroupId = "G002" }
        };
    }
}