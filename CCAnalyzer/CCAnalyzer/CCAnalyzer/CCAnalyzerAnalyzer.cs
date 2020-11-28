using HenningNT.CCAnalyzer.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace HenningNT.CCAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CCAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CCAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor CCRule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(CCRule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterCodeBlockAction(AnalyzCodeBlock);
        }

        private void AnalyzCodeBlock(CodeBlockAnalysisContext context)
        {
            if (context.CodeBlock is MethodDeclarationSyntax method)
            {
                try
                {
                    var cc = CognitiveComplexityAnalyzer.AnalyzeMethod(method);
                    Debug.WriteLine($"Method {method.Identifier.ValueText} has complexity of {cc}");
                    if (cc > 10)
                    {
                        var diagnostic = Diagnostic.Create(CCRule, method.GetLocation(), method.Identifier.ValueText, cc);

                        context.ReportDiagnostic(diagnostic);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception when processing {method.Identifier.ValueText}, Message is '{ex.Message}', stack trace: {ex.StackTrace}");
                }
            }
        }
    }
}
