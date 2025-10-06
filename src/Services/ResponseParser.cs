// ResponseParser.cs
using System;
using System.IO;

public class ParseResult
{
    public int SuccessCount { get; set; }
    public int ErrorCount { get; set; }
}

public static class ResponseParser
{
    // ✅ NEW: Parse from string content (used by MockResponseGenerator)
    public static ParseResult ParseResponse(string responseContent)
    {
        int successCount = 0;
        int errorCount = 0;

        var lines = responseContent.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            if (line.Contains("Error", StringComparison.OrdinalIgnoreCase))
                errorCount++;
            else if (line.Contains("Success", StringComparison.OrdinalIgnoreCase))
                successCount++;
        }

        return new ParseResult { SuccessCount = successCount, ErrorCount = errorCount };
    }

    // ✅ EXISTING: Parse from file (used for static response files)
    public static ParseResult ParseResponseFile(string fileName)
    {
        string filePath = Path.Combine("MockFiles", fileName);

        int successCount = 0;
        int errorCount = 0;

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Response file not found: {filePath}");
            return new ParseResult();
        }

        Console.WriteLine($"Parsing response file: {fileName}");
        Console.WriteLine("----------------------------------------");

        string[] lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            if (line.Contains("Error", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ ERROR: {line}");
                errorCount++;
            }
            else if (line.Contains("Success", StringComparison.OrdinalIgnoreCase) || line.StartsWith("MERGE"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ SUCCESS: {line}");
                successCount++;
            }
            else
            {
                Console.ResetColor();
                Console.WriteLine(line);
            }
        }

        Console.ResetColor();
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"✅ Total Successes: {successCount}");
        Console.WriteLine($"❌ Total Errors: {errorCount}");

        return new ParseResult { SuccessCount = successCount, ErrorCount = errorCount };
    }
}