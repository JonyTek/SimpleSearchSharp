using System.Web.Mvc;

namespace SimpleSearch.Angular.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet]
        public ActionResult Index(string term)
        {
            return View();
        }
    }
}