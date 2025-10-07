using System.Collections.Generic;
using AdmLodPrototype.Models;

namespace AdmLodPrototype.Interfaces
{
    public interface IProductGroupRepository
    {
        IEnumerable<ProductGroup> GetProductGroups();
        IEnumerable<ProductGroupAssignment> GetProductGroupAssignments();
    }
}