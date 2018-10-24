using System.Web;
using System.Web.Mvc;

namespace Cyclr.LaunchExample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new RequireHttpsAttribute());
        }
    }
}
