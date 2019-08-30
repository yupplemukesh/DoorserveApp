﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using doorserve.Models;
using Dapper;
using System.Data;
using System.Reflection;
using doorserve.Permission;
using System.Web;
using System.Threading.Tasks;
using doorserve.Repository.EmailSmsServices;
using doorserve.Filters;

namespace doorserve.Controllers
{
    public class UserController : Controller
    {
        private readonly string _connectionString =
          ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private readonly doorserve.Repository.EmailSmsTemplate.ITemplate _templateRepo;
        private readonly IEmailSmsServices _emailSmsServices;
        private readonly DropdownBindController _drp;
        public UserController()
        {

            _templateRepo = new doorserve.Repository.EmailSmsTemplate.Template();
            _emailSmsServices = new Repository.EmailsmsServices();
            _drp = new DropdownBindController();
        }
        [PermissionBasedAuthorize(new Actions[] {Actions.Create}, (int)MenuCode.Users)]
        public async Task<ActionResult> AddUser()
        {
            var session = Session["User"] as SessionModel;
            var user = new UserModel { RegionList = new SelectList(
                await CommonModel.GetRegionListByComp(session.CompanyId), "Name", "Text"),
                _AddressDetail = new AddressDetail {
                    LocationList = new SelectList(Enumerable.Empty<SelectList>())
                }
              
            };

            return View(user);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Users)]
        [ValidateModel]
        [HttpPost]

        public async Task<ActionResult> AddUser(UserModel objUser)
        {

            var session = Session["User"] as SessionModel;
            objUser.UserLoginId = session.UserId;            
            ResponseModel objResponseModel = new ResponseModel();
            var mpc = new Email_send_code();
            Type type = mpc.GetType();
                var Passwod = doorserve.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(objUser.Password, true);
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("UspInsertUser",
                        new
                        {
                            objUser.UserId,
                            objUser.UserName,
                            Passwod,
                            objUser._ContactPerson.ConFirstName,
                            objUser._ContactPerson.ConMobileNumber,
                            objUser.ConEmailAddress,
                            objUser._AddressDetail.AddressTypeId,
                            objUser._AddressDetail.Address,
                            objUser._AddressDetail.LocationId,
                            objUser._AddressDetail.PinNumber,
                            objUser.IsActive,
                            objUser.UserLoginId,
                            userTypeId= session.UserTypeId,
                            RefId= session.RefKey,
                            companyId= session.CompanyId,
                            objUser.RegionId
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 0)
                    {
                        objResponseModel.IsSuccess = false;
                        objResponseModel.ResponseCode = 0;
                        objResponseModel.Response= "Something went wrong";      
                    }
                    else if (result == 1)
                    {
                        
                        objResponseModel.IsSuccess = true;
                        objResponseModel.ResponseCode = 1;
                        objResponseModel.Response = "Successfully Added";      
                    var Templates = await _templateRepo.GetTemplateByActionName("User Registration", session.CompanyId);
                    session.Email = objUser.ConEmailAddress;
                    var WildCards = await CommonModel.GetWildCards();
                    var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                    U.Val = objUser._ContactPerson.ConFirstName;
                    U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                    U.Val =objUser.Password ;
                    U = WildCards.Where(x => x.Text.ToUpper() == "USER NAME").FirstOrDefault();
                    U.Val = objUser.UserName;
                    session.Mobile = objUser._ContactPerson.ConMobileNumber;
                    var c = WildCards.Where(x => x.Val != string.Empty).ToList();

                    if (Templates != null)
                        await _emailSmsServices.Send(Templates, c, session);

                }
                    else
                    {
                        //TempData["Message"] = "Successfully Updated";
                        objResponseModel.IsSuccess = true;
                        objResponseModel.ResponseCode = 2;
                        objResponseModel.Response = "Successfully Updated";
              
                    }
                    TempData["response"] = objResponseModel;
                    return RedirectToAction("UserList", "User");
                }
         
            
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Users)]
        public async Task<ActionResult> EditUser(Int64 id = 0)
        {
            var session = Session["User"] as SessionModel;
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
                    objUser._AddressDetail.PinNumber = result.PinNumber;
                    objUser._AddressDetail.Address = result.Address;
                    objUser._AddressDetail.AddressTypeId = result.AddressTypeId;               
                    objUser._AddressDetail.District = result.District;
                    objUser._AddressDetail.State = result.State;
                    objUser._ContactPerson.ConFirstName = result.ConFirstName;
                    objUser._ContactPerson.ConMobileNumber = result.ConMobileNumber;
                    objUser.CurrentPassword= doorserve.Encrypt_Decript_Code.encrypt_decrypt.Decrypt(result.Password, true);
                    objUser.RegionId = result.RegionId;
                    objUser._AddressDetail.LocationId= result.LocationId;
                    objUser.ConEmailAddress = result.ConEmailAddress;
                    objUser._AddressDetail.LocationList = new SelectList(_drp.BindLocationByPinCode(result.PinNumber), "Value", "Text");
                    objUser.RegionList = new SelectList(await CommonModel.GetRegionListByComp(session.CompanyId), "Name", "Text");
                    objUser.CurrentEmail = result.ConEmailAddress;
                    objUser.UserTypeId = result.UserTypeId;
                }
            }
            return View(objUser);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Users)]
        [HttpPost]
        [ValidateModel]
        public ActionResult EditUser(User objUser)
        {
            var session = Session["User"] as SessionModel;
            objUser.UserLoginId = session.UserId;
            ResponseModel objResponseModel = new ResponseModel();
            var mpc = new Email_send_code();
            Type type = mpc.GetType();
            var Status = 1;  
            if (Status == 1)
            {
               
                if(!string.IsNullOrEmpty( objUser.Password))
                    objUser.Password = doorserve.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(objUser.Password, true);
                else
                    objUser.Password = doorserve.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(objUser.CurrentPassword, true);
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
                                objUser.ConEmailAddress,
                                objUser._AddressDetail.LocationId,                           
                                objUser._AddressDetail.Address,
                                objUser._AddressDetail.AddressTypeId,
                                objUser._AddressDetail.PinNumber,
                                objUser.IsActive,
                                objUser.UserLoginId,
                                userTypeId = objUser.UserTypeId,
                                RefId = session.RefKey,
                                companyId = session.CompanyId,
                                objUser.RegionId


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

                        }
                        else
                        {
                            //TempData["Message"] = "Successfully Updated";
                            objResponseModel.IsSuccess = true;
                            objResponseModel.ResponseCode = 2;
                            objResponseModel.Response = "Successfully Updated";
                        }
                        TempData["response"] = objResponseModel;
                        return RedirectToAction("UserList", "User");
                    }
                
            }
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Users)]
        public ActionResult UserList()
        {
            var session = Session["User"] as SessionModel;
            int UserId = 0;
            Guid? RefKey = null;
            Guid? CompanyId = null;
            if (session.UserRole.ToLower().Contains("super admin"))
                UserId = session.UserId;
            else if (session.UserRole.ToLower().Contains("company admin"))
                CompanyId = session.CompanyId;
            else
                RefKey = session.RefKey;

            List<User> UserList = new List<User>();
            using (var con = new SqlConnection(_connectionString))
            {
                UserId = session.UserId;
                var result = con.Query<dynamic>("GETUSERLIST", new { UserId,RefKey, CompanyId },
                    commandType: CommandType.StoredProcedure).ToList();
                foreach(var item in result)
                {
                   var objUser = new User();
                    objUser._AddressDetail = new AddressDetail();
                    objUser._ContactPerson = new ContactPersonModel();
                    objUser.UserId = item.UserId;
                    objUser.UserName = item.UserName;
                    objUser.IsActive = item.IsActive;
                    objUser.CreatedDate = item.CreatedDate;
                    objUser.ModifyDate = item.ModifyDate;
                    objUser._AddressDetail.PinNumber = item.PinNumber;
                    objUser.LastUpdatedBy = item.LastUpdatedBy;
                    objUser._AddressDetail.District = item.District;
                    objUser._AddressDetail.State = item.State;
                    objUser.RoleName = item.RoleName;
                    objUser._ContactPerson.ConFirstName = item.ConFirstName;
                    objUser._ContactPerson.ConMobileNumber = item.ConMobileNumber;
                    objUser._ContactPerson.ConEmailAddress = item.ConEmailAddress;
                    UserList.Add(objUser);
                }
     


            }         
            return View(UserList);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Users)]
        public ActionResult ChangePassword()
        {
           return View();
           
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Users)]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel reset)
        {
            if (ModelState.IsValid)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var session = Session["User"] as SessionModel;
                    var encrpt_OldPass = doorserve.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(reset.CurrentPassword, true);
                    var encrpt_NewPass = doorserve.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(reset.NewPassword, true);
                    var response =  con.Query<ResponseModel>("ChangePassword_Proc", new { session.UserId, OldPassword = encrpt_OldPass, NewPassword = encrpt_NewPass },
                                            commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (response.ResponseCode == 0)
                    {
                        response.IsSuccess = true;
                        var Templates = await _templateRepo.GetTemplateByActionName("Change Password",session.CompanyId);
                        session.Email = User.Identity.Name;
                        var WildCards = await CommonModel.GetWildCards();
                        var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                        U.Val = session.UserName;
                         U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                        U.Val = reset.NewPassword;
                        var c = WildCards.Where(x => x.Val != string.Empty).ToList();

                        if (Templates != null)
                            await _emailSmsServices.Send(Templates, c, session);
                    }
                    else
                       response.IsSuccess = false;                  
                        TempData["response"] = response;
                }
            }
            return View(reset);
        }
       
        public ActionResult UserProfile()
        {
            var SessionModel = Session["User"] as SessionModel;
            User objUser = new User();
            int UserId = SessionModel.UserId;
            using (var con = new SqlConnection(_connectionString))
            {

                dynamic result = null;

                if (UserId != 0)
                {
                    result = con.Query<dynamic>("UspGetUserDetails", new { UserId },
                          commandType: CommandType.StoredProcedure).FirstOrDefault();

                    objUser._AddressDetail = new AddressDetail();
                    objUser._ContactPerson = new ContactPersonModel();
                    objUser._UserRole = new UserRole();
                    objUser._OrganizationModel = new OrganizationModel();
                    objUser._ClientModel = new ClientModel();
                }
                if (result != null)
                {
                    objUser.UserName = result.UserName;
                    objUser._AddressDetail.Address = result.Address;
                    objUser._ContactPerson.ConFirstName = result.ConFirstName;
                    objUser._ContactPerson.ConLastName = result.ConLastName;
                    objUser._ContactPerson.ConMobileNumber = result.ConMobileNumber;
                    objUser._ContactPerson.ConEmailAddress = result.ConEmailAddress;
                    objUser._UserRole.RoleName = result.RoleName;
                    objUser._OrganizationModel.OrgCode = result.OrgCode;
                    objUser._OrganizationModel.OrgName = result.OrgName;
                    objUser._ClientModel.ProcessName = result.ProcessName;
                    objUser.RefName = SessionModel.RefName;
                }
            }

            return View(objUser);

        }
    }


}