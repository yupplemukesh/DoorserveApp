﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Security;
using Dapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TogoFogo.Models;
using TogoFogo.Permission;
using TogoFogo.Repository.EmailSmsServices;

namespace TogoFogo.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly string _connectionString =
              ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext context;
        private readonly TogoFogo.Repository.EmailSmsTemplate.ITemplate _templateRepo;
        private readonly IEmailSmsServices _emailSmsServices;
        public AccountController()
        {
            context = new ApplicationDbContext();
            _templateRepo = new TogoFogo.Repository.EmailSmsTemplate.Template();
            _emailSmsServices = new Repository.EmailsmsServices();

        }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            //ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
       // POST: /Account/Login
       [HttpPost]
       [AllowAnonymous]
       [CustomHandleError]
        public async Task<ActionResult> Login(LoginViewModel m)
        {
            if (string.IsNullOrEmpty(m.Email) || string.IsNullOrEmpty(m.Password))
            {
                ViewBag.Message = "Please provide email and password";
                return View(m);
            }
            var encrpt_Pass = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(m.Password, true);
            using (var con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                string iconPath = "/uploadedImages/icon-img/";
                var result = await con.QueryMultipleAsync("Login_Proc", new { Username = m.Email, Password = encrpt_Pass },
                    commandType: CommandType.StoredProcedure);
                var rs = await result.ReadSingleOrDefaultAsync<dynamic>();
                if (rs !=null)                {                   
                    var PerentMenues = await result.ReadAsync<MenuMasterModel>() as List<MenuMasterModel>;
                    PerentMenues = PerentMenues.Select(x => new MenuMasterModel { MenuCapId = x.MenuCapId, IsActive = x.IsActive, Menu_Name = x.Menu_Name, CapName = x.CapName, PagePath = x.PagePath, IconFileNameUl = iconPath + x.IconFileName,ParentMenuId=x.ParentMenuId,ParentMenuName=x.ParentMenuName }).ToList();
                    var SubMenues =    await result.ReadAsync<MenuMasterModel>() as List<MenuMasterModel>;
                    var manues = new MenuMasterModel {ParentMenuList= PerentMenues,SubMenuList=SubMenues };

                    SessionModel session = new SessionModel {UserId=rs.UserId,
                        Email=rs.Email,RefKey=rs.RefKey, CompanyId=rs.CompanyId,
                        UserTypeName=rs.UserTypeName,
                        UserRole = rs.RoleName,
                        UserName=rs.UserName,
                        Mobile= rs.Mobile,
                       Menues= manues
                    };

                    if (!session.UserTypeName.ToLower().Contains("super admin"))
                        session.LogoUrl = "/uploadedImages/Companies/Logo/" + rs.CompLogo;
                 

                    Session["User"] = session;                  
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, m.Email));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, m.Email));
                    claims.Add(new Claim(ClaimTypes.Email, rs.Email));
                    var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                    var ctx = Request.GetOwinContext();
                    var authenticationManager = ctx.Authentication;
                    authenticationManager.SignIn(id);
                    return RedirectToAction("Index", "Home");
                }
                else
                {   
                    ViewBag.Message = "User Name or Password is not correct";
                }         
            }
            return View();
            }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
           // FormsAuthentication.SignOut();
            foreach (var cookie in Request.Cookies.AllKeys)
            {
                Request.Cookies.Remove(cookie);
            }
            foreach (var cookie in Response.Cookies.AllKeys)
            {
                Response.Cookies.Remove(cookie);
            }
         
            Session["User"] = null;
            Session.Abandon();
            Session.Clear();
            return View("Login",null);
        }       
        //GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }
        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
            return View();
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await this.UserManager.AddToRoleAsync(user.Id, model.Name);
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                        return RedirectToAction("Index", "Home");
                    }
                    AddErrors(result);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();

        }
        [AllowAnonymous]
        public ActionResult ViewAll()
        {
            return View();

        }
        [AllowAnonymous]
        public ActionResult Edit()
        {
            return View();

        }
        
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var UserPassword = con.Query<SessionModel>("GETUSERBYUSERNAME",
                    new { Username=model.Email}, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (!string.IsNullOrEmpty(UserPassword.Password))
                    {
                        UserPassword.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Decrypt(UserPassword.Password, true);
                        var Templates = await _templateRepo.GetTemplateByActionName("Forget Password");
                        var WildCards = await CommonModel.GetWildCards();
                        var U = WildCards.Where(x => x.Text.ToUpper() == "USER NAME").FirstOrDefault();
                        U.Val = UserPassword.UserName;
                        U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                        U.Val = UserPassword.Password;
                        var c = WildCards.Where(x => x.Val != string.Empty).ToList();
                        if (Templates != null)
                            await _emailSmsServices.Send(Templates, c, UserPassword);
                        ResponseModel response = new ResponseModel { IsSuccess = true, Response = "Password sent successfully" };
                        TempData["response"] = response;
                        return RedirectToAction("Login");


                    }
                    else
                    {
                        ViewBag.Msg = "Email Does not exits";
                        return View(model);

                    }
          
                }
              

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }
        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}