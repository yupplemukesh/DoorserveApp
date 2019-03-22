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
//using TogoFogo.Models;
using TogoFogo.Models.Template;
using TogoFogo.Repository.EmailSmsTemplate;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Xml;

namespace TogoFogo.Controllers
{
    public class TemplatesController : Controller
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
        public async Task<ActionResult> Index()
        {
            var templates = await _templateRepo.GetTemplates();
            templates.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templates.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            return View(templates);
        }
        public async Task<ActionResult> Create()
        {
            var templatemodel = new TemplateModel();
            templatemodel.IsActive = true;
            templatemodel.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templatemodel.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            templatemodel.TemplateTypeList = new SelectList(await CommonModel.GetLookup("Template"), "Value", "Text");
            templatemodel.PriorityTypeList = new SelectList(await CommonModel.GetLookup("Priority"), "Value", "Text");
            templatemodel.EmailHeaderFooterList = new SelectList(await CommonModel.GetHeaderFooter(), "Value", "Text");
            templatemodel.IsSystemDefined = true;
            return View(templatemodel);
        }
        [HttpPost]
        public async Task<ActionResult> Create(TemplateModel templateModel)
        {
                var response =new TogoFogo.Models.ResponseModel();
                Boolean Isvalid = false;
                DataTable dtToEmailExcelData = new DataTable();
                templateModel.AddedBy = Convert.ToInt32(Session["User_ID"]);
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
                    if (!Isvalid)
                    {
                        response.Response = "Please Enter To CC Email Or Upload To Email Excel file";
                        response.IsSuccess = Isvalid;
                        TempData["response"] = response;
                        TempData.Keep("response");
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
            }
            
            if (Isvalid)
            {
                response = await _templateRepo.AddUpdateDeleteTemplate(templateModel, 'I');
                response.Response = "Please Enter To CC Email Or Upload To Email Excel file";
                response.IsSuccess = Isvalid;
                TempData["response"] = response;
                TempData.Keep("response");
            }

            return RedirectToAction("Index");           
        }
        public async Task<ActionResult> Edit(int id,Guid? GUID)
        {
            var templatemodel = new TemplateModel();
            templatemodel = await _templateRepo.GetTemplateByGUID(id,GUID);
            templatemodel.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templatemodel.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            templatemodel.TemplateTypeList = new SelectList(await CommonModel.GetLookup("Template"), "Value", "Text");
            templatemodel.PriorityTypeList = new SelectList(await CommonModel.GetLookup("Priority"), "Value", "Text");
            templatemodel.EmailHeaderFooterList = new SelectList(await CommonModel.GetHeaderFooter(), "Value", "Text");
            templatemodel.GatewayList=new SelectList(await CommonModel.GetMailerGatewayList(templatemodel.MessageTypeId), "GatewayId", "GatewayName"); 

            return View(templatemodel);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(TemplateModel templateModel)
        {
            var response = new TogoFogo.Models.ResponseModel();
            Boolean Isvalid = false;
            DataTable dtToEmailExcelData = new DataTable();
            templateModel.AddedBy = Convert.ToInt32(Session["User_ID"]);
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
                if (!Isvalid)
                {
                    response.Response = "Please Enter To CC Email Or Upload To Email Excel file";
                    response.IsSuccess = Isvalid;
                    TempData["response"] = response;
                    TempData.Keep("response");
                    return Redirect("Create");
                }

            }
            else
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
                if (!string.IsNullOrEmpty(templateModel.PhoneNumber))
                {
                    string[] strPhoneNumber = templateModel.PhoneNumber.Split(',');
                    templateModel.TotalCount += strPhoneNumber.Length;
                    Isvalid = true;
                }
            }

            if (Isvalid)
            {
                response = await _templateRepo.AddUpdateDeleteTemplate(templateModel, 'U');
                //_templateRepo.Save();
                if (response.ResponseCode == 0)
                {
                    response.Response = "Successfully updated";
                }
                else if (response.ResponseCode == 2)
                {
                    response.Response = "Already exists details";
                }
                else
                {
                    response.Response = "Someting went wrong,please try again";
                }
                TempData["response"] = response;
                TempData.Keep("response");
            }
            return RedirectToAction("Index");
        }
        public JsonResult BindGateway(Int64 GatewayTypeId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var gateway = con.Query<BindGateway>("select GatewayId,GatewayName from MSTGateway where GatewayTypeId="+ GatewayTypeId + "", commandType: CommandType.Text);
                List<ListItem> items = new List<ListItem>();
                
                foreach (var val in gateway)
                {
                    items.Add(new ListItem
                    {
                        Value = val.GatewayId.ToString(), //Value Field(ID)
                        Text = val.GatewayName //Text Field(Name)l
                    });
                }
                return Json(items, JsonRequestBehavior.AllowGet);
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
        public async Task<JsonResult> GetuploadedDataList(Guid GUID,string MessageTypeName)
        {
            var templates = await _templateRepo.GetUploadedExcelListByGUID(GUID,MessageTypeName);
            return Json(templates, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> DeleteUploadedExcelData(Guid GUID, string MessageTypeName,string UploadedData)
        {

            string strUploaded = UploadedData.Replace(',', ';');
            var templates = await _templateRepo.DeleteUploadedExcelData(GUID, MessageTypeName, strUploaded);
            return Json(templates, JsonRequestBehavior.AllowGet);
        }

    }
}