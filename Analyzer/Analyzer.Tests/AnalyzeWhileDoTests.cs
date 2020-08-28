using HenningNT.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Xunit;

namespace HenningNT.Analyzer.Tests
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
        }

        [Fact]
        public void WhileWithConditionScores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "WhileWithCondition");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(2, score);
        }




        public string snip = @"
            namespace HelloWorld
            {
                class Program
                {
                    bool a = true;
                    bool b = true;

                    public void WhileWithCondition()
                    {
                        while (a == b)
                        {
                            a = !a;
                        }
                    }

                    public void SimpleWhileDo()
                    {
                        while (a)
                        {
                            a = !a;
                        }
                    }
                }
            }
        ";
    }

}
