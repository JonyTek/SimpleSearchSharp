using FluentAssertions;
using Lucene.Net.Analysis.Standard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSearch.Core;
using SimpleSearch.Core.Excpetions;
using SimpleSearch.Core.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SimpleSearch.Specs.Search
{
    [TestClass]
    public class SearchIndexSpecs
    {
        [TestCleanup]
        public void CleanUp()
        {
            SimpleSearch<StandardAnalyzer, BasicSchema>.Instance.Dispose();
        }

        [TestMethod]
        public void ShouldAddDociumentsToIndex()
        {
            using (var searchIndex = SimpleSearch<StandardAnalyzer, BasicSchema>
                .Init(DataSeeder.PathToIndex)
                .Index)
            {
                searchIndex.DeleteAll();

                searchIndex.IndexDirectory = DataSeeder.PathToIndex;
                Enumerable.Range(1, 50)
                    .ToList()
                    .ForEach(i => searchIndex.AddUpdateLuceneIndex(new BasicSchema
                    {
                        Id = i,
                        Text = "My text"
                    }));

                searchIndex.Length().Should().Be(50);
                searchIndex.DeleteAll();
            }
        }

        //Ignore to save time
        [Ignore]
        [TestMethod]
        public void ShouldAddDocumentsToIndexAcrossMultipleThreads()
        {
            using (var searchIndex = SimpleSearch<StandardAnalyzer, BasicSchema>
                .Init(DataSeeder.PathToIndex)
                .Index)
            {
                //Spawn up 10 threads and insert 50 docs per thread;

                try
                {
                    Enumerable.Range(1, 10).AsParallel().ForAll(_ =>
                    {
                        Debug.WriteLine("Thread: " + Thread.CurrentThread.ManagedThreadId);
                        Enumerable.Range(1, 50)
                            .ToList()
                            .ForEach(
                                i => searchIndex.AddUpdateLuceneIndex(new BasicSchema
                                {
                                    Id = i,
                                    Text = "My text"
                                }));
                    });
                }
                finally
                {
                    Debug.WriteLine("TOTAL ITEMS: " + searchIndex.Length());
                    searchIndex.DeleteAll();
                }

                true.Should().Be(true);
            }
        }

        [TestMethod]
        public void ShouldThrowIfSchechaHasNotBeenConfigured()
        {
            using (var searchIndex = SearchIndex<StandardAnalyzer, BasicSchema1>.Instance)
            {
                Action action = () => searchIndex.AddUpdateLuceneIndex(new BasicSchema1
                {
                    Id = 4,
                    Text = "text"
                });

                action.ShouldThrow<SchemaException>();
            }
        }

        [TestMethod]
        public void ShouldDeleteASingleItemByItsId()
        {
            using (var searchIndex = SimpleSearch<StandardAnalyzer, BasicSchema>
                .Init(DataSeeder.PathToIndex)
                .Index)
            {
                searchIndex.DeleteAll();

                var doc = new BasicSchema
                {
                    Id = 4,
                    Text = "text"
                };

                searchIndex.AddUpdateLuceneIndex(doc);
                searchIndex.Length().Should().Be(1);
                searchIndex.DeleteByIdentifier(doc);
                searchIndex.Length().Should().Be(0);
            }
        }

        [TestMethod]
        public void ShouldDeleteASingleItemByItsId1()
        {
            using (var searchIndex = SimpleSearch<StandardAnalyzer, BasicSchema>
                .Init(DataSeeder.PathToIndex)
                .Index)
            {
                searchIndex.DeleteAll();

                var doc = new BasicSchema
                {
                    Id = 4,
                    Text = "text"
                };
                var doc1 = new BasicSchema
                {
                    Id = 5,
                    Text = "text"
                };
                searchIndex.AddUpdateLuceneIndex(new[] {doc, doc1} as IEnumerable<BasicSchema>);

                searchIndex.Length().Should().Be(2);
                searchIndex.DeleteByIdentifier(doc);
                searchIndex.Length().Should().Be(1);
            }
        }
    }
}