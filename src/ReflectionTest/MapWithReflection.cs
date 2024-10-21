using SourceGeneratorVsReflection.Models;
using SourceGeneratorVsReflection.Utilities;
using System.Reflection;

namespace SourceGeneratorVsReflection.ReflectionTest;

public static class MapWithReflection
{
    public static List<RandomPropertiesClass> Import(string[] allRowsFile)
    {
        List<RandomPropertiesClass> loadedData = [];

        try
        {
            // read header
            string[] headers = allRowsFile[0].Split(',');

            for (var counter = 1; counter < allRowsFile.Length; counter++)
            {
                // read the data
                var reader = allRowsFile[counter];
                string[] values = Files.ReadCsvLine(reader);
                if (values.Length != headers.Length) continue;

                RandomPropertiesClass item = new RandomPropertiesClass();
                for (int i = 0; i < headers.Length; i++)
                {
                    PropertyInfo prop = typeof(RandomPropertiesClass).GetProperty(headers[i])!;
                    if (prop != null && values[i] != "")
                    {
                        object value = Convert.ChangeType(values[i], prop.PropertyType);
                        prop.SetValue(item, value);
                    }
                }
                loadedData.Add(item);
            }

            return loadedData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Generic error: {ex.Message}");
            return [];
        }
    }
}
