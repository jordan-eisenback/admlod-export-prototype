using System.Text;

public static class AdmLodFileGenerator
{
    public static string Generate(IEnumerable<Product> products, IEnumerable<string> groups, Dictionary<string, List<string>> assignments)
    {
        var sb = new StringBuilder();
        sb.AppendLine("VERSION;2.4;");
        sb.AppendLine("EXPORT;PGRP;;Y;");

        foreach (var group in groups)
        {
            sb.AppendLine(string.IsNullOrWhiteSpace(group)
                ? "# ERROR: Missing Product Group Name"
                : $"MERGE;PGRP;{group};");
        }

        foreach (var product in products)
        {
            if (string.IsNullOrWhiteSpace(product.Code) ||
                string.IsNullOrWhiteSpace(product.Name) ||
                string.IsNullOrWhiteSpace(product.Family))
            {
                sb.AppendLine($"# ERROR: Invalid product entry -> Code: '{product.Code}', Name: '{product.Name}', Family: '{product.Family}'");
            }
            else
            {
                sb.AppendLine($"MERGE;PRODUCT;{product.Code};{product.Name};{product.Family};");
            }
        }

        foreach (var group in assignments.Keys)
        {
            foreach (var code in assignments[group])
            {
                sb.AppendLine(string.IsNullOrWhiteSpace(group) || string.IsNullOrWhiteSpace(code)
                    ? $"# ERROR: Invalid assignment -> Group: '{group}', Code: '{code}'"
                    : $"MERGE;PGRPPROD;{group};{code};");
            }
        }

        return sb.ToString();
    }
}