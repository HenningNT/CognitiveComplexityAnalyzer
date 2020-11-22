using HenningNT.CCAnalyzer.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Xunit;

namespace HenningNT.CCAnalyzer.Tests
{
    public class AnalyseCatchTests
    {
        private readonly CompilationUnitSyntax root;

        public AnalyseCatchTests()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(Snip);
            root = tree.GetCompilationUnitRoot();
        }

        [Fact]
        public void SimpleCatch_Scores_1()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleCatch");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(1, score);
        }

        [Fact]
        public void SimpleCatchWithIf_Scores_2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleCatchWithIf");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(3, score);
        }

        public const string Snip = @"
            using System; 
            using System.Collections.Generic;
            using System.Linq;

            namespace HelloWorld
            {
                class Program
                {
                    public void SimpleCatch()
                    {
                        int a = 42;
                        try
                        {
                            Console.WriteLine(a);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    public void SimpleCatchWithIf()
                    {
                        int a = 42;
                        try
                        {
                            Console.WriteLine(a);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(a);

                            if (a == 24)
                                Console.WriteLine(a);
                        }
                    }
                }
            }";
    }
}
