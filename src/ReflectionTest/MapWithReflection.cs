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
                    if (prop is not null)
                    {
                        object value;
                        
                        // Se non è l'ultimo elemento, fai lo slicing fino all'indice della virgola
                        if (i != headers.Length - 1)
                        {
                            value = Convert.ChangeType(reader.Slice(0, index).ToString(), prop.PropertyType);
                            reader = reader.Slice(index + 1);  // Aggiorna il reader
                            index = reader.IndexOf(',');       // Trova il prossimo indice della virgola
                        }
                        else
                        {
                            // Se è l'ultimo elemento, prendi tutto il resto del reader
                            value = Convert.ChangeType(reader.Slice(0).ToString(), prop.PropertyType);
                        }
                        
                        // Imposta il valore sulla proprietà
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
