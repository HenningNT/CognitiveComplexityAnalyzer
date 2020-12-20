using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Xunit;

namespace HenningNT.CCAnalyzer.Tests
{
    public class AnalyzeForTests
    {
        private readonly CompilationUnitSyntax root;

        public AnalyzeForTests()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(snip);
            root = tree.GetCompilationUnitRoot();
        }

        [Fact]
        public void SimpleForLoop_Scores1()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleForLoop");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(1, score);
        }

        [Fact]
        public void ForLoopWithMethodCallForMax_Scores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "ForLoopWithMethodCallForMax");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(2, score);
        }

        [Fact]
        public void ForLoopWithMethodCallIncrement_Scores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "ForLoopWithMethodCallForCount");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(2, score);
        }

        [Fact]
        public void ForLoopWithMethodCallDeclaration_Scores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "ForLoopWithMethodCallForDeclaration");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(2, score);
        }

        [Fact]
        public void SimpleForLoopWithIf_Scores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleForLoopWithIf_Scores2");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(4, score);
        }


        public string snip = @"
            namespace HelloWorld
            {
                class Program
                {
                    public void SimpleForLoopWithIf_Scores2()
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (i == 2)
                                Console.WriteLine(i);
                        }
                    }
                    public void ForLoopWithMethodCallForDeclaration()
                    {
                        for (int i = GetStart(); i == 10; i++)
                        {

                        }
                    }
                    public void ForLoopWithMethodCallForCount()
                    {
                        for (int i = 0; i == 10; GetCount())
                        {

                        }
                    }

                    public void ForLoopWithMethodCallForMax()
                    {
                        for (int i = 0; i == GetMax(); i++)
                        {

                        }
                    }

                    public void SimpleForLoop()
                    {
                        for (int i = 0; i < 10; i++)
                        {

                        }
                    }
                }
            }
";
    }
}
