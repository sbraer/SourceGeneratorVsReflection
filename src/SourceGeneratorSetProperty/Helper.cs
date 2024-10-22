using Microsoft.CodeAnalysis;

namespace GeneratorFromAttribute;

public static class Helper
{
    public static void ReportClassNotSupportedDiagnostic(SourceProductionContext context, string className)
    {
        var descriptor = new DiagnosticDescriptor(
            id: "FSG001",
            title: "Class not supported",
            messageFormat: "Class '{0}' must be static and partial",
            category: "RegisterSourceOutput",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None, className));
    }
}