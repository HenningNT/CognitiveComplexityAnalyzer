using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HenningNT.Analyzer.Tests
{
    public class AnalyzeForEachTests
    {
        private readonly CompilationUnitSyntax root;

        public AnalyzeForEachTests()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(snip);
            root = tree.GetCompilationUnitRoot();
        }

        [Fact]
        public void SimpleForEachLoop_Scores1()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleForEachLoop");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(1, score);
        }


        [Fact]
        public void ForEachLoopWithInvocations_Scores3()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "ForEachLoopWithCondition");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(3, score);
        }

        public string snip = @"
            namespace HelloWorld
            {
                class Program
                {
                    List<string> list = new List<string>();
                    public void SimpleForEachLoop()
                    {
                        foreach(var item in list)
                        {
                        }
                    }

                    public void ForEachLoopWithCondition()
                    {
                        foreach(var item in list.Where(s => s.Contains('o')))
                        {
                        }
                    }
                }
            }
";
    }
}
