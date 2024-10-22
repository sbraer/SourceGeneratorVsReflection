using SourceGeneratorVsReflection.Models;
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
            List<PropertyInfo> propertiesInfo = [];
            for (int i = 0; i < headers.Length; i++)
            {
                propertiesInfo.Add(typeof(RandomPropertiesClass).GetProperty(headers[i])!);
            }

            for (var counter = 1; counter < allRowsFile.Length; counter++)
            {
                // read the data
                var reader = allRowsFile[counter].AsSpan();

                RandomPropertiesClass item = new();

                var index = reader.IndexOf(',');
                for (int i = 0; i < headers.Length; i++)
                {
                    PropertyInfo prop = propertiesInfo[i];
                    object value;
                    
                    if (i != headers.Length - 1)
                    {
                        value = Convert.ChangeType(reader.Slice(0, index).ToString(), prop.PropertyType);
                        reader = reader.Slice(index + 1);
                        index = reader.IndexOf(',');
                    }
                    else
                    {
                        value = Convert.ChangeType(reader.Slice(0).ToString(), prop.PropertyType);
                    }
                    
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
