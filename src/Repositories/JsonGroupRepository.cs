using System.Text.Json;

public class JsonGroupRepository : IProductGroupRepository
{
    private readonly string _filePath;

    public JsonGroupRepository(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<string> GetProductGroups()
    {
        var json = File.ReadAllText(_filePath);
        var data = JsonSerializer.Deserialize<GroupData>(json);
        return data?.Groups ?? new List<string>();
    }

    public Dictionary<string, List<string>> GetProductGroupAssignments()
    {
        var json = File.ReadAllText(_filePath);
        var data = JsonSerializer.Deserialize<GroupData>(json);
        return data?.Assignments ?? new Dictionary<string, List<string>>();
    }

    private class GroupData
    {
        public List<string>? Groups { get; set; }
        public Dictionary<string, List<string>>? Assignments { get; set; }
    }
}