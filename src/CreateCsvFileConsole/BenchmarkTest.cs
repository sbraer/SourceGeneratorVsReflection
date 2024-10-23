using BenchmarkDotNet.Attributes;
using SourceGeneratorVsReflection;
using SourceGeneratorVsReflection.Models;
using SourceGeneratorVsReflection.ReflectionSimpleTest;
using SourceGeneratorVsReflection.ReflectionSpan;
using SourceGeneratorVsReflection.SourceGeneratorTest;

namespace SourceGeneratorVsReflection;

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
    public List<RandomPropertiesClass> GetEasyReflection() => MapWithEasyReflection.Import(allRowsFile);

    [Benchmark]
    public List<RandomPropertiesClass> GetReflection() => MapWithReflection.Import(allRowsFile);

    [Benchmark]
    public List<RandomPropertiesClass> GetSourceGenerator() => MapWithSourceGenerator.Import(allRowsFile);
}
