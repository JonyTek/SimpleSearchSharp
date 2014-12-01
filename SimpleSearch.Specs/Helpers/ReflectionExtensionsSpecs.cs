using FluentAssertions;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSearch.Core.Helpers;

namespace SimpleSearch.Specs.Helpers
{
    [TestClass]
    public class ReflectionExtensionsSpecs
    {
        [TestMethod]
        public void ShouldCreateAnalyzers()
        {
            typeof(KeywordAnalyzer).Instantiate<KeywordAnalyzer>().Should().NotBeNull();
            typeof(SimpleAnalyzer).Instantiate<KeywordAnalyzer>().Should().NotBeNull();
            typeof(StopAnalyzer).Instantiate<KeywordAnalyzer>().Should().NotBeNull();
            typeof(WhitespaceAnalyzer).Instantiate<KeywordAnalyzer>().Should().NotBeNull();
            typeof(StandardAnalyzer).Instantiate<KeywordAnalyzer>().Should().NotBeNull();
        }
    }
}