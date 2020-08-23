﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace HenningNT.Analyzer
{
    public class CognitiveComplexityAnalyzer
    {
        public static int AnalyzeMethod(MethodDeclarationSyntax methodSyntax )
        {
            return AnalyzeStatements(methodSyntax.Body.Statements, 0);
        }

        private static int AnalyzeStatements(SyntaxList<StatementSyntax> statements, int nesting)
        {
            int score = 0;

            foreach (var item in statements)
            {
                switch (item)
                {
                    case IfStatementSyntax ifStatement:
                        score += AnalyzeIfStatement(ifStatement, nesting);
                        break;
                }
            }
            return score ;
        }

        private static int AnalyzeIfStatement(IfStatementSyntax ifStatement, int nesting)
        {
            int score = 1;

            var nested = ifStatement.Statement.DescendantNodesAndSelf().OfType<StatementSyntax>();
            if (ifStatement.Else != null)
                nested = nested.Concat( ifStatement.Else.Statement.DescendantNodesAndSelf().OfType<StatementSyntax>());

            if (nested.Any())
                score += AnalyzeStatements(new SyntaxList<StatementSyntax>(nested ) , nesting + 1);

            var condition = ifStatement.Condition.DescendantNodesAndSelf(exp => exp.GetType() == typeof(BinaryExpressionSyntax)).Where(exp => exp.GetType() != typeof(IdentifierNameSyntax)).Select(node => node.Kind().ToString()).GroupBy(name => name);
            var tokens = ifStatement.Condition.DescendantNodesAndSelf(exp => exp.GetType() == typeof(SyntaxToken));
            score += condition.Count();
            
            return score+nesting;
        }
    }
}
