using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QUAN_LY_HOC_SINH.Controllers
{
    public class SettingController : BaseController
    {
        // GET: Setting/Culture
        public ActionResult Culture()
        {
            ViewBag.Title = Resource.Language;
            ViewBag.AllCultures = CultureHelper.GetSelectListOfAllCultures();
            return View();
        }

        // POST: Setting/Culture
        [HttpPost]
        public ActionResult Culture(string culture)
        {
            if (culture != null)
            {
                culture = CultureHelper.GetImplementedCulture(culture);
                Session.Add("culture", culture);
                Session.Timeout = 20;
            }
            return RedirectToAction("Culture");
        }
    }
}