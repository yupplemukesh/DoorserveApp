using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TogoFogo.Filters
{
    public class ErrorLoggerAttribute: HandleErrorAttribute
    {


        public override void OnException(ExceptionContext filterContext)
        {
            LogError(filterContext);
            base.OnException(filterContext);
            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            if (filterContext.Exception is HttpAntiForgeryException)
            {

            }
            filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary
                 {
                    { "action", "Index" },
                    { "controller", "ErrorPage" }
                 });

            filterContext.ExceptionHandled = true;

        }

        public void LogError(ExceptionContext filterContext)
        {
            // You could use any logging approach here

            StringBuilder builder = new StringBuilder();
            builder
                .AppendLine("----------")
                .AppendLine(DateTime.Now.ToString())
                .AppendFormat("Source:\t{0}", filterContext.Exception.Source)
                .AppendLine()
                .AppendFormat("Target:\t{0}", filterContext.Exception.TargetSite)
                .AppendLine()
                .AppendFormat("Type:\t{0}", filterContext.Exception.GetType().Name)
                .AppendLine()
                .AppendFormat("Message:\t{0}", filterContext.Exception.Message)
                .AppendLine()
                .AppendFormat("Stack:\t{0}", filterContext.Exception.StackTrace)
                .AppendLine();

            string filePath = filterContext.HttpContext.Server.MapPath("~/App_Data/Error.log");

            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.Write(builder.ToString());
                writer.Flush();
            }
        }
}
    }