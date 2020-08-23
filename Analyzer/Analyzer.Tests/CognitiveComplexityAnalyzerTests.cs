using Xunit;

namespace HenningNT.Analyzer.Tests
{
    public class CognitiveComplexityAnalyzerTests
    {
        [Fact]
        public void Test1()
        {
            var unit = new CognitiveComplexityAnalyzer();

            Assert.NotNull(unit);
        }
    }
}
