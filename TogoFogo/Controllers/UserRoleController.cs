using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;
using System.Data.OleDb;
using System.Data;

namespace TogoFogo.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        public ActionResult AddUserRole(Int64 id=0)
        {
            UserRole objUserRole = new UserRole();
            objUserRole.IsActive = true;
            Int64 RoleId = id;
            if(id!=0)
            using (var con = new SqlConnection(_connectionString))
            {
                objUserRole = con.Query<UserRole>("UspGetUserRoleDetail", new { RoleId },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            using (var con = new SqlConnection(_connectionString))
            {
                objUserRole._MenuList = con.Query<MenuMasterModel>("UspGetMenu",
                    new { RoleId }, commandType: CommandType.StoredProcedure).ToList();
            }
            return View(objUserRole);
        }
        [HttpPost]
        public ActionResult AddUserRole(UserRole objUserRole)
        {
            objUserRole.UserLoginId = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));
            string MenuList = string.Empty;
           
            var SlectedMenuLis = objUserRole._MenuList.Where(m => m.CheckedStatus == true).ToList();
            for(int i=0;i< SlectedMenuLis.Count;i++)
            {
                MenuList +=Convert.ToString(SlectedMenuLis[i].MenuCap_ID) + ",";
            }
            if(!string.IsNullOrEmpty(MenuList))
            {
                MenuList= MenuList.Substring(0, MenuList.Length - 1);
            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("UspInsertUserRole",
                    new
                    {
                        objUserRole.RoleId,
                        objUserRole.RoleName,
                        objUserRole.IsActive,
                        objUserRole.Comments,
                        objUserRole.UserLoginId,
                        MenuList

                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 0)
                {
                    TempData["Message"] = "Something went wrong";

                }
                else if (result == 1)
                {
                    TempData["Message"] = "Successfully Added";
                }
                else
                {
                    TempData["Message"] = "Successfully Updated";
                }
            }
            return View();
        }
        public ActionResult UserRoleList()
        {
            Int64 RoleId = 0;
            
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UserRole>("UspGetUserRoleDetail", new { RoleId },
                    commandType: CommandType.StoredProcedure).ToList();

                
                return View(result);
            }
          
        }
       // [HttpPost]
       // public ActionResult SaveUserRole(List<Menu> objMenu,UserRole objUserRole)
       // {
                    //objRepairRequestDetail = (RepairRequestDetail)TempData["RepairRequestDetail"];
                   //objRepairRequestDetail._selectDevice = Problems;
                  //objRepairRequestDetail.TotalEstimatedPrice = Problems.Sum(x => x.Estimated_Price);
                 //if (objRepairRequestDetail.PromoCodeValue > 0)
                //objRepairRequestDetail.TotalEstimatedPrice = objRepairRequestDetail.TotalEstimatedPrice - objRepairRequestDetail.PromoCodeValue;
               //objRepairRequestDetail.TotalMarketPrice = Problems.Sum(x => x.Market_Price);
              //objRepairRequestDetail.Warnty = warnty;
             //TempData["RepairRequestDetail"] = objRepairRequestDetail;
            //TempData.Keep("RepairRequestDetail");
          
          //  return Json("success", JsonRequestBehavior.AllowGet);
       // }
        //public JsonResult BindMenu()
       //{
      //    using (var con = new SqlConnection(_connectionString))
     //    {
    //        var result = con.Query<MenuMasterModel>("UspGetMenu",
   //        new {  }, commandType: CommandType.StoredProcedure).ToList();
  //        return Json(result, JsonRequestBehavior.AllowGet);
 //    }
//}
    }

}