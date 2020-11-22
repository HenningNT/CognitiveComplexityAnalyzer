using HenningNT.CCAnalyzer.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Xunit;

namespace HenningNT.CCAnalyzer.Tests
{
    public class AnalyzeIfTests
    {
        private readonly CompilationUnitSyntax root;

        public AnalyzeIfTests()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(HelloWorldProgram);
            root = tree.GetCompilationUnitRoot();
        }

        [Fact]
        public void SimpleMethodScores0()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleMethod");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(0, score);
        }

        [Fact]
        public void SimpleIfScores1()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleIf");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(1, score);
        }

        [Fact]
        public void SimpleIf_and_Scores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleIfAnd");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(2, score);
        }

        [Fact]
        public void SimpleIf_or_Scores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleIfOr");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(2, score);
        }

        [Fact]
        public void SimpleIf_AndOr_Scores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleIfAndOr");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(3, score);
        }

        [Fact]
        public void If_RepeatAnd_Scores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "IfRepeatAnd");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(2, score);
        }

        [Fact]
        public void If_RepeatManyAnd_Scores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "IfRepeatManyAnd");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(2, score);
        }

        [Fact]
        public void If_RepeatManyOr_Scores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "IfRepeatManyOr");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(2, score);
        }

        [Fact]
        public void If_MixAndOr_Scores3()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "IfMixAndOr");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(3, score);
        }

        [Fact]
        public void NestedIf_Scores2()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "IfMixAndOr");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(3, score);
        }

        [Fact]
        public void NestedIfAndOr_Scores5()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "NestedIfAndOr");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(5, score);
        }

        [Fact]
        public void IfElseWithIf_Scores3()
        {
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "IfElseWithIf");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(3, score);
        }

        public const string HelloWorldProgram = @"
            using System; 
            using System.Collections.Generic;
            using System.Linq;

            namespace HelloWorld
            {
                class Program
                {
                    bool a,b,c,d,e,f;
                    
                    public void SimpleMethod()  // Scores 0
                    {
                        Console.WriteLine(a);
                    }

                    public void SimpleIf()      // Scores 1
                    {
                        if (a)
                            Console.WriteLine(a);
                    }

                    public void SimpleIfAnd()
                    {
                        if (a && c)
                            Console.WriteLine(a);
                    }
                    public void SimpleIfOr()
                    {
                        if (a && c)
                            Console.WriteLine(a);
                    }
                    public void SimpleIfAndOr()     //Scores 3
                    {
                        if (a && c || b)
                            Console.WriteLine(a);
                    }
                    public void IfRepeatAnd()     //Scores 2
                    {
                        if (a && c && b)
                            Console.WriteLine(a);
                    }
                    public void IfRepeatManyAnd()     //Scores 2
                    {
                        if (a && c && b && b && b && b)
                            Console.WriteLine(a);
                    }
                    public void IfRepeatManyOr()     //Scores 2
                    {
                        if (a || c || b || b || b || b)
                            Console.WriteLine(a);
                    }

                    public void IfMixAndOr()     //Scores 2
                    {
                        if (a && b && c || d || e && f)
                            Console.WriteLine(a);
                    }
                    public void NestedIf()      // Scores 3
                    {
                        if (a)          // +1
                            if (b)      // +2
                                Console.WriteLine(a);
                    }
                    public void NestedIfAndOr()     // Scores 5
                    {
                        if (a && c)     // +1 +1
                            if (b || d) // +2 +1
                                Console.WriteLine(a);
                    }
                    public void IfElseWithIf() // Scores 3
                    {
                        if (a)      // +1
                            Console.WriteLine(a);
                        else if (b) // +1 +1
                            Console.WriteLine(b);
                    }
                }
            }";
    }
}
