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
            MenuMasterModel objMenuMaster = new MenuMasterModel();
            using (var con = new SqlConnection(_connectionString))
            {

                var UserId = Session["User_ID"];

             
                objMenuMaster.ParentMenuList = con.Query<MenuMasterModel>("UspGetMenu",
               new {  }, commandType: CommandType.StoredProcedure).ToList();

                objMenuMaster.SubMenuList = con.Query<MenuMasterModel>("UspGetMenuListByUser",
                 new { UserId }, commandType: CommandType.StoredProcedure).ToList();

                return PartialView("_SideMenu", objMenuMaster);
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
          
        }
    }
}