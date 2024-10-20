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

            for (var counter = 1; counter < allRowsFile.Length; counter++)
            {
                // read the data
                var reader = allRowsFile[counter];
                string[] values = ReadCsvLine(reader);
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

#if (DEBUG)
            Console.WriteLine($"Elements in csv file: {loadedData.Count}");

            foreach (var item in loadedData.Take(3))
            {
                Console.WriteLine($"Name: {item.Name}, Age: {item.Age}, Profession: {item.Profession}");
            }
#endif

            return loadedData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Generic error: {ex.Message}");
            return [];
        }
    }

    private static string[] ReadCsvLine(string row)
    {
        List<string> result = [];
        bool inQuotes = false;
        string currentValue = "";

        for(var counter = 0; counter < row.Length; counter++)
        {
            char c = row[counter];
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(currentValue);
                currentValue = "";
            }            
            else
            {
                currentValue += c;
            }
        }

        result.Add(currentValue);
        return result.ToArray();
    }
}
