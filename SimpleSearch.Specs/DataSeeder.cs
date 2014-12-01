using Lucene.Net.Analysis.Standard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSearch.Core;
using System;

namespace SimpleSearch.Specs
{
    [TestClass]
    public class DataSeeder
    {
        public static string PathToIndex = @"C:\Working\Sandbox\SimpleSearch\Index";

        [TestMethod]
        public void Seed()
        {
            var simpleSearch = SimpleSearch<StandardAnalyzer, BasicSchema2>.Init(PathToIndex);
            var searchIndex = simpleSearch.Index;
            searchIndex.DeleteAll();

            var demoData = new[]
            {
                new BasicSchema2
                {
                    Id = 1,
                    Text = "Foo bar spar",
                    Text1 = "quick fox jump over",
                    Date = new DateTime(2000, 1, 1),
                    CategoryId = 100
                },
                new BasicSchema2
                {
                    Id = 2,
                    Text = "Red cat fat",
                    Text1 = "Fast car drives",
                    Date = new DateTime(2001, 1, 1),
                    CategoryId = 200
                },
                new BasicSchema2
                {
                    Id = 3,
                    Text = "Blue dog jump spar",
                    Text1 = "Slow car drives",
                    Date = new DateTime(2002, 1, 1),
                    CategoryId = 300
                },
            };

            searchIndex.AddUpdateLuceneIndex(demoData);
        }
    }

    public static class Seeder
    {
        public static void Seed()
        {
            new DataSeeder().Seed();
        }
    }
}
