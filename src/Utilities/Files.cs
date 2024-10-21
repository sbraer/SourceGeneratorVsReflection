using SourceGeneratorVsReflection.Models;

namespace SourceGeneratorVsReflection.Utilities;

public static class Files
{
    public static string[] ReadCsvLine(string row)
    {
        List<string> result = [];
        var inQuotes = false;
        var currentValue = string.Empty;

        for (var counter = 0; counter < row.Length; counter++)
        {
            char c = row[counter];
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(currentValue);
                currentValue = string.Empty;
            }
            else
            {
                currentValue += c;
            }
        }

        result.Add(currentValue);
        return result.ToArray();
    }

    public static string GetCsvPath(string subDir)
    {
        var path = new Uri(Environment.CurrentDirectory);
        string newPath = path.Segments[1];
        for (var counter = 2; counter < path.Segments.Length; counter++)
        {
            var pathSegment = path.Segments[counter];
            newPath = Path.Combine(newPath, pathSegment);
            if (pathSegment.Equals("SourceGeneratorVsReflection" + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            {
                newPath = Path.Combine(newPath, subDir);
                break;
            }
        }

        if (!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);
        }

        return newPath;
    }

    public static void CreateCsvFile(List<RandomPropertiesClass> randomData, string csvFilePath)
    {
        using StreamWriter writer = new(csvFilePath);
        // Header
        writer.WriteLine(string.Join(",", typeof(RandomPropertiesClass).GetProperties().Select(p => p.Name)));

        // Write the data
        foreach (var item in randomData)
        {
            var values = typeof(RandomPropertiesClass).GetProperties()
                .Select(p =>
                {
                    var value = p.GetValue(item)?.ToString() ?? "";
                    // manage comma and double quote
                    return value.Contains(',') ? $"\"{value.Replace("\"", "\"\"")}\"" : value;
                });
            writer.WriteLine(string.Join(",", values));
        }
    }
}
