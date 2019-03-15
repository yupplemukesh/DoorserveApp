using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class PromoCodeController : Controller
    {
        private readonly string _connectionString =
           ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // GET: PromoCode
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Promocode")]
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            var _UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(_UserActionRights);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Promocode")]
        public ActionResult AddPromoCode()
        {
            var promocode = new PromoCodeModel();
            return PartialView(promocode);
        }
        [HttpPost]
        public ActionResult AddPromoCode(PromoCodeModel m)
        {
            if (ModelState.IsValid)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Insert_into_MstPromoCode", new {m.PromoCode,m.Amount,m.FromDate,m.ToDate },
                 commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {
                        TempData["Message"] = "Submitted Successfully";
                    }
                    else {
                        TempData["Message"] = "Something Went Wrong";
                    }
                }

            }
            return RedirectToAction("Index");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Promocode")]
        public ActionResult PromoCodeTable()
        {
            PromoCodeModel obj_promocode = new PromoCodeModel();
            using (var con = new SqlConnection(_connectionString))
            {
                obj_promocode._PromoCodeList = con.Query<PromoCodeModel>("Select * from MstPromoCode", new { }, commandType: CommandType.Text).ToList();
                
               // return View(result);
            }
            obj_promocode._UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(obj_promocode);

        }

    }
}