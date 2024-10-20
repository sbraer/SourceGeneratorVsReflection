using SourceGeneratorVsReflection.Models;
using System.Reflection;

namespace SourceGeneratorVsReflection.ReflectionTest;

public static class MapWithReflection
{
    public static List<RandomPropertiesClass> Import()
    {
        string csvFilePath = Configuration.GetCsvFileNameWithPath;
        List<RandomPropertiesClass> loadedData = [];

        try
        {
            using (StreamReader reader = new(csvFilePath))
            {
                // read header
                string[] headers = reader.ReadLine()!.Split(',');

                // read the data
                while (!reader.EndOfStream)
                {
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

    private static string[] ReadCsvLine(StreamReader reader)
    {
        List<string> result = [];
        bool inQuotes = false;
        string currentValue = "";

        while (true)
        {
            int charInt = reader.Read();
            if (charInt == -1) break; // End of file

            char c = (char)charInt;
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(currentValue);
                currentValue = "";
            }
            else if (c == '\r' || c == '\n')
            {
                if (inQuotes)
                {
                    currentValue += c;
                }
                else
                {
                    if (currentValue != "" || result.Count > 0)
                    {
                        result.Add(currentValue);
                        if (c == '\r' && reader.Peek() == '\n') reader.Read(); // \r\n
                        break;
                    }
                }
            }
            else
            {
                currentValue += c;
            }
        }

        return result.ToArray();
    }
}
