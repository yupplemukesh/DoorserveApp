using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using TogoFogo.Models;
using Dapper;
using System.Data;
using System.Reflection;
using TogoFogo.Permission;
using System.Web;

namespace TogoFogo.Controllers
{
    public class UserController : Controller
    {
        private readonly string _connectionString =
        ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;       
        [PermissionBasedAuthorize(new Actions[] {Actions.Create}, "Manage Users")]
        public ActionResult AddUser()
        {
       
            return View();
        }
        [HttpPost]      
        public ActionResult AddUser(User objUser)
        {
            objUser.UserLoginId = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));            
            ResponseModel objResponseModel = new ResponseModel();
            var mpc = new Email_send_code();
            Type type = mpc.GetType();
            var Status = (int)type.InvokeMember("sendmail_update",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, mpc,
                                    new object[] { objUser._ContactPerson.ConEmailAddress, objUser.Password, objUser.UserName });
            if (Status == 1)
            {
                objUser.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(objUser.Password, true);
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("UspInsertUser",
                        new
                        {
                            objUser.UserId,
                            objUser.UserName,
                            objUser.Password,
                            objUser._ContactPerson.ConFirstName,
                            objUser._ContactPerson.ConMobileNumber,
                            objUser._ContactPerson.ConEmailAddress,
                            objUser._AddressDetail.AddressTypeId,
                            objUser._AddressDetail.Address,
                            objUser._AddressDetail.CityId,
                            objUser._AddressDetail.StateId,
                            objUser._AddressDetail.PinNumber,
                            objUser.IsActive,
                            objUser.UserLoginId

                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 0)
                    {
                        objResponseModel.IsSuccess = false;
                        objResponseModel.ResponseCode = 0;
                        objResponseModel.Response= "Something went wrong";
                        TempData["response"] = objResponseModel;
                    }
                    else if (result == 1)
                    {
                        
                        objResponseModel.IsSuccess = true;
                        objResponseModel.ResponseCode = 1;
                        objResponseModel.Response = "Successfully Added";
                        TempData["response"] = objResponseModel;
                    }
                    else
                    {
                        //TempData["Message"] = "Successfully Updated";
                        objResponseModel.IsSuccess = true;
                        objResponseModel.ResponseCode = 2;
                        objResponseModel.Response = "Successfully Updated";
                        TempData["response"] = objResponseModel;
                    }

                    return RedirectToAction("UserList", "User");
                }
            }
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Users")]
        public ActionResult EditUser(Int64 id = 0)
        {
            User objUser = new User();
            Int64 UserId = id;
            using (var con = new SqlConnection(_connectionString))
            {
                dynamic result = null;
                if (UserId != 0)
                {
                    result = con.Query<dynamic>("UspGetUserDetails", new { UserId },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    objUser._AddressDetail = new AddressDetail();
                    objUser._ContactPerson = new ContactPersonModel();
                }
                if (result != null)
                {
                    
                    objUser.UserId = result.UserId;
                    objUser.UserName = result.UserName;
                    objUser.IsActive = result.IsActive;
                    objUser.CreatedDate = result.CreatedDate;
                    objUser.ModifyDate = result.ModifyDate;
                    objUser.Password = result.Password;
                    objUser._AddressDetail.PinNumber = result.PinNumber;
                    objUser._AddressDetail.Address = result.Address;
                    objUser._AddressDetail.AddressTypeId = result.AddressTypeId;
                    objUser._AddressDetail.CityId = result.CityId;
                    objUser._AddressDetail.StateId = result.StateId;
                    objUser._AddressDetail.City = result.City;
                    objUser._AddressDetail.State = result.State;
                    objUser._ContactPerson.ConFirstName = result.ConFirstName;
                    objUser._ContactPerson.ConMobileNumber = result.ConMobileNumber;
                    objUser._ContactPerson.ConEmailAddress = result.ConEmailAddress;
                }
            }
            return View(objUser);
        }
        [HttpPost]
        public ActionResult EditUser(User objUser)
        {
            objUser.UserLoginId = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));
            ResponseModel objResponseModel = new ResponseModel();
            var mpc = new Email_send_code();
            Type type = mpc.GetType();
            var Status = (int)type.InvokeMember("sendmail_update",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, mpc,
                                    new object[] { objUser._ContactPerson.ConEmailAddress, objUser.Password, objUser.UserName });
            if (Status == 1)
            {
                objUser.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(objUser.Password, true);
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("UspInsertUser",
                        new
                        {
                            objUser.UserId,
                            objUser.UserName,
                            objUser.Password,
                            objUser._ContactPerson.ConFirstName,
                            objUser._ContactPerson.ConMobileNumber,
                            objUser._ContactPerson.ConEmailAddress,
                            objUser._AddressDetail.AddressTypeId,
                            objUser._AddressDetail.Address,
                            objUser._AddressDetail.CityId,
                            objUser._AddressDetail.StateId,
                            objUser._AddressDetail.PinNumber,
                            objUser.IsActive,
                            objUser.UserLoginId

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
                        //TempData["Message"] = "Successfully Added";
                        objResponseModel.IsSuccess = true;
                        objResponseModel.ResponseCode = 1;
                        objResponseModel.Response = "Successfully Added";
                        TempData["response"] = objResponseModel;
                    }
                    else
                    {
                        //TempData["Message"] = "Successfully Updated";
                        objResponseModel.IsSuccess = true;
                        objResponseModel.ResponseCode = 2;
                        objResponseModel.Response = "Successfully Updated";
                        TempData["response"] = objResponseModel;
                    }

                    return RedirectToAction("UserList", "User");
                }
            }
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Users")]
        public ActionResult UserList()
        {
            int UserId = 0;            
            User objUser = new User();
            List<User> UserList = new List<User>();
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<dynamic>("UspGetUserDetails", new { UserId },
                    commandType: CommandType.StoredProcedure).ToList();
                foreach(var item in result)
                {
                    objUser = new User();
                    objUser._AddressDetail = new AddressDetail();
                    objUser._ContactPerson = new ContactPersonModel();
                    objUser.SerialNo = item.SerialNo;
                    objUser.UserId = item.UserId;
                    objUser.UserName = item.UserName;
                    objUser.IsActive = item.IsActive;
                    objUser.CreatedDate = item.CreatedDate;
                    objUser.ModifyDate = item.ModifyDate;
                    objUser._AddressDetail.PinNumber = item.PinNumber;
                    objUser._AddressDetail.Address = item.Address;
                    objUser._AddressDetail.AddressTypeId = item.AddressTypeId;
                    objUser._AddressDetail.CityId = item.CityId;
                    objUser._AddressDetail.StateId = item.StateId;
                    objUser._AddressDetail.City = item.City;
                    objUser._AddressDetail.State = item.State;
                    objUser._ContactPerson.ConFirstName = item.ConFirstName;
                    objUser._ContactPerson.ConMobileNumber = item.ConMobileNumber;
                    objUser._ContactPerson.ConEmailAddress = item.ConEmailAddress;
                    UserList.Add(objUser);
                }
                objUser.objUserList = UserList;


            }

            UserActionRights objUserActiobRight = new UserActionRights();
            objUser._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objUser._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objUser._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objUser._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objUser._UserActionRights.Delete = true;
                    }
                }
            }
            else
            {

                objUser._UserActionRights.Create = true;
                objUser._UserActionRights.Edit = true;
                objUser._UserActionRights.Delete = true;
                objUser._UserActionRights.View = true;
                objUser._UserActionRights.History = true;
                objUser._UserActionRights.ExcelExport = true;

            }
            return View(objUser);
        }
    }
}