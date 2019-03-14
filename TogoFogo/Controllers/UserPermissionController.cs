using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using TogoFogo.Models;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class UserPermissionController : Controller
    {
        private readonly string _connectionString =
        ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage User Permission")]
        public async Task<ActionResult> AddUserPermission(Int64 RoleId = 0,Int64 PermissionId=0,Int64 UserId=0)
        {
            UserPermission objUserPermission = new UserPermission();               
            using (var con = new SqlConnection(_connectionString))
            {
                objUserPermission.UserList = con.Query<User>("GETUSERLIST",
                commandType: CommandType.StoredProcedure).ToList();
           
                objUserPermission.UserRoleList = con.Query<UserRole>("UspGetUserRoleDetail", new { RoleId=0 },
                commandType: CommandType.StoredProcedure).ToList();    
             
             
                objUserPermission._MenuList = con.Query<MenuMasterModel>("UspGetMenuByPermissionRole",
                    new { RoleId, PermissionId }, commandType: CommandType.StoredProcedure).ToList();
                
                }
            var actionsList = await CommonModel.GetActionList();
            foreach (var item in objUserPermission._MenuList)
            {
                var actions = new List<CheckBox>();
                foreach (var tom in actionsList)
                {
                    var action = new CheckBox
                    {
                        Value = tom.Value,
                        IsChecked = false
                    };
                    if (!String.IsNullOrEmpty(item.ActionIds.Trim()))
                    {
                        if (item.ActionIds.Contains(tom.Value.ToString()))
                            action.IsChecked = true;
                    }
                    actions.Add(action);
                }
                item.RightActionList = actions;

            }
            return View(objUserPermission);
        }
        [HttpPost]
        public ActionResult AddUserPermission(UserPermission permission,List<MenuMasterModel> objMenuMasterModel)
        {
            XmlDocument doc = new XmlDocument();
            ResponseModel objResponseModel = new ResponseModel();
            //(1) the xml declaration is recommended, but not mandatory
            //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
           // doc.InsertBefore(xmlDeclaration, root);
            //(2) string.Empty makes cleaner code
            XmlElement element1 = doc.CreateElement(string.Empty, "MenuXML", string.Empty);
            doc.AppendChild(element1);         
           // doc.Save("D:\\document.xml");
            List<MenuMasterModel> objSelectedMenuList = objMenuMasterModel.Where(x => x.CheckedStatus == true).ToList();
            foreach(var item in objSelectedMenuList)
            {
                List<CheckBox> SelectedActionList = item.RightActionList.Where(x => x.IsChecked == true).ToList();
                XmlElement element2 = doc.CreateElement(string.Empty, "item", string.Empty);
                XmlElement element3 = doc.CreateElement(string.Empty, "MenuCap_ID", string.Empty);
                XmlText MenuId = doc.CreateTextNode(item.MenuCap_ID.ToString());
                element3.AppendChild(MenuId);
                element2.AppendChild(element3);
                XmlElement element4 = doc.CreateElement(string.Empty, "ActionRight_Id", string.Empty);
                XmlText ActionRightId = doc.CreateTextNode(item.ActionRightId.ToString());
                element4.AppendChild(ActionRightId);
                element2.AppendChild(element4);
                
                XmlElement element5 = doc.CreateElement(string.Empty, "Action", string.Empty);
                XmlElement element6 = doc.CreateElement(string.Empty, "item", string.Empty);
                foreach (var ActionItem in SelectedActionList)
                {
                    XmlElement element7 = doc.CreateElement(string.Empty, "ActionId", string.Empty);
                    XmlText ActionId = doc.CreateTextNode(ActionItem.Value.ToString());
                    element7.AppendChild(ActionId);
                    element6.AppendChild(element7);

                }
                element5.AppendChild(element6);
                element2.AppendChild(element5);
                element1.AppendChild(element2);
            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("UspInsertMenuActionRights",
                    new
                    {
                        permission.PermissionId,
                        permission.UserId,
                        permission.RoleId,
                        MenuActionRightXml = doc.InnerXml,
                        permission.UserLoginId
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage User Permission")]
        public async Task<ActionResult> EditUserPermission(Int64 RoleId = 0, Int64 PermissionId = 0, Int64 UserId = 0)
        {
            UserPermission objUserPermission = new UserPermission();
            using (var con = new SqlConnection(_connectionString))
            {
                objUserPermission.UserList = con.Query<User>("GETUSERLIST",
                commandType: CommandType.StoredProcedure).ToList();

                objUserPermission.UserRoleList = con.Query<UserRole>("UspGetUserRoleDetail", new { RoleId = 0 },
                commandType: CommandType.StoredProcedure).ToList();


                objUserPermission._MenuList = con.Query<MenuMasterModel>("UspGetMenuByPermissionRole",
                    new { RoleId, PermissionId }, commandType: CommandType.StoredProcedure).ToList();

            }
            var actionsList = await CommonModel.GetActionList();
            foreach (var item in objUserPermission._MenuList)
            {
                var actions = new List<CheckBox>();
                foreach (var tom in actionsList)
                {
                    var action = new CheckBox
                    {
                        Value = tom.Value,
                        IsChecked = false
                    };
                    if (!String.IsNullOrEmpty(item.ActionIds.Trim()))
                    {
                        if (item.ActionIds.Contains(tom.Value.ToString()))
                            action.IsChecked = true;
                        else
                            action.IsChecked = false;
                    }
                    actions.Add(action);
                }
                item.RightActionList = actions;

            }
            return View(objUserPermission);
        }
        [HttpPost]
        public ActionResult EditUserPermission(UserPermission permission, List<MenuMasterModel> objMenuMasterModel)
        {
            XmlDocument doc = new XmlDocument();
            ResponseModel objResponseModel = new ResponseModel();
            //(1) the xml declaration is recommended, but not mandatory
            //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            // doc.InsertBefore(xmlDeclaration, root);
            //(2) string.Empty makes cleaner code
            XmlElement element1 = doc.CreateElement(string.Empty, "MenuXML", string.Empty);
            doc.AppendChild(element1);
            // doc.Save("D:\\document.xml");
            List<MenuMasterModel> objSelectedMenuList = objMenuMasterModel.Where(x => x.CheckedStatus == true).ToList();
            foreach (var item in objSelectedMenuList)
            {
                List<CheckBox> SelectedActionList = item.RightActionList.Where(x => x.IsChecked == true).ToList();
                XmlElement element2 = doc.CreateElement(string.Empty, "item", string.Empty);
                XmlElement element3 = doc.CreateElement(string.Empty, "MenuCap_ID", string.Empty);
                XmlText MenuId = doc.CreateTextNode(item.MenuCap_ID.ToString());
                element3.AppendChild(MenuId);
                element2.AppendChild(element3);
                XmlElement element4 = doc.CreateElement(string.Empty, "ActionRight_Id", string.Empty);
                XmlText ActionRightId = doc.CreateTextNode(item.ActionRightId.ToString());
                element4.AppendChild(ActionRightId);
                element2.AppendChild(element4);

                XmlElement element5 = doc.CreateElement(string.Empty, "Action", string.Empty);
                XmlElement element6 = doc.CreateElement(string.Empty, "item", string.Empty);
                foreach (var ActionItem in SelectedActionList)
                {
                    XmlElement element7 = doc.CreateElement(string.Empty, "ActionId", string.Empty);
                    XmlText ActionId = doc.CreateTextNode(ActionItem.Value.ToString());
                    element7.AppendChild(ActionId);
                    element6.AppendChild(element7);

                }
                element5.AppendChild(element6);
                element2.AppendChild(element5);
                element1.AppendChild(element2);
            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("UspInsertMenuActionRights",
                    new
                    {
                        permission.PermissionId,
                        permission.UserId,
                        permission.RoleId,
                        MenuActionRightXml = doc.InnerXml,
                        permission.UserLoginId
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
        public async Task<ActionResult> BindMenu(Int64 RoleId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                Int64 PermissionId = 0;
                var result = con.Query<MenuMasterModel>("UspGetMenuByPermissionRole",
                new { RoleId, PermissionId }, commandType: CommandType.StoredProcedure).ToList();
                var actionsList = await CommonModel.GetActionList();                
                foreach (var item in result)
                {
                    var actions = new List<CheckBox>();
                    foreach (var tom in actionsList)
                    {
                        var action = new CheckBox { Value = tom.Value,
                            IsChecked = false
                    };
                        if (!String.IsNullOrEmpty(item.ActionIds.Trim()))
                        {
                            if (item.ActionIds.Contains(tom.Value.ToString()))
                                action.IsChecked = true;
                        }
                            actions.Add(action);
                    }
                   item.RightActionList = actions;

                }
                return PartialView("_Permission",result);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage User Permission")]
        public ActionResult UserPermissionList()
        {
            var objUserPermission = new List<UserPermission>();
            using (var con = new SqlConnection(_connectionString))
            {
                objUserPermission = con.Query<UserPermission>("UspGetActionPermissionDetail", new {  },
                    commandType: CommandType.StoredProcedure).ToList();
           }
           
        

            return View(objUserPermission);
        }
    }
}