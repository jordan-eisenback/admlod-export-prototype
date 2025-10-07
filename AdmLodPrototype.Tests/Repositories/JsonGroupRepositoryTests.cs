using Xunit;
using AdmLodPrototype.Models;
using AdmLodPrototype.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class JsonGroupRepositoryTests
{
    private readonly string _mockFilePath;

    public JsonGroupRepositoryTests()
    {
        _mockFilePath = Path.Combine(Path.GetTempPath(), $"groups_{Guid.NewGuid()}.json");

        var mockData = new
        {
            Groups = new List<string> { "DIESEL", "GASOLINE" },
            Assignments = new Dictionary<string, List<string>>
            {
                { "DIESEL", new List<string> { "D01" } },
                { "GASOLINE", new List<string> { "G01" } }
            }
        };

        File.WriteAllText(_mockFilePath, JsonSerializer.Serialize(mockData));

    }

    [Fact]
    public void GetProductGroups_ShouldReturnGroupNames()
    {
        var repo = new JsonGroupRepository(_mockFilePath);
        var groups = repo.GetProductGroups();

        Assert.NotNull(groups);
        Assert.Contains(groups, g => g.Name == "DIESEL");
    }

    [Fact]
    public void GetProductGroupAssignments_ShouldReturnAssignments()
    {
        var repo = new JsonGroupRepository(_mockFilePath);
        var assignments = repo.GetProductGroupAssignments();

        Assert.NotNull(assignments);
        Assert.Contains(assignments, a => a.GroupName == "GASOLINE" && a.ProductCodes.Contains("G01"));
    }
}