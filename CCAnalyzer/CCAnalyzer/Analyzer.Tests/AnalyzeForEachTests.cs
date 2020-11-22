using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

            Assert.Equal(2, score);
        }

        [Fact]
        public void ForEachLoopWithIfInLoop_Scores3()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleForEachWithIfInLoop");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(3, score);
        }


        public string snip = @"
            namespace HelloWorld
            {
                class Program
                {
                    List<bool> list = new List<bool>();
                    public void SimpleForEachWithIfInLoop()
                    {
                        foreach (var item in list)
                        {
                            if (item)
                                Console.WriteLine(item);
                        }
                    }

                    public void SimpleForEachLoop()
                    {
                        foreach(var item in list)
                        {
                            Console.WriteLine(item);
                        }
                    }

                    public void ForEachLoopWithCondition()
                    {
                        foreach(var item in list.Where(s => s.Contains('o')))
                        {
                            Console.WriteLine(item);
                        }
                    }
                }
            }
";
    }
}
