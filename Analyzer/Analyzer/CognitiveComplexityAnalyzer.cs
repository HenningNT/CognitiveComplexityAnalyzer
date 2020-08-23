using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Linq;

namespace HenningNT.Analyzer
{
    public class CognitiveComplexityAnalyzer
    {
        public static int AnalyzeMethod(MethodDeclarationSyntax methodSyntax )
        {
            return AnalyzeMethod(methodSyntax, 0);
        }

        private static int AnalyzeMethod(MethodDeclarationSyntax methodSyntax, int nesting)
        {
            int score = 0;
            foreach (var item in methodSyntax.Body.Statements)
            {
                if (item is IfStatementSyntax ifStatement)
                {
                    score = AnalyzeIfStatement(ifStatement, nesting);
                }
            }
            return score ;
        }

        private static int AnalyzeIfStatement(IfStatementSyntax ifStatement, int nesting)
        {
            int score = 1;
            var condition = ifStatement.Condition.DescendantNodesAndSelf(exp => exp.GetType() == typeof(BinaryExpressionSyntax));
            if (condition.Count() != 0)
            {

            }
               
            return score;
        }
    }
}
