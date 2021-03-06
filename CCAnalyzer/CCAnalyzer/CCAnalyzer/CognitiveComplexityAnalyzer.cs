﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace HenningNT.CCAnalyzer
{
    public class CognitiveComplexityAnalyzer
    {
        public static int AnalyzeMethod(MethodDeclarationSyntax methodSyntax)
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
                    case SwitchStatementSyntax switchStatement:
                        score += AnalyzeSwitchStatement(switchStatement, nesting);
                        break;
                    case TryStatementSyntax trySyntax:
                        score += AnalyzeTrySyntax(trySyntax, nesting);
                        break;
                    case WhileStatementSyntax whileSyntax:
                        score += AnalyzeWhileSyntax(whileSyntax, nesting);
                        break;
                    case DoStatementSyntax whileSyntax:
                        score += AnalyzeDoWhileSyntax(whileSyntax, nesting);
                        break;
                    case ForStatementSyntax forSyntax:
                        score += AnalyzeForSyntax(forSyntax, nesting);
                        break;
                    case ForEachStatementSyntax forEachSyntax:
                        score += AnalyzeForEachSyntax(forEachSyntax, nesting);
                        break;
                }
            }
            return score;
        }

        private static int AnalyzeForEachSyntax(ForEachStatementSyntax forEachSyntax, int nesting)
        {
            int score = 1;
            score += forEachSyntax.Expression.DescendantNodes().Where(t => t is InvocationExpressionSyntax).Count();
            var a = forEachSyntax.Expression.DescendantNodes();
            var nested = forEachSyntax.DescendantNodes().OfType<StatementSyntax>();
            score += AnalyzeStatements(new SyntaxList<StatementSyntax>(nested), nesting + 1);

            return score;
        }

        private static int AnalyzeForSyntax(ForStatementSyntax forSyntax, int nesting)
        {
            int score = 1;

            score += forSyntax.Declaration.DescendantNodesAndSelf().Where(t => t is InvocationExpressionSyntax).Count();

            score += forSyntax.Incrementors.Where(t => t is InvocationExpressionSyntax).Count();

            if (forSyntax.Condition is BinaryExpressionSyntax bes)
            {
                var leftNested = bes.Left.DescendantNodesAndSelf().OfType<InvocationExpressionSyntax>();
                var rightNested = bes.Right.DescendantNodesAndSelf().OfType<InvocationExpressionSyntax>();
                score += leftNested.Count() + rightNested.Count();
            }

            var nested = forSyntax.DescendantNodes().OfType<StatementSyntax>();
            score += AnalyzeStatements(new SyntaxList<StatementSyntax>(nested), nesting + 1);

            return score;
        }

        private static int AnalyzeDoWhileSyntax(DoStatementSyntax whileSyntax, int nesting)
        {
            int score = 1;

            var condition = whileSyntax.Condition.DescendantNodesAndSelf(exp => exp.GetType() == typeof(BinaryExpressionSyntax)).Where(exp => exp.GetType() != typeof(IdentifierNameSyntax)).Where(exp => exp.GetType() != typeof(LiteralExpressionSyntax)).Select(node => node.Kind().ToString()).GroupBy(name => name);
            score += condition.Count();

            var nested = whileSyntax.DescendantNodes().OfType<StatementSyntax>();
            score += AnalyzeStatements(new SyntaxList<StatementSyntax>(nested), nesting + 1);

            return score;
        }

        private static int AnalyzeWhileSyntax(WhileStatementSyntax whileSyntax, int nesting)
        {
            int score = 1;

            var condition = whileSyntax.Condition.DescendantNodesAndSelf(exp => exp.GetType() == typeof(BinaryExpressionSyntax)).Where(exp => exp.GetType() != typeof(IdentifierNameSyntax)).Where(exp => exp.GetType() != typeof(LiteralExpressionSyntax)).Select(node => node.Kind().ToString()).GroupBy(name => name);
            score += condition.Count();

            var nested = whileSyntax.DescendantNodes().OfType<StatementSyntax>();
            score += AnalyzeStatements(new SyntaxList<StatementSyntax>(nested), nesting+1);

            return score;
        }

        private static int AnalyzeTrySyntax(TryStatementSyntax trySyntax, int nesting)
        {
            int score = 1;

            score += trySyntax.Catches.Sum(cautch => AnalyzeStatements(cautch.Block.Statements, nesting));

            return score;
        }

        private static int AnalyzeSwitchStatement(SwitchStatementSyntax switchStatement, int nesting)
        {
            int score = 1;

            var nested = switchStatement.DescendantNodes().OfType<StatementSyntax>();

            if (nested.Any())
                score += AnalyzeStatements(new SyntaxList<StatementSyntax>(nested), nesting);

            return score + nesting;
        }

        private static int AnalyzeIfStatement(IfStatementSyntax ifStatement, int nesting)
        {
            int score = 1;

            var nested = ifStatement.Statement.DescendantNodesAndSelf().OfType<StatementSyntax>();
            if (ifStatement.Else != null)
                nested = nested.Concat(ifStatement.Else.Statement.DescendantNodesAndSelf().OfType<StatementSyntax>());

            if (nested.Any())
                score += AnalyzeStatements(new SyntaxList<StatementSyntax>(nested), nesting + 1);

            var condition = ifStatement.Condition.DescendantNodesAndSelf(exp => exp.GetType() == typeof(BinaryExpressionSyntax)).Where(exp => exp.GetType() != typeof(IdentifierNameSyntax)).Where(exp => exp.GetType() != typeof(LiteralExpressionSyntax)).Select(node => node.Kind().ToString()).GroupBy(name => name);
            score += condition.Count();

            return score + nesting;
        }
    }
}
