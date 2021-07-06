using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace QUAN_LY_HOC_SINH.Controllers
{
    public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = Request.QueryString["culture"] as string;

            if (cultureName == null)
            {
                cultureName = Session["culture"] as string;
            }

            // If cultureName still null, obtain culture from HTTP header AcceptLanguages
            if (cultureName == null)
            {
                cultureName = (Request.UserLanguages != null && Request.UserLanguages.Length > 0) ?
                        Request.UserLanguages[0] : null;
            }

            Session.Add("culture", cultureName);
            Session.Timeout = 20;

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName);

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

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