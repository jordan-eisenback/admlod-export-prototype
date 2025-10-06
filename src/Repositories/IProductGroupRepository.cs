public interface IProductGroupRepository
{
    IEnumerable<string> GetProductGroups();
    Dictionary<string, List<string>> GetProductGroupAssignments();
}