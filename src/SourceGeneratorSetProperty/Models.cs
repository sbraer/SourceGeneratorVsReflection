using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace GeneratorFromAttribute;

public class MasterType(string className, bool isPartial, bool isStatic)
{
    public string ClassName { get; set; } = className;
    public bool IsPartial { get; set; } = isPartial;
    public bool IsStatic { get; set; } = isStatic;
    public List<SubTypeClass> SubTypes { get; set; } = [];
}

public class SubTypeClass(string className, string? nameSpace, List<SubProperty> properties)
{
    public string Classname { get; set; } = className;
    public string? Namespace { get; set; } = nameSpace;
    public List<SubProperty> Properties { get; set; } = properties;
}

public class SubProperty(string name, ITypeSymbol typeSimbol)
{
    public string Name { get; set; } = name;
    public ITypeSymbol Type { get; set; } = typeSimbol;
}