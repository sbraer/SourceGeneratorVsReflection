using BenchmarkDotNet.Attributes;
using SourceGeneratorVsReflection;
using SourceGeneratorVsReflection.Models;
using SourceGeneratorVsReflection.ReflectionTest;
using SourceGeneratorVsReflection.SourceGeneratorTest;

namespace CreateCsvFileConsole;

//[ShortRunJob]
[MemoryDiagnoser]
[RankColumn]
public class BenchmarkTest
{
    private readonly string[] allRowsFile;
    public BenchmarkTest()
    {
        allRowsFile = File.ReadAllLines(Configuration.GetCsvFileNameWithPath);
    }
    [Benchmark]
    public List<RandomPropertiesClass> GetReflection() => MapWithReflection.Import(allRowsFile);

    [Benchmark]
    public List<RandomPropertiesClass> GetSourceGenerator() => MapWithSourceGenerator.Import(allRowsFile);
}
