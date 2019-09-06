using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using doorserve.Models;
using doorserve.Models.Template;
using doorserve.Repository.EmailSmsTemplate;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Xml;
using doorserve.Permission;
using doorserve.Filters;

namespace doorserve.Controllers
{
    public class TemplatesController : BaseController
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private readonly ITemplate _templateRepo;
        public const string MatchEmailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public TemplatesController()
        {
            _templateRepo = new Template();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.EMail_SMS_Notification_IVR_Template)]
        public async Task<ActionResult> Index()
        {


            var templates = await _templateRepo.GetTemplates(new Filters.FilterModel { CompId = CurrentUser.CompanyId });
            templates.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templates.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            return View(templates);
        }

        public async Task<ActionResult> Index1()
        {


            var templates = await _templateRepo.GetTemplates(new Filters.FilterModel { CompId = CurrentUser.CompanyId });
            templates.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templates.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            return View(templates);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.EMail_SMS_Notification_IVR_Template)]
        public async Task<ActionResult> Create()
        {

            var templatemodel = new TemplateModel();
            templatemodel.IsActive = true;
            templatemodel.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templatemodel.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            templatemodel.TemplateTypeList = new SelectList(await CommonModel.GetLookup("Template"), "Value", "Text");
            templatemodel.PriorityTypeList = new SelectList(await CommonModel.GetLookup("Priority"), "Value", "Text");
            templatemodel.WildCardList = new SelectList(CommonModel.GetWildCards(CurrentUser.CompanyId), "Text", "Text");
            templatemodel.EmailHeaderFooterList = new SelectList(CommonModel.GetHeaderFooter(CurrentUser.CompanyId), "Value", "Text");
            templatemodel.IsSystemDefined = true;
            if (CurrentUser.UserTypeName.ToLower() == "super admin")
            {
                templatemodel.IsAdmin = true;
                templatemodel.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
            }
            return View(templatemodel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.EMail_SMS_Notification_IVR_Template)]
        [HttpPost]

        public async Task<ActionResult> Create(TemplateModel templateModel)
        {

            var response = new doorserve.Models.ResponseModel();
            Boolean Isvalid = false;
            DataTable dtToEmailExcelData = new DataTable();
            templateModel.UserId = CurrentUser.UserId;
            if (CurrentUser.UserTypeName.ToLower() != "super admin")
                templateModel.CompanyId = CurrentUser.CompanyId;
            if (!string.IsNullOrEmpty(templateModel.ScheduleDate) && !string.IsNullOrEmpty(templateModel.ScheduleTime))
            {

                var times = templateModel.ScheduleTime.Split(':');

                templateModel.ScheduleDateTime = Convert.ToDateTime(templateModel.ScheduleDate).AddHours(Convert.ToInt32(times[0])).AddMinutes(Convert.ToInt32(times[1])).ToString();
            }
            if (templateModel.MessageTypeName == "SMTP Gateway")
            {
                if (templateModel.ToEmailFile != null)
                {
                    string excelPath = SaveFile(templateModel.ToEmailFile, "ToEmail");
                    string conString = string.Empty;
                    string extension = Path.GetExtension(templateModel.ToEmailFile.FileName);
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 or higher
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;

                    }
                    conString = string.Format(conString, excelPath);

                    using (OleDbConnection excel_con = new OleDbConnection(conString))
                    {
                        excel_con.Open();
                        string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                        dtToEmailExcelData.Columns.AddRange(new DataColumn[1] {
                            new DataColumn("ToEmail", typeof(string))
                        });
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [To Email]as ToEmail  FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtToEmailExcelData);
                        }
                        excel_con.Close();
                    }
                    if (dtToEmailExcelData != null && dtToEmailExcelData.Rows.Count > 0)
                    {
                        var emailChecklist = dtToEmailExcelData.AsEnumerable().Select(x =>
                                      new { Valid = IsEmail(x.Field<string>("ToEmail")) }).ToList();

                        int Count = (from mail in emailChecklist
                                     where mail.Valid == false
                                     select mail).Count();
                        if (Count > 0)
                        {
                            response.Response = "Upload Valid Email";
                            response.IsSuccess = Isvalid;
                            TempData["response"] = response;
                            return Redirect("Create");
                        }
                        else
                            Isvalid = true;

                        var ToEmailList = dtToEmailExcelData.AsEnumerable().Select(r => r.Field<string>("ToEmail")).ToList();
                        templateModel.UploadedEmail = string.Join(";", ToEmailList);
                        templateModel.TotalCount = dtToEmailExcelData.Rows.Count;
                    }
                }
                if (!string.IsNullOrEmpty(templateModel.ToEmail))
                {
                    string[] strToemail = templateModel.ToEmail.Split(';');

                    templateModel.TotalCount += strToemail.Length;
                    Isvalid = true;
                }
                if (!string.IsNullOrEmpty(templateModel.ToCCEmail))
                {
                    string[] strToEmailcc = templateModel.ToCCEmail.Split(';');
                    templateModel.TotalCount += strToEmailcc.Length;
                    Isvalid = true;
                }
                if (templateModel.TemplateTypeId == 69)
                {
                    Isvalid = true;
                }
                if (!Isvalid)
                {
                    response.Response = "Please Enter To CC Email Or Upload To Email Excel file";
                    response.IsSuccess = Isvalid;
                    TempData["response"] = response;
                    return Redirect("Index");
                }
            }
            else
            {
                if (templateModel.ToMobileNoFile != null)
                {
                    string excelPath = SaveFile(templateModel.ToMobileNoFile, "ToMobile");
                    string conString = string.Empty;
                    string extension = Path.GetExtension(templateModel.ToMobileNoFile.FileName);
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 or higher
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;

                    }
                    conString = string.Format(conString, excelPath);
                    DataTable dtToMobileNoExcelData = new DataTable();
                    using (OleDbConnection excel_con = new OleDbConnection(conString))
                    {
                        excel_con.Open();
                        string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                        dtToMobileNoExcelData.Columns.AddRange(new DataColumn[1]
                        {
                            new DataColumn("ToMobileNo", typeof(string))

                        });
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [To MobileNo]as ToMobileNo  FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtToMobileNoExcelData);
                        }
                        excel_con.Close();
                    }

                    if (dtToMobileNoExcelData != null && dtToMobileNoExcelData.Rows.Count > 0)
                    {
                        var ToMobileList = dtToMobileNoExcelData.AsEnumerable().Select(r => r.Field<string>("ToMobileNo")).ToList();
                        templateModel.UploadedMobile = string.Join(",", ToMobileList);
                        templateModel.TotalCount = dtToMobileNoExcelData.Rows.Count;

                    }
                }
                if (!string.IsNullOrEmpty(templateModel.PhoneNumber))
                {
                    string[] strPhoneNumber = templateModel.PhoneNumber.Split(',');
                    templateModel.TotalCount += strPhoneNumber.Length;
                    Isvalid = true;
                }
                if (templateModel.TemplateTypeId == 69)
                {
                    Isvalid = true;
                }
            }
            if (Isvalid)
            {
                response = await _templateRepo.AddUpdateDeleteTemplate(templateModel, 'I');
                // response.Response = "Successfully inserted record";
                //response.IsSuccess = Isvalid;
                TempData["response"] = response;
            }

            return RedirectToAction("Index");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.EMail_SMS_Notification_IVR_Template)]
        public async Task<ActionResult> Edit(int id, Guid? GUID)
        {

            var templatemodel = new TemplateModel();
            templatemodel = await _templateRepo.GetTemplateByGUID(id, GUID);
            if (!string.IsNullOrEmpty(templatemodel.ScheduleDateTime))
            {
                string[] strSheduleArray = templatemodel.ScheduleDateTime.Split(' ');
                templatemodel.ScheduleDate = strSheduleArray[0];
                templatemodel.ScheduleTime = strSheduleArray[1];
            }

            templatemodel.WildCardList = new SelectList(CommonModel.GetWildCards(templatemodel.CompanyId), "Text", "Text");
            templatemodel.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templatemodel.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            templatemodel.TemplateTypeList = new SelectList(await CommonModel.GetLookup("Template"), "Value", "Text");
            templatemodel.PriorityTypeList = new SelectList(await CommonModel.GetLookup("Priority"), "Value", "Text");
            templatemodel.EmailHeaderFooterList = new SelectList(CommonModel.GetHeaderFooter(templatemodel.CompanyId), "Value", "Text");
            templatemodel.GatewayList = new SelectList(CommonModel.GetMailerGatewayList(templatemodel.MessageTypeId, templatemodel.CompanyId), "GatewayId", "GatewayName");
            if (CurrentUser.UserTypeName.ToLower() == "super admin")
            {
                templatemodel.IsAdmin = true;
                templatemodel.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
            }
            return View(templatemodel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.EMail_SMS_Notification_IVR_Template)]
        [HttpPost]

        public async Task<ActionResult> Edit(TemplateModel templateModel)
        {

            var response = new doorserve.Models.ResponseModel();
            Boolean Isvalid = false;
            DataTable dtToEmailExcelData = new DataTable();
            templateModel.UserId = CurrentUser.UserId;
            if (CurrentUser.UserTypeName.ToLower() != "super admin")
                templateModel.CompanyId = CurrentUser.CompanyId;
            if (!string.IsNullOrEmpty(templateModel.ScheduleDate) && !string.IsNullOrEmpty(templateModel.ScheduleTime))
            {
                var times = templateModel.ScheduleTime.Split(':');
                templateModel.ScheduleDateTime = Convert.ToDateTime(templateModel.ScheduleDate).AddHours(Convert.ToInt32(times[0])).AddMinutes(Convert.ToInt32(times[1])).ToString();
            }
            if (templateModel.MessageTypeName == "SMTP Gateway")
            {
                if (templateModel.ToEmailFile != null)
                {
                    string excelPath = SaveFile(templateModel.ToEmailFile, "ToEmail");
                    string conString = string.Empty;
                    string extension = Path.GetExtension(templateModel.ToEmailFile.FileName);
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 or higher
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;

                    }
                    conString = string.Format(conString, excelPath);

                    using (OleDbConnection excel_con = new OleDbConnection(conString))
                    {
                        excel_con.Open();
                        string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                        dtToEmailExcelData.Columns.AddRange(new DataColumn[1] {
                            new DataColumn("ToEmail", typeof(string))
                        });
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [To Email]as ToEmail  FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtToEmailExcelData);
                        }
                        excel_con.Close();
                    }
                    if (dtToEmailExcelData != null && dtToEmailExcelData.Rows.Count > 0)
                    {
                        var emailChecklist = dtToEmailExcelData.AsEnumerable().Select(x =>
                                      new { Valid = IsEmail(x.Field<string>("ToEmail")) }).ToList();

                        int Count = (from mail in emailChecklist
                                     where mail.Valid == false
                                     select mail).Count();
                        if (Count > 0)
                        {
                            response.Response = "Upload Valid Email";
                            response.IsSuccess = Isvalid;
                            TempData["response"] = response;
                            TempData.Keep("response");
                            return Redirect("Create");
                        }
                        else
                            Isvalid = true;

                        var ToEmailList = dtToEmailExcelData.AsEnumerable().Select(r => r.Field<string>("ToEmail")).ToList();
                        templateModel.UploadedEmail = string.Join(";", ToEmailList);
                        templateModel.TotalCount = dtToEmailExcelData.Rows.Count;
                    }
                }
                if (!string.IsNullOrEmpty(templateModel.ToEmail))
                {
                    string[] strToemail = templateModel.ToEmail.Split(';');

                    templateModel.TotalCount += strToemail.Length;
                    Isvalid = true;
                }
                if (!string.IsNullOrEmpty(templateModel.ToCCEmail))
                {
                    string[] strToEmailcc = templateModel.ToCCEmail.Split(';');
                    templateModel.TotalCount += strToEmailcc.Length;
                    Isvalid = true;
                }
                if (!string.IsNullOrEmpty(templateModel.BccEmails))
                {
                    string[] strToEmailcc = templateModel.BccEmails.Split(';');
                    templateModel.TotalCount += strToEmailcc.Length;
                    Isvalid = true;
                }

                if (!Isvalid)
                {
                    response.Response = "Please Enter To CC Email Or Upload To Email Excel file";
                    response.IsSuccess = Isvalid;
                    TempData["response"] = response;
                    return Redirect("Create");
                }

            }
            else
            {
                if (templateModel.ToMobileNoFile != null)
                {
                    string excelPath = SaveFile(templateModel.ToMobileNoFile, "ToMobile");
                    string conString = string.Empty;
                    string extension = Path.GetExtension(templateModel.ToMobileNoFile.FileName);
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 or higher
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;

                    }
                    conString = string.Format(conString, excelPath);
                    DataTable dtToMobileNoExcelData = new DataTable();
                    using (OleDbConnection excel_con = new OleDbConnection(conString))
                    {
                        excel_con.Open();
                        string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                        dtToMobileNoExcelData.Columns.AddRange(new DataColumn[1]
                        {
                            new DataColumn("ToMobileNo", typeof(string))

                        });
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [To MobileNo]as ToMobileNo  FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtToMobileNoExcelData);
                        }
                        excel_con.Close();
                    }

                    if (dtToMobileNoExcelData != null && dtToMobileNoExcelData.Rows.Count > 0)
                    {
                        var ToMobileList = dtToMobileNoExcelData.AsEnumerable().Select(r => r.Field<string>("ToMobileNo")).ToList();
                        templateModel.UploadedMobile = string.Join(",", ToMobileList);
                        templateModel.TotalCount = dtToMobileNoExcelData.Rows.Count;

                    }
                }
                if (!string.IsNullOrEmpty(templateModel.PhoneNumber))
                {
                    string[] strPhoneNumber = templateModel.PhoneNumber.Split(',');
                    templateModel.TotalCount += strPhoneNumber.Length;
                    Isvalid = true;
                }

            }

            if (templateModel.TemplateTypeId == 69)
            {
                Isvalid = true;
            }
            if (Isvalid)
            {

                response = await _templateRepo.AddUpdateDeleteTemplate(templateModel, 'U');
                _templateRepo.Save();
                if (response.ResponseCode == 0)
                {
                    response.Response = "Successfully updated";
                }

                TempData["response"] = response;
                TempData.Keep("response");
            }
            return RedirectToAction("Index");
        }
        public ActionResult BindGateway(Int64 GatewayTypeId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var getway = CommonModel.GetMailerGatewayList(GatewayTypeId, CurrentUser.CompanyId);

                return Json(getway, JsonRequestBehavior.AllowGet);
            }
        }
        private string SaveFile(HttpPostedFileBase file, string folderName)
        {
            try
            {
                string path = Server.MapPath("~/Files/" + folderName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Guid.NewGuid();
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                return path + "\\" + savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        public static bool IsEmail(string email)
        {
            if (email != null)
                return System.Text.RegularExpressions.Regex.IsMatch(email, MatchEmailPattern);
            else
                return false;
        }
        public async Task<JsonResult> GetuploadedDataList(Guid GUID, string MessageTypeName)
        {
            var templates = await _templateRepo.GetUploadedExcelListByGUID(GUID, MessageTypeName);
            return Json(templates, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> DeleteUploadedExcelData(Guid GUID, string MessageTypeName, string UploadedData)
        {

            string strUploaded = string.Empty;
            string mstrMsgType = "SMTP Gateway";
            if (MessageTypeName.ToLower() == mstrMsgType.ToLower())
            {
                strUploaded = UploadedData.Replace(',', ';');
            }
            else
            {
                strUploaded = UploadedData;
            }

            var templates = await _templateRepo.DeleteUploadedExcelData(GUID, MessageTypeName, strUploaded);
            return Json(templates, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHeaderFooterByCompany(Guid? compId,int MessageTypeId)
        {
            var result = new HeaderFooterTemplateModel {
                EmailHeaderFooterList = new SelectList(CommonModel.GetHeaderFooter(compId), "value", "text"),
                WildCardList = new SelectList(CommonModel.GetWildCards(compId), "value", "text"),
               GatewayList = new SelectList(CommonModel.GetMailerGatewayList(MessageTypeId, compId), "GatewayId", "GatewayName")
        };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}