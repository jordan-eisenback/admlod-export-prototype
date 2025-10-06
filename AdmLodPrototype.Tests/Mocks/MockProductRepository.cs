using AdmLodPrototype.Models;
using AdmLodPrototype.Services.Interfaces;
using System.Collections.Generic;

public class MockProductRepository : IProductRepository
{
    public IEnumerable<Product> GetProducts()
    {
        return new List<Product>
        {
            new Product { Id = "P001", Name = "TestProduct1" },
            new Product { Id = "P002", Name = "TestProduct2" }
        };
    }
}