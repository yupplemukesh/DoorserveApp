using System.Web;
using System.Web.Mvc;
using doorserve.Filters;

namespace doorserve
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorLoggerAttribute());
          
        }
    }
}
