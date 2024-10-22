using SourceGeneratorVsReflection.Utilities;

namespace SourceGeneratorVsReflection;

public static class Configuration
{
    private static string? csvPath;

    public static string GetCsvFileNameWithPath
    {
        get => Path.Combine(GetCsvPath, "example.csv");
    }

    public static string GetCsvPath
    {
        get
        {
            if (string.IsNullOrEmpty(csvPath))
            {
                csvPath = Files.GetCsvPath("CsvFile");
            }
            
            return csvPath;
        }
    }
}
