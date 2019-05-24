using System.Web;
using System.Web.Mvc;
using TogoFogo.Filters;

namespace TogoFogo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorLoggerAttribute());
          
        }
    }
}
