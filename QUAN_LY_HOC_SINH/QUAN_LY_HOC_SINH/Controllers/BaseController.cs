using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QUAN_LY_HOC_SINH.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;

            ViewData["ErrorMessage"] = e.Message;

            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = this.ViewData
            };

            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }
    }
}