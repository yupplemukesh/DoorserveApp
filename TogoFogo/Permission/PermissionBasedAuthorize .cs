using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TogoFogo.Permission
{

    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }
            if (HttpContext.Current.Session["User_ID"] != null)
            {
                int UserId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]);
                string privilegeLevels = string.Join("", GetUserRights(UserId)); // Call another method to get rights of the user from DB

                return privilegeLevels.Contains(this.AccessLevel);
            }
            else
            return false;
        }

        private  string GetUserRights(int userID)
        {
            return "";
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Error",
                                action = "Unauthorised"
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