using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TogoFogo.Models;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace TogoFogo.Permission
{
    public class PermissionBasedAuthorize : AuthorizeAttribute
    {
        // Custom property
        private readonly string _connectionString =
           ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public Actions[] AccessLevel { get; set; }
        public int  MenuName { get; set; }
        public PermissionBasedAuthorize(Actions[] actionRights, int menu)
        {
            AccessLevel = actionRights;
            MenuName = menu;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            bool Valid = false;
            if (!isAuthorized)
                Valid = false;
            if (HttpContext.Current.Session["User"] != null)
            {
                var session = HttpContext.Current.Session["User"] as SessionModel;
                if (session.UserRole.ToLower().Contains("super admin"))
                {
                    Permissions.AssignRight(new UserActionRights { Create = true, Edit = true, ExcelExport = true, History = true, View = true });
                    return true;
                }
                else
                {
                    int UserId = session.UserId;
                    var menues = session.Menues as MenuMasterModel;
                    string privilegeLevels = menues.SubMenuList.Where(x => x.MenuCapId== MenuName).Select(x => x.ActionIds).FirstOrDefault();                   
                    if (AccessLevel.Length > 0 && !string.IsNullOrEmpty(privilegeLevels))
                    {
                        string[] items = privilegeLevels.Split(',');
                        var UserActionRights = new UserActionRights();
                        for (int i = 0; i < items.Length; i++)
                        {
                            if (Convert.ToInt32(items[i]) == 1)
                                UserActionRights.View = true;
                            if (Convert.ToInt32(items[i]) == 2)
                                UserActionRights.Create = true;
                            if (Convert.ToInt32(items[i]) == 3)
                                UserActionRights.Edit = true;
                            if (Convert.ToInt32(items[i]) == 4)
                                UserActionRights.Delete = true;
                            if (Convert.ToInt32(items[i]) == 5)
                                UserActionRights.History = true;
                            if (Convert.ToInt32(items[i]) == 6)
                                UserActionRights.ExcelExport = true;

                        }
                        Permissions.AssignRight(UserActionRights);
                        if (privilegeLevels.Contains(((int)AccessLevel[0]).ToString()) == true)
                            Valid = true;
                        else
                            Valid = false;
                    }
                }
            }
            else
                Valid = false;

            return Valid;
        }      
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (HttpContext.Current.Session["User"] != null)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Unauthorized",
                                action = "Index"
                            })
                        );
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                                       new RouteValueDictionary(
                                           new
                                           {
                                               controller = "Account",
                                               action = "Login"
                                           })
                                       );

            }
        }
    }

    public class CustomHandleError : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
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
    }
}