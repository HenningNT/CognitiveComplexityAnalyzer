using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Xunit;

namespace HenningNT.Analyzer.Tests
{
    public class AnalyseSwitchTests
    {
        private readonly CompilationUnitSyntax root;

        public AnalyseSwitchTests()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(Snip);
            root = tree.GetCompilationUnitRoot();
        }

        [Fact]
        public void SimpleSwitch_Scores_1()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleSwitch");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(1, score);
        }
        [Fact]
        public void SwitchWithIf_Scores_2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SwitchWithIf");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(2, score);
        }

        public const string Snip = @"
            using System; 
            using System.Collections.Generic;
            using System.Linq;

            namespace HelloWorld
            {
                class Program
                {
                    bool b;
                    public void SimpleSwitch()
                    {
                        int a = 2;
                        switch (a)
                        {
                            case 1:
                                Console.WriteLine(a);
                                break;
                            case 2:
                                Console.WriteLine(a);
                                break;
                            default:
                                break;
                        }
                    }
                    public void SwitchWithIf()
                    {
                        int a = 2;
                        switch (a)
                        {
                            case 1:
                                if (b)
                                    Console.WriteLine(a);
                                break;
                            case 2:
                                Console.WriteLine(a);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }";
    }
}
