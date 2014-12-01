using Lucene.Net.Analysis.Standard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSearch.Core;
using System;

namespace SimpleSearch.Specs
{
    [Ignore]
    [TestClass]
    public class Usage
    {
        [TestMethod]
        public void Search()
        {
            var simpleSearch = SimpleSearch<StandardAnalyzer, MySchema>.Init(DataSeeder.PathToIndex);

            simpleSearch.Index.Optimize();

            var sort = simpleSearch.SortBuilder
                .SortBy(schema => schema.Text);

            var query = simpleSearch.QueryBuilder
                .WildCardLike(schema => schema.Text, "Some search term")
                .DateAfter(schema => schema.Date, DateTime.Now)
                .ToString();

            var results = simpleSearch.Searcher.Search(query, sort);
        }

    }
}