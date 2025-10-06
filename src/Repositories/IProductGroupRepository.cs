using AdmLodPrototype.Models;

namespace AdmLodPrototype.Services.Interfaces
{
    public interface IProductGroupRepository
    {
        IEnumerable<ProductGroup> GetProductGroups();
        IEnumerable<ProductGroupAssignment> GetProductGroupAssignments();
    }
}