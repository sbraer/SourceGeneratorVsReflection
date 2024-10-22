using GeneratorFromAttributeExample;
using SourceGeneratorVsReflection.Models;

namespace SourceGeneratorVsReflection.SourceGeneratorTest;

[GenerateSetProperty<RandomPropertiesClass>()]
internal static partial class ClassHelper { }