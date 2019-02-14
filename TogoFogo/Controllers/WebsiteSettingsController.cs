﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;

namespace TogoFogo.Controllers
{
    public class WebsiteSettingsController : Controller
    {
        private readonly string _connectionString =
           ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        public ActionResult EmailGatewayIndex()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.AddLocation = TempData["Message"].ToString();
            }
            return View();
        }

        public ActionResult AddEmailGateway()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddEmailGateway(EmailGateway model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_EmailGateway",
                        new
                        {
                            model.ProcessName,
                            model.SettingName,
                            model.SettingID,
                            model.FromID,
                            model.FromName,
                            model.UserName,
                            model.Paswrd,
                            model.ServerName,
                            model.PortNumber,
                            model.SSLEnable,
                            model.IsDefault,
                            model.IsActive,
                            model.Comments,
                            User = "123",
                            Action = "add"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {

                        TempData["Message"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["Message"] = "Setting Name Already Exist";
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("EmailGatewayIndex");
        }

        public ActionResult EditEmailGateway(int? id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<EmailGateway>("Select * from MstEmailGateway where SettingID=@id", new { id = id },
                    commandType: CommandType.Text).FirstOrDefault();
                return PartialView(result);
            }
        }

        [HttpPost]
        public ActionResult EditEmailGateway(EmailGateway model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_EmailGateway",
                        new
                        {
                            model.ProcessName,
                            model.SettingName,
                            model.SettingID,
                            model.FromID,
                            model.FromName,
                            model.UserName,
                            model.Paswrd,
                            model.ServerName,
                            model.PortNumber,
                            model.SSLEnable,
                            model.IsDefault,
                            model.IsActive,
                            model.Comments,
                            User = "123",
                            Action = "edit"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {

                        TempData["Message"] = "Successfully Update";
                    }
                    else
                    {

                        TempData["Message"] = "Something Went Wrong";
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("EmailGatewayIndex");
        }
        public ActionResult EmailGatewayTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<EmailGateway>("Select * from MstEmailGateway", new { }, commandType: CommandType.Text).ToList();
                return View(result);
            }
        }

        // ***********************SMSGate****************************************
        public ActionResult SMSGatewayIndex()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.AddLocation = TempData["Message"].ToString();
            }
            return View();
        }
        public ActionResult AddSMSGateway()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddSMSGateway(SMSGateway model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_SMSGateway",
                        new
                        {
                            model.ProcessName,
                            model.SettingID,
                            model.SettingName,
                            model.CompanyName,
                            model.LoginURL,
                            model.UserID,
                            model.Paswrd,
                            model.URLSetting,
                            model.SuccessMsg,
                            model.AvailCredit,
                            model.IsActive,
                            model.Comments,
                            User = "123",
                            Action = "add"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {

                        TempData["Message"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["Message"] = "Setting Name Already Exist";
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("SMSGatewayIndex");
        }

        public ActionResult EditSMSGateway(int? id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<SMSGateway>("Select * from MstSMSGateway where SettingID=@id", new { id = id },
                    commandType: CommandType.Text).FirstOrDefault();
                return PartialView(result);
            }
        }

        [HttpPost]
        public ActionResult EditSMSGateway(SMSGateway model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_SMSGateway",
                        new
                        {
                            model.ProcessName,
                            model.SettingID,
                            model.SettingName,
                            model.CompanyName,
                            model.LoginURL,
                            model.UserID,
                            model.Paswrd,
                            model.URLSetting,
                            model.SuccessMsg,
                            model.AvailCredit,
                            model.IsActive,
                            model.Comments,
                            User = "123",
                            Action = "edit"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {

                        TempData["Message"] = "Successfully Update";
                    }
                    else
                    {

                        TempData["Message"] = "Something Went Wrong";
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("SMSGatewayIndex");
        }
        public ActionResult SMSGatewayTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<SMSGateway>("Select * from MstSMSGateway", new { }, commandType: CommandType.Text).ToList();
                return View(result);
            }
        }
        // ***********************Header Footer Tempelate****************************************
        public ActionResult HFTemplateIndex()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.AddLocation = TempData["Message"].ToString();
            }
            return View();
        }
        public ActionResult AddHFTemplate()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddHFTemplate(HFTemplate model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_HFTemplate",
                        new
                        {
                            model.TemplateID,
                            model.ProcessName,
                            model.ActionName,
                            model.TemplateName,
                            model.UsedIn,
                            model.HeaderContent,
                            model.FooterContent,
                            model.IsActive,
                            model.Comments,
                            User = "123",
                            Action = "add"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {

                        TempData["Message"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["Message"] = "Setting Name Already Exist";
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("HFTemplateIndex");
        }

        public ActionResult EditHFTemplate(int? id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<HFTemplate>("Select * from HeaderFooterTemplate where TemplateID=@id", new { id = id },
                    commandType: CommandType.Text).FirstOrDefault();
                return PartialView(result);
            }
        }

        [HttpPost]
        public ActionResult EditHFTemplate(HFTemplate model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_HFTemplate",
                        new
                        {
                            model.TemplateID,
                            model.ProcessName,
                            model.ActionName,
                            model.TemplateName,
                            model.UsedIn,
                            model.HeaderContent,
                            model.FooterContent,
                            model.IsActive,
                            model.Comments,
                            User = "123",
                            Action = "edit"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {

                        TempData["Message"] = "Successfully Update";
                    }
                    else
                    {

                        TempData["Message"] = "Something Went Wrong";
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("HFTemplateIndex");
        }
        public ActionResult HFTemplateTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<HFTemplate>("Select * from HeaderFooterTemplate", new { }, commandType: CommandType.Text).ToList();
                return View(result);
            }
        }


        // ***********************IVR Tempelate****************************************

        public ActionResult IVRTemplateIndex()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.AddLocation = TempData["Message"].ToString();
            }
            return View();
        }
        public ActionResult AddIVRTemplate()
        {
            ViewBag.Gateway = new SelectList(Enumerable.Empty<SelectListItem>());
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddIVRTemplate(IVRTemplate model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_IVRTemplate",
                        new
                        {
                            model.TemplateID,
                            model.ProcessName,
                            model.ActionName,
                            model.TemplateName,
                            model.UsedIn,
                            model.MsgType,
                            model.HFTemplateName,
                            model.Gateway,
                            model.SSLEnable,
                            model.IsSystemEmail,
                            model.IsSystemSMS,
                            model.ToEmailID,
                            model.ToMobileNo,
                            model.BCCEmailID,
                            model.EmailSubject,
                            model.MsgContent,
                            model.Priority,
                            model.IsActive,
                            model.Comments,
                            User = "123",
                            Action = "add"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {

                        TempData["Message"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["Message"] = "Setting Name Already Exist";
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("IVRTemplateIndex");
        }

        public ActionResult EditIVRTemplate(int? id)
        {
            ViewBag.Gateway = new SelectList(Enumerable.Empty<SelectListItem>());
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<IVRTemplate>("Select * from IVRTemplate where TemplateID=@id", new { id = id },
                    commandType: CommandType.Text).FirstOrDefault();
                if (result != null)
                {
                    if (result.MsgType == "E-Mail")
                    {
                        ViewBag.Gateway = new SelectList(dropdown.BindEmailGatewayEdit(), "Value", "Text");
                    }
                    else if (result.MsgType == "SMS")
                    {
                        ViewBag.Gateway = new SelectList(dropdown.BindSMSGatewayEdit(), "Value", "Text");
                    }
                }
                return PartialView(result);
            }
        }

        [HttpPost]
        public ActionResult EditIVRTemplate(IVRTemplate model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_IVRTemplate",
                        new
                        {
                            model.TemplateID,
                            model.ProcessName,
                            model.ActionName,
                            model.TemplateName,
                            model.UsedIn,
                            model.MsgType,
                            model.HFTemplateName,
                            model.Gateway,
                            model.SSLEnable,
                            model.IsSystemEmail,
                            model.IsSystemSMS,
                            model.ToEmailID,
                            model.ToMobileNo,
                            model.BCCEmailID,
                            model.EmailSubject,
                            model.MsgContent,
                            model.Priority,
                            model.IsActive,
                            model.Comments,
                            User = "123",
                            Action = "edit"
                        }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {

                        TempData["Message"] = "Successfully Update";
                    }
                    else
                    {

                        TempData["Message"] = "Something Went Wrong";
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("IVRTemplateIndex");
        }
        public ActionResult IVRTemplateTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<IVRTemplate>("Select * from IVRTemplate", new { }, commandType: CommandType.Text).ToList();
                return View(result);
            }
        }
    }
}