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
                if (Convert.ToString(HttpContext.Current.Session["RoleName"]).ToLower() == "supper admin")
                {
                    httpContext.Items["ActionsRights"] = new UserActionRights { Create = true, Edit = true, ExcelExport = true, History = true, View = true };
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
                        httpContext.Items["ActionsRights"] = UserActionRights;

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
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Unauthorized",
                                action = "Index"
                            })
                        );
        }
    }
    //public class MyCustomAuthAttribute : FilterAttribute, IAuthorizationFilter
    //{
    //    public string Permission { get; set; }
    //    public string Groups { get; set; }

    //    public void OnAuthorization(AuthorizationContext filterContext)
    //    {
    //        bool isauthorized = CheckIfUserIsAuthorized();
    //        if (!isauthorized)
    //            context.Result = new HttpUnauthorizedResult(); // mark unauthorized

    //        // Only do something if we are about to give a HttpUnauthorizedResult and we are in AJAX mode.
    //        if (filterContext.Result is HttpUnauthorizedResult && filterContext.HttpContext.Request.IsAjaxRequest())
    //        {
    //            filterContext.Result = new JsonResult
    //            {
    //                Data = new { Success = false, Message = "Unauthorized Access" },
    //                JsonRequestBehavior = JsonRequestBehavior.AllowGet
    //            };
    //        }
    //        else
    //        {
    //            base.OnAuthorization(filterContext);
    //            if (filterContext.Result is HttpUnauthorizedResult)
    //            {
    //                HttpContext.Current.Session.Abandon();
    //                System.Web.Security.FormsAuthentication.SignOut();
    //                filterContext.Result = new RedirectResult("Your Login Page.");
    //            }
    //        }
    //    }

    //    private bool IsAuthorizedUser()
    //    {
    //        // use Permission, Groups and your logic
    //    }
    //}
    //public class PermissionBasedAuthorize : AuthorizeAttribute
    //{
    //    private readonly string _connectionString =
    //        ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    //    private List<string> ActionIds { get; set; }
    //    public PermissionBasedAuthorize(string ActionRights)
    //    {
    //        if (!string.IsNullOrEmpty(ActionRights))
    //            ActionIds = ActionRights.Split(',').ToList();
    //    }
    //    public override void OnAuthorization(HttpActionContext actionContext)
    //    {
    //        base.OnAuthorization(actionContext);
    //        var UserId = HttpContext.Current.Session["User_ID"];
    //        //ApplicationContext db = new ApplicationContext();

    //        //var Permissions = db.Permissions.Find(UserId);
    //        MenuMasterModel objMenuMaster = new MenuMasterModel();
    //        if (ActionIds == null || ActionIds.Count() == 0)
    //        {
    //            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
    //        }

    //        bool IsAllowed = false;
    //        // string pathUrl = String.Format("/{0}/{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);
    //        using (var con = new SqlConnection(_connectionString))
    //        {
    //            objMenuMaster.SubMenuList = con.Query<MenuMasterModel>("UspGetMenuListByUser",
    //            new { UserId }, commandType: CommandType.StoredProcedure).ToList();
    //        }
    //        foreach (var item in ActionIds)
    //            //foreach (var property in objMenuMaster.SubMenuList)
    //            //{
    //            //    if (property.Name.ToLower().Equals(item.ToLower()))
    //            //    {
    //            //        bool Value = (bool)property.GetValue(Permissions, null);
    //            //        if (Value)
    //            //        {
    //            //            IsAllowed = true;
    //            //        }
    //            //        break;
    //            //    }
    //            //}

    //            if (!IsAllowed)
    //            {
    //                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
    //            }
    //    }

    //}
}