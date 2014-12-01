using FluentAssertions;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSearch.Core;

namespace SimpleSearch.Specs.Helpers
{
    [TestClass]
    public class ConfiguarationSpecs
    {
        [TestMethod]
        public void ShouldReturnTrueIfSchemaTypesMatch()
        {
            var match = SimpleSearch<StandardAnalyzer, MySchema>
                .Instance
                .Index
                .SchemasMatch<MySchema>();
            
            match.Should().BeTrue();
        }
    
       [TestMethod]
        public void ShouldReturnFalseIfSchemaTypesDontMatch()
       {
           var match = SimpleSearch<StandardAnalyzer, MySchema>
               .Instance
               .Index
               .SchemasMatch<MyExceptionSchema>();

            match.Should().BeFalse();
        }

       [TestMethod]
       public void ShouldBeAbleToCaterForDifferentAnalyzerTypes()
       {
           SimpleSearch<KeywordAnalyzer, BasicSchema>
               .Init(DataSeeder.PathToIndex)
               .Index
               .AddUpdateLuceneIndex(new BasicSchema { Id = 1, Text = "DFGHJK" });

           SimpleSearch<SimpleAnalyzer, BasicSchema>
              .Init(DataSeeder.PathToIndex)
              .Index
              .AddUpdateLuceneIndex(new BasicSchema { Id = 1, Text = "DFGHJK" });

           SimpleSearch<StopAnalyzer, BasicSchema>
              .Init(DataSeeder.PathToIndex)
              .Index
              .AddUpdateLuceneIndex(new BasicSchema { Id = 1, Text = "DFGHJK" });

           SimpleSearch<WhitespaceAnalyzer, BasicSchema>
              .Init(DataSeeder.PathToIndex)
              .Index
              .AddUpdateLuceneIndex(new BasicSchema { Id = 1, Text = "DFGHJK" });

           SimpleSearch<StandardAnalyzer, BasicSchema>
              .Init(DataSeeder.PathToIndex)
              .Index
              .AddUpdateLuceneIndex(new BasicSchema { Id = 1, Text = "DFGHJK" });

       }
    }
}