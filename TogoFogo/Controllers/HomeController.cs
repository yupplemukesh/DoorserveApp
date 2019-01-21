using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TogoFogo.Extended;
using TogoFogo.Extension;
using TogoFogo.Models;

namespace TogoFogo.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString =
             ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [Authorize]
        public ActionResult DynamicLinks()
        {
            using (var con=new SqlConnection(_connectionString))
            {
                var uuuu = User.Identity.Name;
                var id = User.GetId();
                var UserId = Session["User_ID"];
                //var result=con.Query<Menu>("select distinct * from menuTable where Id in(select MenuID from user_rights where UserId=@UserID) Or MenuCap_ID in(select distinct parentmenu_id from user_rights where UserID=@UserID)", new {UserID=UserId}, commandType: CommandType.Text).ToList();
                //var result = con.Query<Menu>("Get_SideNav_Test", new { UserId }, commandType: CommandType.StoredProcedure).ToList();
                var result=con.Query<Menu>("select distinct * from menuTable where MenuCap_ID in(select MenuID from user_rights_Test where UserId=@UserID) Or MenuCap_ID in(select distinct parentmenu_id from user_rights_Test where UserID=@UserID)", new { UserID = id }, commandType: CommandType.Text).ToList();
                return PartialView("_SideMenu", result);
            }

            
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Index()
        {
                    
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel m)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<string>("Login_Proc", new { Username = m.Email, Password = m.Password },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                if ((Request.Form["Email"] == m.Email) && (Request.Form["Password"] == m.Password))
                {
                    FormsAuthentication.SetAuthCookie(Request.Form["Email"].ToString(), false);

                    return RedirectToAction("Brand", "Master");
                    //return View("About");
                }
                else
                {
                    return View("Login");
                }
            }
            return View();
        }
    }
}