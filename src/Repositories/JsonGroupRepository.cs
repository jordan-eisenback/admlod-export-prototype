using System.Text.Json;
using AdmLodPrototype.Models;
using AdmLodPrototype.Interfaces;

namespace AdmLodPrototype.Repositories
{
    public class JsonGroupRepository : IProductGroupRepository
    {
        private readonly string _filePath;
        private GroupData? _cachedData;

        public JsonGroupRepository(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        private GroupData GetData()
        {
            if (_cachedData != null)
                return _cachedData;

            if (!File.Exists(_filePath))
                return new GroupData();

            var json = File.ReadAllText(_filePath);
            _cachedData = JsonSerializer.Deserialize<GroupData>(json) ?? new GroupData();
            return _cachedData;
        }

        public IEnumerable<ProductGroup> GetProductGroups()
        {
            var data = GetData();
            return data.Groups?.Select(g => new ProductGroup { Name = g }) 
                   ?? Enumerable.Empty<ProductGroup>();
        }

        public IEnumerable<ProductGroupAssignment> GetProductGroupAssignments()
        {
            var data = GetData();
            return data.Assignments?.Select(kvp => new ProductGroupAssignment
            {
                GroupName = kvp.Key,
                ProductCodes = kvp.Value
            }) ?? Enumerable.Empty<ProductGroupAssignment>();
        }

        private class GroupData
        {
            public List<string>? Groups { get; set; }
            public Dictionary<string, List<string>>? Assignments { get; set; }
        }
    }
}
