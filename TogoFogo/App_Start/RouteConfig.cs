using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TogoFogo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                        name: "DefaultBlog",
                        url: "{controller}/{action}/{CC_no}",
                        defaults: new
                        {
                            controller = "ServiceCenter",
                            action = "PrintShippingLabel",
                            CC_no = UrlParameter.Optional
                        }
                        );
        }

    }
}
