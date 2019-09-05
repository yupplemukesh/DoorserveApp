using System;
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
    public class UserController :BaseController
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


            var user = new UserModel { RegionList = new SelectList(
                await CommonModel.GetRegionListByComp(CurrentUser.CompanyId), "Name", "Text"),                 
                    LocationList = new SelectList(Enumerable.Empty<SelectList>())

        };
            user.EventAction = 'I';
            return View(user);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Users)]
        [HttpPost]

        public async Task<ActionResult> AddUser(UserModel objUser)
        {

            if (!ModelState.IsValid)

            {
                objUser.RegionList = new SelectList(
                await CommonModel.GetRegionListByComp(CurrentUser.CompanyId), "Name", "Text");
                objUser.LocationList = new SelectList( _drp.BindLocationByPinCode(objUser.PinNumber),"Value","Text");
                return View(objUser);

            }

            objUser.UserLoginId = CurrentUser.UserId;            
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
                            Password=Passwod,
                            objUser.ConFirstName,
                            objUser.ConMobileNumber,
                            objUser.ConEmailAddress,
                            objUser.LocationId,
                            objUser.Address,
                            objUser.AddressTypeId,
                            objUser.PinNumber,
                            objUser.IsActive,
                            objUser.UserLoginId,
                            userTypeId = objUser.UserTypeId,
                            RefId = CurrentUser.RefKey,
                            companyId = CurrentUser.CompanyId,
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
                    var Templates = await _templateRepo.GetTemplateByActionId(12, CurrentUser.CompanyId);
                    CurrentUser.Email = objUser.ConEmailAddress;
                    var WildCards =  CommonModel.GetWildCards(CurrentUser.CompanyId);
                    var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                    U.Val = objUser.ConFirstName;
                    U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                    U.Val =objUser.Password ;
                    U = WildCards.Where(x => x.Text.ToUpper() == "USER NAME").FirstOrDefault();
                    U.Val = objUser.UserName;
                    CurrentUser.Mobile = objUser.ConMobileNumber;
                    var c = WildCards.Where(x => x.Val != string.Empty).ToList();
                    if (Templates.Count>0)
                        await _emailSmsServices.Send(Templates, c, CurrentUser);

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

            User objUser = new User();
            Int64 UserId = id;
            using (var con = new SqlConnection(_connectionString))
            {
                dynamic result = null;
                if (UserId != 0)
                {
                    result = con.Query<dynamic>("UspGetUserDetails", new { UserId },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();                   
                }
                if (result != null)
                {
                    
                    objUser.UserId = result.UserId;
                    objUser.UserName = result.UserName;
                    objUser.IsActive = result.IsActive;                    
                    objUser.PinNumber = result.PinNumber;
                    objUser.Address = result.Address;
                    objUser.AddressTypeId = result.AddressTypeId;               
                    objUser.District = result.District;
                    objUser.State = result.State;
                    objUser.ConFirstName = result.ConFirstName;
                    objUser.ConMobileNumber = result.ConMobileNumber;
                    objUser.CurrentPassword= doorserve.Encrypt_Decript_Code.encrypt_decrypt.Decrypt(result.Password, true);
                    objUser.RegionId = result.RegionId;
                    objUser.LocationId= result.LocationId;
                    objUser.ConEmailAddress = result.ConEmailAddress;
                    objUser.LocationList = new SelectList(_drp.BindLocationByPinCode(result.PinNumber), "Value", "Text");
                    objUser.RegionList = new SelectList(await CommonModel.GetRegionListByComp(CurrentUser.CompanyId), "Name", "Text");
                    objUser.CurrentEmail = result.ConEmailAddress;
                    objUser.UserTypeId = result.UserTypeId;
                }
            }
            objUser.EventAction = 'U';
            return View(objUser);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Users)]
        [HttpPost]

        public async Task<ActionResult> EditUser(User objUser)
        {
           
            if (!ModelState.IsValid)
            {
                objUser.RegionList = new SelectList(
                await CommonModel.GetRegionListByComp(CurrentUser.CompanyId), "Name", "Text");
                objUser.LocationList = new SelectList(_drp.BindLocationByPinCode(objUser.PinNumber), "Value", "Text");
                return View(objUser);

            }
            objUser.UserLoginId = CurrentUser.UserId;
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
                                objUser.ConFirstName,
                                objUser.ConMobileNumber,
                                objUser.ConEmailAddress,
                                objUser.LocationId,                           
                                objUser.Address,
                                objUser.AddressTypeId,
                                objUser.PinNumber,
                                objUser.IsActive,
                                objUser.UserLoginId,
                                userTypeId = objUser.UserTypeId,
                                RefId = CurrentUser.RefKey,
                                companyId = CurrentUser.CompanyId,
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
          
            int UserId = 0;
            Guid? RefKey = null;
            Guid? CompanyId = null;
            if (CurrentUser.UserRole.ToLower().Contains("super admin"))
                UserId = CurrentUser.UserId;
            else if (CurrentUser.UserRole.ToLower().Contains("company admin"))
                CompanyId = CurrentUser.CompanyId;
            else
                RefKey = CurrentUser.RefKey;

            List<User> UserList = new List<User>();
            using (var con = new SqlConnection(_connectionString))
            {
                UserId = CurrentUser.UserId;
                var result = con.Query<dynamic>("GETUSERLIST", new { UserId,RefKey, CompanyId },
                    commandType: CommandType.StoredProcedure).ToList();
                foreach(var item in result)
                {
                   var objUser = new User();               
                    objUser.UserId = item.UserId;
                    objUser.UserName = item.UserName;
                    objUser.IsActive = item.IsActive;
                    objUser.AddedOn = item.CreatedDate;
                    objUser.ModifiedOn = item.ModifyDate;
                    objUser.PinNumber = item.PinNumber;
                    objUser.ModifiedBy = item.LastUpdatedBy;
                    objUser.District = item.District;
                    objUser.LocationName = item.LocationName;
                    objUser.State = item.State;
                    objUser.RoleName = item.RoleName;
                    objUser.ConFirstName = item.ConFirstName;
                    objUser.ConMobileNumber = item.ConMobileNumber;
                    objUser.ConEmailAddress = item.ConEmailAddress;
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

                    var encrpt_OldPass = doorserve.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(reset.CurrentPassword, true);
                    var encrpt_NewPass = doorserve.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(reset.NewPassword, true);
                    var response =  con.Query<ResponseModel>("ChangePassword_Proc", new { CurrentUser.UserId, OldPassword = encrpt_OldPass, NewPassword = encrpt_NewPass },
                                            commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (response.ResponseCode == 0)
                    {
                        response.IsSuccess = true;
                        var Templates = await _templateRepo.GetTemplateByActionId(7,CurrentUser.CompanyId);
                        CurrentUser.Email = User.Identity.Name;
                        var WildCards =  CommonModel.GetWildCards(CurrentUser.CompanyId);
                        var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                        U.Val = CurrentUser.UserName;
                         U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                        U.Val = reset.NewPassword;
                        var c = WildCards.Where(x => x.Val != string.Empty).ToList();

                        if (Templates != null)
                            await _emailSmsServices.Send(Templates, c, CurrentUser);
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
           
            User objUser = new User();
            int UserId = CurrentUser.UserId;
            using (var con = new SqlConnection(_connectionString))
            {

                dynamic result = null;

                if (UserId != 0)
                {
                    result = con.Query<dynamic>("UspGetUserDetails", new { UserId },
                          commandType: CommandType.StoredProcedure).FirstOrDefault();

               
                    objUser._UserRole = new UserRole();
                    objUser._OrganizationModel = new OrganizationModel();

                }
                if (result != null)
                {
                    objUser.UserName = result.UserName;
                    objUser.Address = result.Address;
                    objUser.ConFirstName = result.ConFirstName;
                    objUser.ConLastName = result.ConLastName;
                    objUser.ConMobileNumber = result.ConMobileNumber;
                    objUser.ConEmailAddress = result.ConEmailAddress;
                    objUser._UserRole.RoleName = result.RoleName;
                    objUser._OrganizationModel.OrgCode = result.OrgCode;
                    objUser._OrganizationModel.OrgName = result.OrgName;
                    objUser.RefName = CurrentUser.RefName;
                }
            }

            return View(objUser);

        }
    }


}