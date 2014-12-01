using System;
using Lucene.Net.Analysis.Standard;
using SimpleSearch.Core;
using SimpleSearch.Core.Models;

namespace SimpleSearch.Angular.Search
{
    public static class Searcher
    {
        //You can pass in any valid Lucene.Net analyzer
        //KeywordAnalyzer 
        //SimpleAnalyzer 
        //StopAnalyzer 
        //WhitespaceAnalyzer 
        //StandardAnalyzer
        public static readonly SimpleSearch<StandardAnalyzer, Schema> SimpleSearch;

        private const string PathToIndex = @"C:\Working\Sandbox\SimpleSearch\Index";

        static Searcher()
        {
            SimpleSearch = SimpleSearch<StandardAnalyzer, Schema>.Init(PathToIndex);
        }

        /// <summary>
        /// Basic search functionality used in page header
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public static SearchResultSet<Schema> BasicSearch(string term)
        {
            var query = SimpleSearch.QueryBuilder
                .WildCardLike(schema => schema.Body, term)
                .OrWildcardLike(schema => schema.Heading, term)
                .ToString();

            var sort = SimpleSearch.SortBuilder
                .SortBy<Schema>(null);

            return SimpleSearch.Searcher.Search(query, sort);
        }

        /// <summary>
        /// Advanced search functionality used for search page
        /// </summary>
        /// <param name="term"></param>
        /// <param name="notContains"></param>
        /// <param name="createdAfter"></param>
        /// <param name="createdBefore"></param>
        /// <param name="category"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public static SearchResultSet<Schema> AdvancedSearch(
            string term,
            string notContains = null,
            string createdAfter = null,
            string createdBefore = null,
            string category = null,
            string sortBy = null)
        {
            var query = SimpleSearch.QueryBuilder
                .WildCardLike(schema => schema.AllText, term);

            if (notContains != null)
                query.AndNotWildcardLike(schema => schema.AllText, notContains);
            if (createdAfter != null)
            {
                var when = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Convert.ToDouble(createdAfter));
                query.AndDateAfter(schema => schema.CreatedAt, when);
            }
            if (createdBefore != null)
            {
                var when = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Convert.ToDouble(createdBefore));
                query.AndDateBefore(schema => schema.CreatedAt, when);
            }
            if (category != null)
                query.ContainsKeyword(schema => schema.CategoryId, category);

            var sort = SimpleSearch.SortBuilder
                .SortBy(sortBy);

            return SimpleSearch.Searcher.Search(query.ToString(), sort);
        }
    }
}