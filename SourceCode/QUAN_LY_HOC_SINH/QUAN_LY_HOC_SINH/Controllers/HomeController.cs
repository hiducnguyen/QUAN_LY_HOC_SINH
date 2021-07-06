using Resources;
using System.Web.Mvc;

namespace QUAN_LY_HOC_SINH.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Title = Resource.HomePage;
            return View();
        }
    }
}