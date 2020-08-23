using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Xunit;

namespace HenningNT.Analyzer.Tests
{
    public class CognitiveComplexityAnalyzerTests
    {
        private readonly CompilationUnitSyntax root;

        public CognitiveComplexityAnalyzerTests()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(CodeSnippets.HelloWorldProgram);
            root = tree.GetCompilationUnitRoot();
        }

        [Fact]
        public void MainMethodScores1()
        {
            var mainMethod = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "Main");
            var unit = new CognitiveComplexityAnalyzer();

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(mainMethod);

            Assert.Equal(1, score);
        }
    }

    public class CodeSnippets
    {
        public const string HelloWorldProgram = @"
            using System; 
            using System.Collections.Generic;
            using System.Linq;

            namespace HelloWorld
            {
                class Program
                {
                    static void Main(string[] args)
                    {
                        Console.WriteLine(""Hello World!"");
                    }
                }
            }";
    }
}
