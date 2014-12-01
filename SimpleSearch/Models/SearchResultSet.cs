using System.Collections.Generic;
using SimpleSearch.Core.Schema;

namespace SimpleSearch.Core.Models
{
    public class SearchResultSet<TSchema>
        where TSchema : BaseSchema, new()
    {
        /// <summary>
        /// Actulal results of type TSchema
        /// </summary>
        public IEnumerable<TSchema> Results { get; set; }

        /// <summary>
        /// The number of documents within this set
        /// </summary>
        public int SetCount { get; set; }

        /// <summary>
        /// The number of documents that match a query
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The actual lucene query
        /// </summary>
        public string Query { get; set; }
    }
}