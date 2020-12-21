using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Xunit;

namespace HenningNT.CCAnalyzer.Tests
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

        [Fact]
        public void WhileWithConditionAndStatementsScores4()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "WhileWithConditionAndStatements");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(5, score);
        }

        [Fact]
        public void DoWhileScores1()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleDoWhile");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(1, score);
        }

        [Fact]
        public void DoWhileWithConditionScores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleDoWhileWithCondition");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(2, score);
        }

        [Fact]
        public void DoWhileWithConditionAndStatementScores5()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "DoWhileWithConditionAndStatements");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(5, score);
        }


        public string snip = @"
            namespace HelloWorld
            {
                class Program
                {
                    bool a = true;
                    bool b = true;

                    public void DoWhileWithConditionAndStatements()
                    {
                        do
                        {
                            if (b == true)
                                a = !a;
                        }while (a == b);
                    }

                    public void SimpleDoWhileWithCondition()
                    {
                        do
                        {
                        } while (a == false);
                    }

                    public void SimpleDoWhile()
                    {
                        do
                        {
                        } while (a);
                    }

                    public void WhileWithConditionAndStatements()
                    {
                        while (a == b)
                        {
                            if (b == true)
                                a = !a;
                        }
                    }
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
