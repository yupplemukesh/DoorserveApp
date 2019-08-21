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
using System.Threading.Tasks;
using TogoFogo.Repository.EmailSmsServices;

namespace TogoFogo.Controllers
{
    public class UserController : Controller
    {
        private readonly string _connectionString =
          ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private readonly TogoFogo.Repository.EmailSmsTemplate.ITemplate _templateRepo;
        private readonly IEmailSmsServices _emailSmsServices;
   public UserController()
        {

            _templateRepo = new TogoFogo.Repository.EmailSmsTemplate.Template();
            _emailSmsServices = new Repository.EmailsmsServices();
        }
        [PermissionBasedAuthorize(new Actions[] {Actions.Create}, (int)MenuCode.Users)]
        public ActionResult AddUser()
        {
       
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Users)]
        [HttpPost]      
        public async Task<ActionResult> AddUser(User objUser)
        {

            var session = Session["User"] as SessionModel;
            objUser.UserLoginId = session.UserId;            
            ResponseModel objResponseModel = new ResponseModel();
            var mpc = new Email_send_code();
            Type type = mpc.GetType();
            var pwd = "CA5680";


                objUser.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(pwd, true);
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
                            objUser._AddressDetail.AddressTypeId,
                            objUser._AddressDetail.Address,
                            objUser._AddressDetail.District,
                            objUser._AddressDetail.StateId,
                            objUser._AddressDetail.PinNumber,
                            objUser.IsActive,
                            objUser.UserLoginId,
                            userTypeId= session.UserTypeId,
                            RefId= session.RefKey,
                            companyId= session.CompanyId
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
      
                    var Templates = await _templateRepo.GetTemplateByActionName("User Registration");
                    session.Email = objUser.ConEmailAddress;
                    var WildCards = await CommonModel.GetWildCards();
                    var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                    U.Val = objUser._ContactPerson.ConFirstName;
                    U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                    U.Val = pwd;
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
                    objUser.Password = result.Password;
                    objUser._AddressDetail.PinNumber = result.PinNumber;
                    objUser._AddressDetail.Address = result.Address;
                    objUser._AddressDetail.AddressTypeId = result.AddressTypeId;
                    //objUser._AddressDetail.CityId = result.CityId;
                    objUser._AddressDetail.StateId = result.StateId;
                    objUser._AddressDetail.District = result.District;
                    objUser._AddressDetail.State = result.State;
                    objUser._ContactPerson.ConFirstName = result.ConFirstName;
                    objUser._ContactPerson.ConMobileNumber = result.ConMobileNumber;
                    //objUser._ContactPerson.ConEmailAddress = result.ConEmailAddress;
                    objUser.ConEmailAddress = result.ConEmailAddress;
                    objUser.CurrentEmail = result.ConEmailAddress;
                    //objUser._ContactPerson.CurrentEmail = result.ConEmailAddress;
                    objUser.UserTypeId = result.UserTypeId;
                }
            }
            return View(objUser);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Users)]
        [HttpPost]
        public ActionResult EditUser(User objUser)
        {
            var session = Session["User"] as SessionModel;
            objUser.UserLoginId = session.UserId;
            ResponseModel objResponseModel = new ResponseModel();
            var mpc = new Email_send_code();
            Type type = mpc.GetType();
            var Status = 1;
            //var Status = (int)type.InvokeMember("sendmail_update",
            //                        BindingFlags.Instance | BindingFlags.InvokeMethod |
            //                        BindingFlags.NonPublic, null, mpc,
            //                        new object[] { objUser._ContactPerson.ConEmailAddress, objUser.Password, objUser.UserName });
            if (Status == 1)
            {
               
                    //objUser.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(objUser.Password, true);
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
                                objUser._AddressDetail.StateId,
                                objUser._AddressDetail.District,
                                objUser._AddressDetail.Address,
                                objUser._AddressDetail.AddressTypeId,
                                objUser._AddressDetail.PinNumber,
                                objUser.IsActive,
                                objUser.UserLoginId,
                                userTypeId = objUser.UserTypeId,
                                RefId = session.RefKey,
                                companyId = session.CompanyId

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
                    var encrpt_OldPass = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(reset.CurrentPassword, true);
                    var encrpt_NewPass = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(reset.NewPassword, true);
                    var response =  con.Query<ResponseModel>("ChangePassword_Proc", new { session.UserId, OldPassword = encrpt_OldPass, NewPassword = encrpt_NewPass },
                                            commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (response.ResponseCode == 0)
                    {
                        response.IsSuccess = true;
                        var Templates = await _templateRepo.GetTemplateByActionName("Change Password");
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