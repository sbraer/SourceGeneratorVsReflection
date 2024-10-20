using BenchmarkDotNet.Attributes;
using SourceGeneratorVsReflection.Models;
using SourceGeneratorVsReflection.ReflectionTest;
using SourceGeneratorVsReflection.SourceGeneratorTest;

namespace CreateCsvFileConsole;

//[ShortRunJob]
[MemoryDiagnoser]
[RankColumn]
public class BenchmarkTest
{
    [Benchmark]
    public List<RandomPropertiesClass> GetReflection() => MapWithReflection.Import();

    [Benchmark]
    public List<RandomPropertiesClass> GetSourceGenerator() => MapWithSourceGenerator.Import();
}
