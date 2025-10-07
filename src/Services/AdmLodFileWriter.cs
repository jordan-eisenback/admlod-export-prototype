//AdmLodFileWriter.cs
using System.IO;

public static class AdmLodFileWriter
{
    public static void WriteExportFile(string content, string filePath = "MockFiles/adm_export.txt")
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("ERROR: File path is null or empty.");
            return;
        }

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(filePath, content);
    }
}