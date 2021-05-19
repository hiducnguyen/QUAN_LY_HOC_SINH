using System.Web;
using System.Web.Mvc;

namespace QUAN_LY_HOC_SINH
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
