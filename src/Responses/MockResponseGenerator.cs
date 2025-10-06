using System.Text;

public class MockResponseGenerator : IResponseGenerator
{
    public string GenerateResponse(string exportContent)
    {
        var lines = exportContent.Split(Environment.NewLine);
        var sb = new StringBuilder();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith("# ERROR"))
            {
                sb.AppendLine($"Error: {line.Replace("# ERROR:", "").Trim()}");
            }
            else if (line.StartsWith("MERGE"))
            {
                // Simulate success for valid MERGE commands
                sb.AppendLine($"Success: {line}");
            }
            else
            {
                // Treat unknown lines as warnings
                sb.AppendLine($"Warning: Unrecognized line format -> {line}");
            }
        }

        return sb.ToString();
    }
}