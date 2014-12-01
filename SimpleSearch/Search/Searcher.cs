using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using SimpleSearch.Core.Helpers;
using SimpleSearch.Core.Models;
using SimpleSearch.Core.Schema;
using System;
using Version = Lucene.Net.Util.Version;

namespace SimpleSearch.Core.Search
{
    public class Searcher<TAnalyzer, TSchema> : IDisposable
        where TSchema : BaseSchema, new()
        where TAnalyzer : Analyzer
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static Searcher<TAnalyzer, TSchema> _instance;

        /// <summary>
        /// Thread lock for below
        /// </summary>        
        // ReSharper disable once StaticFieldInGenericType
        private static readonly object ThreadLock = new object();

        /// <summary>
        /// Private ctor
        /// </summary>
        private Searcher()
        {
        }

        /// <summary>
        /// Singleton implementation
        /// </summary>
        internal static Searcher<TAnalyzer, TSchema> Instance
        {
            get
            {
                lock (ThreadLock)
                {
                    return _instance
                           ?? (_instance = new Searcher<TAnalyzer, TSchema>());
                }
            }
        }

        /// <summary>
        /// Performs a basic text search with optional sort.
        /// If a null value is passed for sort it will return by Relavance.
        /// </summary>
        /// <param name="luceneQuery"></param>
        /// <param name="sort"></param>
        /// <param name="limitTo"></param>
        /// <returns></returns>
        public SearchResultSet<TSchema> Search(string luceneQuery, Sort sort, int limitTo = 9999)
        {
            Validate.NotNull(sort, "No sort has been specified.");
            Validate.StringNotEmpty(luceneQuery, "No search query specified.");

            var directory = SearchIndex<TAnalyzer, TSchema>.Instance.Directory;

            using (var searcher = new IndexSearcher(directory, false))
            {
                using (var analyzer = typeof (TAnalyzer).Instantiate<TAnalyzer>())
                {
                    var parser = new QueryParser(Version.LUCENE_30, "text", analyzer);
                    var query = parser.Parse(luceneQuery);

                    var hits = searcher.Search(query, null, limitTo, sort).ScoreDocs;
                    var results = hits.TakeWhile((doc, index) => index <= hits.Length)
                        .Select(t => searcher.Doc(t.Doc).ToSchema<TSchema>())
                        .ToList();

                    return new SearchResultSet<TSchema>
                    {
                        Results = results,
                        SetCount = results.Count,
                        TotalCount = hits.Length,
                        Query = luceneQuery
                    };
                }
            }
        }

        /// <summary>
        /// Return everything contained within an index
        /// </summary>
        /// <returns></returns>
        public SearchResultSet<TSchema> Everything()
        {
            var directory = SearchIndex<TAnalyzer, TSchema>.Instance.Directory;

            using (var searcher = new IndexSearcher(directory, false))
            {
                var searchQuery = new TermQuery(new Term("IDENTITY", "identity"));
                var hits = searcher.Search(searchQuery, 9999999).ScoreDocs;
                var results = hits.TakeWhile((t, i) => i <= hits.Length)
                        .Select(t => searcher.Doc(t.Doc).ToSchema<TSchema>())
                        .ToList();

                return new SearchResultSet<TSchema>
                {
                    Results = results,
                    SetCount = results.Count,
                    TotalCount = results.Count,
                    Query = "*"
                };
            }
        } 

        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            _instance = null;
        }
    }
}