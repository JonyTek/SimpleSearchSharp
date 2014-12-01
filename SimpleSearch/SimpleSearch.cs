using System;
using Lucene.Net.Analysis;
using SimpleSearch.Core.Helpers;
using SimpleSearch.Core.Schema;
using SimpleSearch.Core.Search;

namespace SimpleSearch.Core
{
    public class SimpleSearch<TAnalyzer, TSchema> : IDisposable
        where TAnalyzer : Analyzer
        where TSchema : BaseSchema, new()
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static SimpleSearch<TAnalyzer, TSchema> _instance;

        /// <summary>
        /// Thread lock for below
        /// </summary>
        // ReSharper disable once StaticFieldInGenericType
        private static readonly object ThreadLock = new object();

        /// <summary>
        /// Private ctor
        /// </summary>
        private SimpleSearch()
        {
            Index.SetSchema();
        }

        /// <summary>
        /// Singleton implementation
        /// </summary>
        public static SimpleSearch<TAnalyzer, TSchema> Instance
        {
            get
            {
                lock (ThreadLock)
                {
                    return _instance
                           ?? (_instance = new SimpleSearch<TAnalyzer, TSchema>());
                }
            }
        }

        /// <summary>
        /// Sets the path to the search index.
        /// </summary>
        /// <param name="indexLocation"></param>
        public static SimpleSearch<TAnalyzer, TSchema> Init(string indexLocation)
        {
            Validate.NotNull(indexLocation, "You must provide a path for you index.");

            SearchIndex<TAnalyzer, TSchema>.Instance.IndexDirectory = indexLocation.TrimEnd('\\');

            lock (ThreadLock)
            {
                return _instance
                       ?? (_instance = new SimpleSearch<TAnalyzer, TSchema>());
            }
        }

        /// <summary>
        /// Stores the public SearchIndex instance
        /// </summary>
        public SearchIndex<TAnalyzer, TSchema> Index
        {
            get { return SearchIndex<TAnalyzer, TSchema>.Instance; }
        }

        /// <summary>
        /// Stores the public Searcher instance
        /// </summary>
        public Searcher<TAnalyzer, TSchema> Searcher
        {
            get { return Searcher<TAnalyzer, TSchema>.Instance; }
        }

        /// <summary>
        /// Store a sort builder instance
        /// </summary>
        /// <returns></returns>
        public SortBuilder<TSchema> SortBuilder
        {
            get { return SortBuilder<TSchema>.Instance; }
        }

        /// <summary>
        /// Store a query builder instance
        /// </summary>
        /// <returns></returns>
        public QueryBuilder<TSchema> QueryBuilder
        {
            get { return QueryBuilder<TSchema>.Instance; }
        }

        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            _instance = null;
            Index.Dispose();
            Searcher.Dispose();
        }
    }
}