using BenchmarkDotNet.Running;
using SourceGeneratorVsReflection;
using SourceGeneratorVsReflection.Models;
using SourceGeneratorVsReflection.ReflectionSimpleTest;
using SourceGeneratorVsReflection.ReflectionSpan;
using SourceGeneratorVsReflection.SourceGeneratorTest;
using SourceGeneratorVsReflection.Utilities;
using System.Diagnostics;
using System.Globalization;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

List<RandomPropertiesClass> randomData = FakeData.GetRandomPropertiesClassColection();
foreach (var item in randomData.Take(15))
{
    Console.WriteLine($"Name: {item.Name}, Age: {item.Age}, Profession: {item.Profession}, City: {item.City}");
}

string csvFilePath = Configuration.GetCsvFileNameWithPath;
Files.CreateCsvFile(randomData, csvFilePath);
Console.WriteLine($"File created: {Path.GetFullPath(csvFilePath)}");

#if(!DEBUG)
var benchmarkSummary1 = BenchmarkRunner.Run<BenchmarkTest>();
#else
Console.WriteLine(new string('=', 120));

string[] allRowsFile = File.ReadAllLines(Configuration.GetCsvFileNameWithPath);
Stopwatch sw0 = Stopwatch.StartNew();
var result0 = MapWithEasyReflection.Import(allRowsFile);
sw0.Stop();
ShowData(result0);
Console.WriteLine($"Reflection easy import = {sw0.ElapsedMilliseconds}ms");
Console.WriteLine();

Stopwatch sw1 = Stopwatch.StartNew();
var result1 = MapWithReflection.Import(allRowsFile);
sw1.Stop();
ShowData(result1);
Console.WriteLine($"Reflection import = {sw1.ElapsedMilliseconds}ms");
Console.WriteLine();

Stopwatch sw2 = Stopwatch.StartNew();
var result2 = MapWithSourceGenerator.Import(allRowsFile);
sw2.Stop();
ShowData(result2);
Console.WriteLine($"Source Generator import = {sw2.ElapsedMilliseconds}ms");

static void ShowData(List<RandomPropertiesClass> loadedData)
{
    Console.WriteLine($"Elements in csv file: {loadedData.Count}");
    foreach (var item in loadedData.Take(3))
    {
        Console.WriteLine($"Name: {item.Name}, Age: {item.Age}, Profession: {item.Profession}, City: {item.City}");
    }
}
#endif
