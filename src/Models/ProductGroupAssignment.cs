namespace AdmLodPrototype.Models;
public class ProductGroupAssignment
{
    public required string GroupName { get; set; }
    public required List<string> ProductCodes { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(GroupName))
            throw new ArgumentException("GroupName is required");

        if (ProductCodes == null || ProductCodes.Count == 0)
            throw new ArgumentException("ProductCodes must contain at least one item");
    }
}