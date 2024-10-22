using SourceGeneratorVsReflection.Models;
using System.Reflection;

namespace SourceGeneratorVsReflection.ReflectionSimpleTest;

public static class MapWithEasyReflection
{
    public static List<RandomPropertiesClass> Import(string[] allRowsFile)
    {
        List<RandomPropertiesClass> loadedData = [];

        try
        {
            // read header
            string[] headers = allRowsFile[0].Split(',');
            List<PropertyInfo> propertiesInfo = [];
            for (int i = 0; i < headers.Length; i++)
            {
                propertiesInfo.Add(typeof(RandomPropertiesClass).GetProperty(headers[i])!);
            }

            for (var counter = 1; counter < allRowsFile.Length; counter++)
            {
                // read the data
                var reader = allRowsFile[counter];

                RandomPropertiesClass item = new();

                var cols = reader.Split(',');
                for (int i = 0; i < headers.Length; i++)
                {
                    PropertyInfo prop = propertiesInfo[i];
                    object value = Convert.ChangeType(cols[i], prop.PropertyType);
                    prop.SetValue(item, value);
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
