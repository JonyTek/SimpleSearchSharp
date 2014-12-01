using System.Web.UI.WebControls;
using SimpleSearch.Angular.Models;
using SimpleSearch.Angular.Search;
using System;
using System.Web.Http;

namespace SimpleSearch.Angular.Controllers
{
    public class AdvancedSearchController : ApiController
    {
        public IHttpActionResult Get(
            string term,
            string sortBy,
            string notContains = null,
            string createdAfter = null,
            string createdBefore = null,
            string category = null)
        {
            try
            {
                return Ok(new SearchViewModel
                {
                    ResultSet = string.IsNullOrEmpty(term)
                        ? Helper.EmptyResultSet
                        : Searcher.AdvancedSearch(term, notContains, createdAfter, createdBefore, category, sortBy),
                    OrderByOptions = Searcher.SimpleSearch.SortBuilder.GetSortableFields()
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Get(string term)
        {
            return Ok(new SearchViewModel
            {
                ResultSet = string.IsNullOrEmpty(term)
                    ? Helper.EmptyResultSet
                    : Searcher.BasicSearch(term),
                OrderByOptions = Searcher.SimpleSearch.SortBuilder.GetSortableFields()
            });
        }
    }
}
