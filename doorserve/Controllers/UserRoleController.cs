using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using doorserve.Models;
using System.Data.OleDb;
using System.Data;
using doorserve.Permission;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace doorserve.Controllers
{
    public class UserRoleController : BaseController
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_User_Roles)]
        public ActionResult AddUserRole(Int64 id=0)
        {

            UserRole objUserRole = new UserRole();
            objUserRole.IsActive = true;
            Int64 RoleId = id;           
            using (var con = new SqlConnection(_connectionString))
            {
                objUserRole._MenuList = con.Query<MenuMasterModel>("UspGetMenuByRole",
                    new { RoleId }, commandType: CommandType.StoredProcedure).ToList();
                var menuList = objUserRole._MenuList.Where(x => x.ParentMenuId==0).ToList();
                foreach (var item in menuList)
                {
                    var subMenues = objUserRole._MenuList.Where(x => x.ParentMenuId == item.MenuCapId).ToList();
                    item.SubMenuList = subMenues;
                }
                objUserRole._MenuList = menuList;
            }
            return View(objUserRole);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_User_Roles)]
        [HttpPost]
        public ActionResult AddUserRole(UserRole objUserRole)
        {

             
            string MenuList = string.Empty;
            ResponseModel objResponseModel = new ResponseModel();
            var SelectedMenuList = objUserRole._MenuList.Where(m => m.CheckedStatus == true).ToList();
            foreach (var item in SelectedMenuList)
            {
                if (item.SubMenuList != null)
                {
                    var menues = item.SubMenuList.Where(x => x.CheckedStatus == true).ToList();
                    item.SubMenuList = menues;
                 }

            }
            var xml = ToXML(SelectedMenuList);
            using (var con = new SqlConnection(_connectionString))
            {

                var result = con.Query<int>("UspInsertUserRole",
                    new
                    {
                        objUserRole.RoleId,
                        objUserRole.RoleName,
                        objUserRole.IsActive,
                        objUserRole.Comments,
                        UserLoginId= CurrentUser.UserId,
                        MenuList=xml,
                        CurrentUser.RefKey,
                        objUserRole.RefName,
                        companyId = CurrentUser.CompanyId
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_User_Roles)]
        public ActionResult EditUserRole(Int64 id = 0)
        {
         
            UserRole objUserRole = new UserRole();           
            Int64 RoleId = id;
            Guid? RefKey = null;
            int UserId = 0;     
                using (var con = new SqlConnection(_connectionString))
                {
                    objUserRole = con.Query<UserRole>("UspGetUserRoleDetail", new { RoleId, UserId, RefKey },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            using (var con = new SqlConnection(_connectionString))
            {
                objUserRole._MenuList = con.Query<MenuMasterModel>("UspGetMenuByRole",
                    new { RoleId }, commandType: CommandType.StoredProcedure).ToList();

                var menuList = objUserRole._MenuList.Where(x => x.ParentMenuId == 0).ToList();
                foreach (var item in menuList)
                {
                    var subMenues = objUserRole._MenuList.Where(x => x.ParentMenuId == item.MenuCapId).ToList();
                    item.SubMenuList = subMenues;

                }
                objUserRole._MenuList = menuList;

            }
            return View(objUserRole);
        }
        public string ToXML(Object oObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_User_Roles)]
        [HttpPost]
        public ActionResult EditUserRole(UserRole objUserRole)
        {


            objUserRole.UserLoginId = CurrentUser.UserId;
            string MenuList = string.Empty;
            ResponseModel objResponseModel = new ResponseModel();
            var SelectedMenuList = objUserRole._MenuList.Where(m => m.CheckedStatus == true).ToList();
            foreach (var item in SelectedMenuList)
            {
                if (item.SubMenuList != null)
                {
                    var menues = item.SubMenuList.Where(x => x.CheckedStatus == true).ToList();
                    item.SubMenuList = menues;
                }

            }
            var xml = ToXML(SelectedMenuList);
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
                        MenuList=xml,
                        objUserRole.RefKey,
                        objUserRole.RefName,
                        companyId = CurrentUser.CompanyId
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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_User_Roles)]
        public ActionResult UserRoleList()
        {
          
            Int64 RoleId = 0;
            int UserId = 0;
            if (!CurrentUser.UserRole.ToLower().Contains("super admin"))
                UserId = CurrentUser.UserId;
            Guid? RefKey= CurrentUser.RefKey;
            var objUserRole = new  List<UserRole>();          
            using (var con = new SqlConnection(_connectionString))
            { 
               
                   objUserRole = con.Query<UserRole>("UspGetUserRoleDetail", new { RoleId, UserId, RefKey },
                    commandType: CommandType.StoredProcedure).ToList();                             
            }


            return View(objUserRole);

        }    
    }

}