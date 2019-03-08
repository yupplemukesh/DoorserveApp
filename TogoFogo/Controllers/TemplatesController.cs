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

namespace TogoFogo.Controllers
{
    public class TemplatesController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private readonly ITemplate _templateRepo;
        public const string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]? [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
    + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]? [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
    + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
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
            if (templateModel.MailerTemplateName == "NonActionBased")
            {
                Boolean Isvalid = false;
                DataTable dtToEmailExcelData = new DataTable();
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

                        }
                    }
                    if (!string.IsNullOrEmpty(templateModel.ToEmail))
                    {
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
                    System.IO.StringWriter writer = new System.IO.StringWriter();
                    dtToEmailExcelData.WriteXml(writer, XmlWriteMode.WriteSchema, false);
                    string result = writer.ToString();
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
                }
            }
            else
            {
                templateModel.AddedBy = Convert.ToInt32(Session["User_ID"]);
                response = await _templateRepo.AddUpdateDeleteTemplate(templateModel, 'I');
                _templateRepo.Save();
                if (response.ResponseCode == 0)
                {
                    response.Response = "Successfully insert details";
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
        public async Task<ActionResult> Edit(int id)
        {
            var templatemodel = new TemplateModel();        
            templatemodel = await _templateRepo.GetTemplateById(id);
            templatemodel.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templatemodel.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            templatemodel.TemplateTypeList = new SelectList(await CommonModel.GetLookup("Template"), "Value", "Text");
            templatemodel.PriorityTypeList = new SelectList(await CommonModel.GetLookup("Priority"), "Value", "Text");
            templatemodel.EmailHeaderFooterList = new SelectList(await CommonModel.GetHeaderFooter(), "Value", "Text");
            templatemodel.GatewayList=new SelectList(await CommonModel.GetMailerGatewayList(templatemodel.MessageTypeId), "GatewayId", "GatewayName"); 

            return View(templatemodel);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(TemplateModel templatemodel)
        {            
                templatemodel.AddedBy = Convert.ToInt32(Session["User_ID"]);
                var response = await _templateRepo.AddUpdateDeleteTemplate(templatemodel, 'U');
                _templateRepo.Save();
            if(response.ResponseCode==0)
            {
                response.Response = "Successfully updated";
            }
            else if(response.ResponseCode == 2)
            {
                response.Response = "Already exists details";
            }
            else
            {
                response.Response = "Someting went wrong,please try again";
            }
                TempData["response"] = response;
                TempData.Keep("response");
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
        //public async Task<JsonResult> GetTemplatefilterData(int MessageTypeId, int ActionTypeId, string MailerTemplateName)
        //{
        //    var templates = await _templateRepo.GetTemplatesByMessageTypeActionType(MessageTypeId, ActionTypeId, MailerTemplateName);
        //    return Json(templates, JsonRequestBehavior.AllowGet);
        //}
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
    }
}