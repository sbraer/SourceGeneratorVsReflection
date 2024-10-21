using SourceGeneratorVsReflection.Models;
using SourceGeneratorVsReflection.Utilities;

namespace SourceGeneratorVsReflection.SourceGeneratorTest;

public class MapWithSourceGenerator
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

                RandomPropertiesClass item = new();
                ClassHelper.SetPropertiesRandomPropertiesClass(item, values);
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
}
