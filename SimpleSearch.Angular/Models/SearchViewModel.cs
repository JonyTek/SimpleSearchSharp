using System.Collections.Generic;
using SimpleSearch.Angular.Search;
using SimpleSearch.Core.Models;

namespace SimpleSearch.Angular.Models
{
    public class SearchViewModel
    {
        public SearchResultSet<Schema> ResultSet { get; set; }

        public Dictionary<string, string> OrderByOptions { get; set; }
    }
}