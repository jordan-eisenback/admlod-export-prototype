using Xunit;
using AdmLodPrototype.Models;
using System.Collections.Generic;

public class AdmLodFileGeneratorTests
{
    [Fact]
    public void ShouldGenerateValidMergeLines()
    {
        var products = new List<Product>
        {
            new() { Code = "D01", Name = "Diesel #1", Family = "D" },
            new() { Code = "G01", Name = "Gasoline Regular", Family = "G" }
        };
        var groups = new List<string> { "DIESEL", "GASOLINE" };
        var assignments = new Dictionary<string, List<string>>
        {
            { "DIESEL", new List<string> { "D01" } },
            { "GASOLINE", new List<string> { "G01" } }
        };

        var result = AdmLodFileGenerator.Generate(products, groups, assignments);

        Assert.Contains("MERGE;PRODUCT;D01;Diesel #1;D;", result);
        Assert.Contains("MERGE;PGRP;DIESEL;", result);
        Assert.Contains("MERGE;PGRPPROD;GASOLINE;G01;", result);
    }

    [Fact]
    public void ShouldReportMissingProductFields()
    {
        var products = new List<Product>
        {
            new() { Code = "", Name = "Gasoline Premium", Family = "G" }
        };

        var result = AdmLodFileGenerator.Generate(products, new List<string>(), new Dictionary<string, List<string>>());

        Assert.Contains("ERROR: Invalid product entry", result);
    }
}