using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
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
            var method = root.DescendantNodesAndSelf().OfType<MethodDeclarationSyntax>().First(method => method.Identifier.ValueText == "SimpleIf");

            var score = CognitiveComplexityAnalyzer.AnalyzeMethod(method);

            Assert.Equal(1, score);
        }
        class Program
        {
            bool a, b, c;
            public void SimpleIf()
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
                if (a || b)
                    Console.WriteLine(a);
            }
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
                    bool a,b,c;
                    
                    public void SimpleIf()
                    {
                        if (a)
                            Console.WriteLine(a);
                    }
                }
            }";
    }
}
