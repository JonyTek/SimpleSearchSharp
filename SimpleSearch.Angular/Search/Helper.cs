using System.Collections.Generic;
using SimpleSearch.Core.Models;

namespace SimpleSearch.Angular.Search
{
    public class Helper
    {
        /// <summary>
        /// Default empty result set
        /// </summary>
        public static SearchResultSet<Schema> EmptyResultSet
        {
            get
            {
                return new SearchResultSet<Schema>
                {
                    Results = new List<Schema>()
                };
            }
        }  
    }
}