using HenningNT.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Xunit;

namespace Analyzer.Tests
{
    public class AnalyzeWhileDoTests
    {
        private readonly CompilationUnitSyntax root;

        public AnalyzeWhileDoTests()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(snip);
            root = tree.GetCompilationUnitRoot();
        }

        [Fact]
        public void SimpleWhileScores1()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleWhileDo");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(1, score);

            bool a = true;
            while (a)
            {
                a = !a;
            }
        }


        public string snip = @"
            namespace HelloWorld
            {
                class Program
                {
        public void SimpleWhileDo()
        {
            bool a = true;
            while (a)
            {
                a = !a;
            }
        }
}}
        ";
    }

}
