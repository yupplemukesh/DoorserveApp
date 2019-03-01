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
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage User Roles")]
        public ActionResult AddUserRole(Int64 id=0)
        {
            UserRole objUserRole = new UserRole();
            objUserRole.IsActive = true;
            Int64 RoleId = id;
            using (var con = new SqlConnection(_connectionString))
            {
                objUserRole._MenuList = con.Query<MenuMasterModel>("UspGetMenuByRole",
                    new { RoleId }, commandType: CommandType.StoredProcedure).ToList();
            }
            return View(objUserRole);
        }
        [HttpPost]
        public ActionResult AddUserRole(UserRole objUserRole)
        {
            objUserRole.UserLoginId = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));
            string MenuList = string.Empty;
            ResponseModel objResponseModel = new ResponseModel();
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
                 
                    objResponseModel.IsSuccess = false;
                    objResponseModel.ResponseCode = 0;
                    objResponseModel.Response = "Something went wrong";
                    TempData["response"] = objResponseModel;
                }
                else if (result == 1)
                {
                  

                    objResponseModel.IsSuccess = true;
                    objResponseModel.ResponseCode = 1;
                    objResponseModel.Response = "Successfully Added";
                    TempData["response"] = objResponseModel;
                }
                else if (result == 3)
                {
                    

                    objResponseModel.IsSuccess = false;
                    objResponseModel.ResponseCode = 3;
                    objResponseModel.Response = "Role Name already exists";
                    TempData["response"] = objResponseModel;
                }
                else
                {
                   

                    objResponseModel.IsSuccess = true;
                    objResponseModel.ResponseCode = 2;
                    objResponseModel.Response = "Successfully Updated";
                    TempData["response"] = objResponseModel;

                }
                return RedirectToAction("UserRoleList", "UserRole");
            }
           
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage User Roles")]
        public ActionResult EditUserRole(Int64 id = 0)
        {
            UserRole objUserRole = new UserRole();           
            Int64 RoleId = id;           
                using (var con = new SqlConnection(_connectionString))
                {
                    objUserRole = con.Query<UserRole>("UspGetUserRoleDetail", new { RoleId },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            using (var con = new SqlConnection(_connectionString))
            {
                objUserRole._MenuList = con.Query<MenuMasterModel>("UspGetMenuByRole",
                    new { RoleId }, commandType: CommandType.StoredProcedure).ToList();
            }
            return View(objUserRole);
        }
        [HttpPost]
        public ActionResult EditUserRole(UserRole objUserRole)
        {
            objUserRole.UserLoginId = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));
            string MenuList = string.Empty;
            ResponseModel objResponseModel = new ResponseModel();
            var SlectedMenuLis = objUserRole._MenuList.Where(m => m.CheckedStatus == true).ToList();
            for (int i = 0; i < SlectedMenuLis.Count; i++)
            {
                MenuList += Convert.ToString(SlectedMenuLis[i].MenuCap_ID) + ",";
            }
            if (!string.IsNullOrEmpty(MenuList))
            {
                MenuList = MenuList.Substring(0, MenuList.Length - 1);
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

                    objResponseModel.IsSuccess = false;
                    objResponseModel.ResponseCode = 0;
                    objResponseModel.Response = "Something went wrong";
                    TempData["response"] = objResponseModel;
                }
                else if (result == 1)
                {


                    objResponseModel.IsSuccess = true;
                    objResponseModel.ResponseCode = 1;
                    objResponseModel.Response = "Successfully Added";
                    TempData["response"] = objResponseModel;
                }
                else if (result == 3)
                {


                    objResponseModel.IsSuccess = false;
                    objResponseModel.ResponseCode = 3;
                    objResponseModel.Response = "Role Name already exists";
                    TempData["response"] = objResponseModel;
                }
                else
                {


                    objResponseModel.IsSuccess = true;
                    objResponseModel.ResponseCode = 2;
                    objResponseModel.Response = "Successfully Updated";
                    TempData["response"] = objResponseModel;

                }
                return RedirectToAction("UserRoleList", "UserRole");
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage User Roles")]
        public ActionResult UserRoleList()
        {
           
            Int64 RoleId = 0;
            UserRole objUserRole = new UserRole();            
            using (var con = new SqlConnection(_connectionString))
            {
                objUserRole.RoleList = con.Query<UserRole>("UspGetUserRoleDetail", new { RoleId },
                    commandType: CommandType.StoredProcedure).ToList();                             
            }
            UserActionRights objUserActiobRight = new UserActionRights();
            objUserRole._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objUserRole._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objUserRole._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objUserRole._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objUserRole._UserActionRights.ExcelExport = true;
                    }
                }
            }
            else
            {

                objUserRole._UserActionRights.Create = true;
                objUserRole._UserActionRights.Edit = true;
                objUserRole._UserActionRights.Delete = true;
                objUserRole._UserActionRights.View = true;
                objUserRole._UserActionRights.History = true;
                objUserRole._UserActionRights.ExcelExport = true;

            }
            return View(objUserRole);

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