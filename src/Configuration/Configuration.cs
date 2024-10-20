namespace SourceGeneratorVsReflection;

public static class Configuration
{
    public static string GetCsvFileNameWithPath
    {
        get => Path.Combine(GetCsvPath, "example.csv");
    }
    public static string GetCsvPath
    {
        get
        {
            var path = new Uri(Environment.CurrentDirectory);
            string newPath = path.Segments[1];
            for (var counter = 2; counter < path.Segments.Length; counter++)
            {
                var pathSegment = path.Segments[counter];
                newPath = Path.Combine(newPath, pathSegment);
                if (pathSegment.Equals("SourceGeneratorVsReflection" + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                {
                    newPath = Path.Combine(newPath, "CsvFile");
                    break;
                }
            }

            if (Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            return newPath;
        }
    }
}
