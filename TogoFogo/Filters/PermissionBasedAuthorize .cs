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
        public string  MenuName { get; set; }
        public PermissionBasedAuthorize(Actions[] actionRights, string menu)
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
            if (HttpContext.Current.Session["User_ID"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["RoleName"]).ToLower().Contains("super admin"))
                {
                    Permissions.AssignRight(new UserActionRights { Create = true, Edit = true, ExcelExport = true, History = true, View = true });
                    return true;


                }
                else
                {
                    int UserId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]);
                    string privilegeLevels = GetUserRights(UserId).Where(x => x.Menu_Name.Contains(MenuName)).Select(x => x.ActionIds).FirstOrDefault();
                    if (AccessLevel.Length > 0 && privilegeLevels != null)
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
        private  List<MenuMasterModel> GetUserRights(int userId)
        {
            MenuMasterModel objMenuMaster = new MenuMasterModel();
            using (var con = new SqlConnection(_connectionString))
            {
                objMenuMaster.SubMenuList = con.Query<MenuMasterModel>("UspGetMenuListByUser",
              new { userId }, commandType: CommandType.StoredProcedure).ToList();
            }
            return objMenuMaster.SubMenuList;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (HttpContext.Current.Session["User_ID"] != null)
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
}