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
                var reader = allRowsFile[counter].AsSpan();

                RandomPropertiesClass item = new RandomPropertiesClass();
                var index = reader.IndexOf(',');
                for (int i = 0; i < headers.Length; i++)
                {
                    PropertyInfo prop = typeof(RandomPropertiesClass).GetProperty(headers[i])!;
                    if (prop != null)
                    {
                        object value;
                        if (i != headers.Length - 1)
                        {
                            value = Convert.ChangeType(reader.Slice(0, index).ToString(), prop.PropertyType);
                            prop.SetValue(item, value);
                            reader = reader.Slice(index + 1);
                            index = reader.IndexOf(',');
                        }
                        else
                        {
                            value = Convert.ChangeType(reader.Slice(0).ToString(), prop.PropertyType);                            
                            prop.SetValue(item, value);
                        }
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
