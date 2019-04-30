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
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace TogoFogo.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        private  SessionModel user; 
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage User Roles")]
        public ActionResult AddUserRole(Int64 id=0)
        {
            user = Session["User"] as SessionModel;
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
        [HttpPost]
        public ActionResult AddUserRole(UserRole objUserRole)
        {

            user = Session["User"] as SessionModel;          
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
                        UserLoginId=user.UserId,
                        MenuList=xml,
                        user.RefKey,
                        objUserRole.RefName,
                        companyId = user.CompanyId
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
             user = Session["User"] as SessionModel;        
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

        [HttpPost]
        public ActionResult EditUserRole(UserRole objUserRole)
        {

            user = Session["User"] as SessionModel;
            objUserRole.UserLoginId = user.UserId;
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
                        companyId = user.CompanyId
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
            user = Session["User"] as SessionModel;
            Int64 RoleId = 0;
            int UserId = 0;
            if (!user.UserRole.ToLower().Contains("super admin"))
                UserId = user.UserId;
            Guid? RefKey=user.RefKey;
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