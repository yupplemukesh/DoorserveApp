using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using doorserve.Models;
using doorserve.Permission;

namespace doorserve.Controllers
{
    public class UserPermissionController : Controller
    {

        private readonly string _connectionString =
        ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_User_Permission)]
        public async Task<ActionResult> AddUserPermission(Int64 RoleId = 0, Int64 PermissionId = 0, Int64 UserId = 0)
        {
            var SessionModel = Session["User"] as SessionModel;
            UserPermission objUserPermission = new UserPermission();
            using (var con = new SqlConnection(_connectionString))
            {
                Guid? RefKey = null;
                Guid? CompanyId = null;

                if (SessionModel.UserRole.ToLower().Contains("super admin"))
                    UserId = 0;
                else if (SessionModel.UserRole.ToLower().Contains("company admin"))
                {
                    CompanyId = SessionModel.CompanyId;
                    RefKey = SessionModel.RefKey;
                }

                else
                    RefKey = SessionModel.RefKey;

                objUserPermission.UserList = con.Query<User>("GETUSERLIST", new { UserId = 0, RefKey,CompanyId },
                commandType: CommandType.StoredProcedure).ToList();

                objUserPermission.UserRoleList = con.Query<UserRole>("UspGetUserRoleDetail", new { RoleId = 0, UserId = 0, RefKey },
                commandType: CommandType.StoredProcedure).ToList();


                objUserPermission._MenuList = con.Query<MenuMasterModel>("UspGetMenuByPermissionRole",
                    new { RoleId, PermissionId }, commandType: CommandType.StoredProcedure).ToList();
                var Menues = objUserPermission._MenuList.Where(x => x.ParentMenuId == 0).ToList();
                foreach (var item in Menues)
                {
                    item.RightActionList = getActions("");
                    item.SubMenuList = objUserPermission._MenuList.Where(x => x.ParentMenuId == item.MenuCapId).Select(x => new MenuMasterModel { Menu_Name = x.Menu_Name, PagePath = x.PagePath, MenuCapId = x.MenuCapId, ParentMenuId = x.ParentMenuId, RightActionList = getActions(x.ActionIds) }).ToList();
                }
                objUserPermission._MenuList = Menues;
            }


            return View(objUserPermission);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_User_Permission)]
        [HttpPost]
        public ActionResult AddUserPermission(UserPermission permission, List<MenuMasterModel> objMenuMasterModel)
        {
         
            ResponseModel objResponseModel = new ResponseModel();
            var selectedMenu = objMenuMasterModel.Where(x => x.CheckedStatus == true).Select(x => new MenuMasterModel { MenuCapId = x.MenuCapId, Menu_Name = x.Menu_Name, ParentMenuId = x.ParentMenuId, SubMenuList = x.SubMenuList,CapName=x.CapName,  ActionIds = getActions(x.RightActionList) }).ToList();
            foreach (var item in selectedMenu)
            {
                if (item.SubMenuList != null)
                {
                    var menu = item.SubMenuList.Where(x => x.CheckedStatus = true).Select(x => new MenuMasterModel { MenuCapId = x.MenuCapId, ParentMenuId = x.ParentMenuId, Menu_Name = x.Menu_Name,CapName=x.CapName, ActionIds = getActions(x.RightActionList) }).ToList();
                    item.SubMenuList = menu;
                }

            }
            var xml = ToXML(selectedMenu);
            using (var con = new SqlConnection(_connectionString))
            {
                var SessionModel = Session["User"] as SessionModel;
                var result = con.Query<int>("UspInsertMenuActionRights",
                    new
                    {
                        permission.PermissionId,
                        permission.UserId,
                        permission.RoleId,
                        MenuActionRightXml = xml,
                        UserLoginId = SessionModel.UserId,
                        SessionModel.RefKey,
                        companyId = SessionModel.CompanyId
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 0)
                {
                    //   TempData["response"] = "Something went wrong";
                    objResponseModel.IsSuccess = false;
                    objResponseModel.ResponseCode = 0;
                    objResponseModel.Response = "Something went wrong";
                    TempData["response"] = objResponseModel;

                }
                else if (result == 1)
                {
                    //TempData["response"] = "Successfully Added";
                    objResponseModel.IsSuccess = true;
                    objResponseModel.ResponseCode = 1;
                    objResponseModel.Response = "Successfully Added";
                    TempData["response"] = objResponseModel;

                }
                else if (result == 2)
                {
                    // TempData["response"] = "User Name already exits on this role Please select another role";
                    objResponseModel.IsSuccess = true;
                    objResponseModel.ResponseCode = 2;
                    objResponseModel.Response = "User Name already exits on this role Please select another role";
                    TempData["response"] = objResponseModel;

                }
                return RedirectToAction("UserPermissionList", "UserPermission");
            }

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_User_Permission)]
        public async Task<ActionResult> EditUserPermission(Int64 RoleId = 0, Int64 PermissionId = 0, Int64 UserId = 0)
        {
            var SessionModel = Session["User"] as SessionModel;

            Guid? RefKey = SessionModel.RefKey;
            UserPermission objUserPermission;
            using (var con = new SqlConnection(_connectionString))
            {

                objUserPermission = con.Query<UserPermission>("GETUSERPERMISSIONBYID", new { PermissionId }, commandType: CommandType.StoredProcedure).SingleOrDefault();

  
                objUserPermission._MenuList = con.Query<MenuMasterModel>("UspGetMenuByPermissionRole",
                    new { RoleId, PermissionId }, commandType: CommandType.StoredProcedure).ToList();
                var Menues = objUserPermission._MenuList.Where(x => x.ParentMenuId == 0).ToList();
                foreach (var item in Menues)
                {
                    item.RightActionList = getActions(item.ActionIds);
                    item.SubMenuList = objUserPermission._MenuList.Where(x => x.ParentMenuId == item.MenuCapId).Select(x => new MenuMasterModel { Menu_Name = x.Menu_Name, PagePath = x.PagePath, MenuCapId = x.MenuCapId, ParentMenuId = x.ParentMenuId, CheckedStatus = x.CheckedStatus,CapName=x.CapName,  RightActionList = getActions(x.ActionIds) }).ToList();
                }
                objUserPermission._MenuList = Menues;
            }



            objUserPermission.UserId = UserId;
            objUserPermission.RoleId = RoleId;
            objUserPermission.PermissionId = PermissionId;

            return View(objUserPermission);
        }
        private List<CheckBox> getActions(string actionIds)
        {
            var actionsList = CommonModel.GetActionList();
            foreach (var item in actionsList)
            {
                if (!String.IsNullOrEmpty(actionIds.Trim()))
                {
                    if (actionIds.Contains(item.Value.ToString()))
                        item.IsChecked = true;
                    else
                        item.IsChecked = false;
                }

            }

            return actionsList;

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
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_User_Permission)]
        [HttpPost]
        public ActionResult EditUserPermission(UserPermission permission, List<MenuMasterModel> objMenuMasterModel)
        {
            var SessionModel = Session["User"] as SessionModel;
            var selectedMenu = objMenuMasterModel.Where(x => x.CheckedStatus == true).Select(x=>new MenuMasterModel {MenuCapId=x.MenuCapId,Menu_Name=x.Menu_Name,ParentMenuId=x.ParentMenuId, SubMenuList = x.SubMenuList,ActionIds = getActions(x.RightActionList) }).ToList();
            foreach (var item in selectedMenu)
            {
                if (item.SubMenuList!=null)
                {
                    var menu = item.SubMenuList.Where(x => x.CheckedStatus = true).Select(x => new MenuMasterModel { MenuCapId = x.MenuCapId, ParentMenuId = x.ParentMenuId, Menu_Name = x.Menu_Name, ActionIds = getActions(x.RightActionList) }).ToList();
                    item.SubMenuList = menu;
                }

            }
            var xml = ToXML(selectedMenu);
            ResponseModel objResponseModel = new ResponseModel();
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("UspInsertMenuActionRights",
                    new
                    {
                        permission.PermissionId,
                        permission.UserId,
                        permission.RoleId,
                        MenuActionRightXml = xml,
                        UserLoginId = SessionModel.UserId,
                        refKey = SessionModel.RefKey,
                                 companyId = SessionModel.CompanyId
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 0)
                {
                    //   TempData["response"] = "Something went wrong";
                    objResponseModel.IsSuccess = false;
                    objResponseModel.ResponseCode = 0;
                    objResponseModel.Response = "Something went wrong";
                    TempData["response"] = objResponseModel;

                }
                else if (result == 1)
                {
                    //TempData["response"] = "Successfully Added";
                    objResponseModel.IsSuccess = true;
                    objResponseModel.ResponseCode = 1;
                    objResponseModel.Response = "Successfully Added";
                    TempData["response"] = objResponseModel;

                }
                else if (result == 2)
                {
                    // TempData["response"] = "User Name already exits on this role Please select another role";
                    objResponseModel.IsSuccess = true;
                    objResponseModel.ResponseCode = 2;
                    objResponseModel.Response = "User Name already exits on this role Please select another role";
                    TempData["response"] = objResponseModel;

                }
                return RedirectToAction("UserPermissionList", "UserPermission");
            }

        }

        public string getActions(List<CheckBox> Actions)
        {
            string actions = "";
            foreach (var item in Actions)
            {
                if (item.IsChecked)
                    actions = actions + ","+ item.Value ;

            }
            actions = actions.Trim(',');
            return actions;
        }
        public async Task<ActionResult> BindMenu(Int64 RoleId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                Int64 PermissionId = 0;
                var result = con.Query<MenuMasterModel>("UspGetMenuByPermissionRole",
                new { RoleId, PermissionId }, commandType: CommandType.StoredProcedure).ToList();

                var Menues = result.Where(x => x.ParentMenuId == 0).ToList();
                foreach (var item in Menues)
                {
                    
                        item.RightActionList = getActions("");
                        item.SubMenuList = result.Where(x => x.ParentMenuId == item.MenuCapId).Select(x => new MenuMasterModel { Menu_Name = x.Menu_Name, PagePath = x.PagePath, MenuCapId = x.MenuCapId, ParentMenuId = x.ParentMenuId, CapName=x.CapName, RightActionList = getActions(x.ActionIds) }).ToList();
               

                }
                result = Menues;
                     
             
                return PartialView("_Permission",result);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_User_Permission)]
        public ActionResult UserPermissionList()
        {
            var SessionModel = Session["User"] as SessionModel;
            int UserId = SessionModel.UserId;
            Guid? refKey = null;
            Guid? companyId = null;
            if (SessionModel.UserTypeName.ToLower().Contains("super admin"))
                UserId = 0;
            else if(SessionModel.UserTypeName.ToLower().Contains("company"))
                companyId = SessionModel.CompanyId;
            else
                refKey = SessionModel.RefKey;
            var objUserPermission = new List<UserPermission>();
            using (var con = new SqlConnection(_connectionString))
            {
                objUserPermission = con.Query<UserPermission>("UspGetActionPermissionDetail", new {UserId, refKey,companyId},
                    commandType: CommandType.StoredProcedure).ToList();
           }
           
        

            return View(objUserPermission);
        }
    }
}