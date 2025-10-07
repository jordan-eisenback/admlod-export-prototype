namespace AdmLodPrototype.Models
{
    public class ProductGroup
    {
        public required string Name { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Name is required");
        }
    }
}