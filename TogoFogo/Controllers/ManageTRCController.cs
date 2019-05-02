using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class ManageTRCController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        // File Save Code
        private string SaveImageFile(HttpPostedFileBase file)
        {
            try
            {
                string path = Server.MapPath("~/UploadedImages");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Path.GetFileNameWithoutExtension(fileFullName);
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                return savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }

        // GET: ManageTRC
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Service_Center_TRC)]
        public ActionResult TRC()
        {
            ViewBag.Supported_Category = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.STATE_TERRITORY = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.COUNTRY = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.CITY = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SupportedCategory = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ORG_NAME = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["AddTRC"] != null )
            {
                ViewBag.AddTRC = TempData["AddTRC"].ToString();
                //ViewBag.EditTRC = TempData["EditTRC"].ToString();
            }
            if (TempData["EditTRC"] != null)
            {
               
                ViewBag.EditTRC = TempData["EditTRC"].ToString();
            }
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create}, (int)MenuCode.Manage_Service_Center_TRC)]
        public ActionResult AddTRC()
        {

            ViewBag.STATE_TERRITORY = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.COUNTRY = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ViewBag.SupportedCategory = new SelectList(dropdown.BindCategorySelectpicker(), "Value", "Text");
            ViewBag.ORG_NAME = new SelectList(dropdown.BindServiceProvider(), "Value", "Text");
            ViewBag.CITY = new SelectList(Enumerable.Empty<SelectListItem>());

          
            return View();
        }
        [HttpPost]
        public ActionResult AddTRC(ManageTRCModel model)
        {
            try
            {

                model.CREATE_BY = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                model.MODIFY_BY = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                if (model.UploadPanCardNO != null)
                {
                    model.UPLOAD_PAN_NO = SaveImageFile(model.UploadPanCardNO);
                }
                if (model.UPLOAD_GST_NO1 != null)
                {
                    model.UPLOAD_GST_NO = SaveImageFile(model.UPLOAD_GST_NO1);
                }
                if (model.PERSON_UPLOAD_PANNO1 != null)
                {
                    model.PERSON_UPLOAD_PANNO = SaveImageFile(model.PERSON_UPLOAD_PANNO1);
                }
                if (model.UPLOAD_VOTER_CARD_NO1 != null)
                {
                    model.UPLOAD_VOTER_CARD_NO = SaveImageFile(model.UPLOAD_VOTER_CARD_NO1);
                }
               if (model.UPLOAD_AADHAR_CARD_NO1 != null)
                {
                    model.UPLOAD_AADHAR_CARD_NO = SaveImageFile(model.UPLOAD_AADHAR_CARD_NO1);
                }
               if (model.UPLOAD_CANCELLED_CHEQUE1 != null)
                {
                    model.UPLOAD_CANCELLED_CHEQUE = SaveImageFile(model.UPLOAD_CANCELLED_CHEQUE1);
                }
                using (var con = new SqlConnection(_connectionString))
                {
                   
                    var finalValue = "";
                    if (model.SupportedCategory != null)
                    {
                        var problem = model.SupportedCategory.Length;
                        //for (var i = 0; i <= problem - 1; i++)
                        //{
                        //    var Data = m.ProblemFound[i].FirstOrDefault();
                        //    value = Data + ",";
                        //    finalValue = finalValue + value;
                        //}
                        finalValue = string.Join(",", model.SupportedCategory);
                       
                    }
                    var result = con.Query<int>("Add_Edit_Delete_TRC",
                        new
                        {
                            
                            TRC_ID = "",
                            model.PROCESS_NAME,
                            model.TRC_CODE,
                            model.TRC_NAME,
                            model.ORG_NAME,
                            model.CIN,
                            model.IEC_NO,
                            model.STATUTORY_TYPE,
                            model.APPL_TAX_TYPE,
                            model.GST_N0,
                            model.UPLOAD_GST_NO,
                            model.PAN_NO,
                            model.UPLOAD_PAN_NO,
                            model.ADDRESS_TYPE,
                            model.COUNTRY,
                            model.STATE_TERRITORY,
                            model.CITY,
                            model.ADDRESS,
                            model.LOCALITY,
                            model.NEAR_BY_LOCATION,
                            model.PIN_CODE,
                            model.FIRST_NAME,
                            model.LAST_NAME,
                            model.MOBILE_NO,
                            model.EMAIL,
                            model.IsUser,
                            model.CONTACT_PERSON_PANNO,
                            model.PERSON_UPLOAD_PANNO,
                            model.VOTER_CARD_N0,
                            model.UPLOAD_VOTER_CARD_NO,
                            model.AADHAR_CARD_NO,
                            model.UPLOAD_AADHAR_CARD_NO,
                            model.BANK_NAME,
                            model.BANK_ACCOUNT_NO,
                            model.COMPANY_NAME_BANK_ACCNT,
                            model.IFSC_CODE,
                            model.BANK_BRANCH,
                            model.UPLOAD_CANCELLED_CHEQUE,
                            model.IsActive,
                            model.COMMENTS,
                            model.CREATE_BY,
                            model.MODIFY_BY,
                            model.DELETE_BY,
                            ACTION = "add",
                            SupportedCategory=finalValue
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 0)
                    {
                        TempData["AddTRC"] = "TRC Code Already Exist";

                    }
                    else
                    {
                        TempData["AddTRC"] = "Successfully Added TRC";
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("TRC");
        }
        public ActionResult TRCTable()
        {
            ManageTRCModel objManageTRCModel = new ManageTRCModel();
            using (var con = new SqlConnection(_connectionString))
            {
                objManageTRCModel.ManageTRCModelList = con.Query<ManageTRCModel>("GETTRCDetails", new { }, commandType: CommandType.StoredProcedure).ToList();
              
            }
            UserActionRights objUserActiobRight = new UserActionRights();
            objManageTRCModel._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objManageTRCModel._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objManageTRCModel._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objManageTRCModel._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objManageTRCModel._UserActionRights.Delete = true;
                    }
                }
            }
            else
            {

                objManageTRCModel._UserActionRights.Create = true;
                objManageTRCModel._UserActionRights.Edit = true;
                objManageTRCModel._UserActionRights.Delete = true;
                objManageTRCModel._UserActionRights.View = true;
                objManageTRCModel._UserActionRights.History = true;
                objManageTRCModel._UserActionRights.ExcelExport = true;

            }
            return View(objManageTRCModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        public ActionResult EditTRC(int TRCID)
        {

            ViewBag.Supported_Category = new SelectList(dropdown.BindCategorySelectpicker(), "Value", "Text");
            
            ViewBag.ORG_NAME = new SelectList(dropdown.BindServiceProvider(), "Value", "Text");
            ViewBag.COUNTRY = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ViewBag.CITY = new SelectList(dropdown.BindLocation(), "Value", "Text");
            ViewBag.STATE_TERRITORY = new SelectList(dropdown.BindState(), "Value", "Text");
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageTRCModel>("Select * from msttrc where TRC_ID=@TRC_ID", new { TRC_ID = TRCID },
                    commandType: CommandType.Text).FirstOrDefault();
               
                     result.Supported_Category = result.SupportedCategory.Split(',');
                ViewBag.Supported_Category = new SelectList(dropdown.BindCategorySelectpicker(), "Value", "Text");
                return PartialView("EditTRC", result);
            }
        }
        [HttpPost]
        public ActionResult EditTRC(ManageTRCModel model)
        {
            try
            {
                model.MODIFY_BY = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                if (model.UploadPanCardNO != null)
                {
                    model.UPLOAD_PAN_NO = SaveImageFile(model.UploadPanCardNO);
                }
                 if (model.UPLOAD_GST_NO1 != null)
                {
                    model.UPLOAD_GST_NO = SaveImageFile(model.UPLOAD_GST_NO1);
                }
                 if (model.PERSON_UPLOAD_PANNO1 != null)
                {
                    model.PERSON_UPLOAD_PANNO = SaveImageFile(model.PERSON_UPLOAD_PANNO1);
                }
                 if (model.UPLOAD_VOTER_CARD_NO1 != null)
                {
                    model.UPLOAD_VOTER_CARD_NO = SaveImageFile(model.UPLOAD_VOTER_CARD_NO1);
                }
                 if (model.UPLOAD_AADHAR_CARD_NO1 != null)
                {
                    model.UPLOAD_AADHAR_CARD_NO = SaveImageFile(model.UPLOAD_AADHAR_CARD_NO1);
                }
                 if (model.UPLOAD_CANCELLED_CHEQUE1 != null)
                {
                    model.UPLOAD_CANCELLED_CHEQUE = SaveImageFile(model.UPLOAD_CANCELLED_CHEQUE1);
                }
               
                    using (var con = new SqlConnection(_connectionString))
                    {
                    var finalValue = "";
                    if (model.Supported_Category != null)
                    {
                        
                     
                        finalValue = string.Join(",", model.Supported_Category);

                    }
                    var result = con.Query<int>("Add_Edit_Delete_TRC",
                            new
                            {
                                model.TRC_ID,
                                model.PROCESS_NAME,
                                model.TRC_CODE,
                                model.TRC_NAME,
                                model.ORG_NAME,
                                model.CIN,
                                model.IEC_NO,
                                model.STATUTORY_TYPE,
                                model.APPL_TAX_TYPE,
                                model.GST_N0,
                                model.UPLOAD_GST_NO,
                                model.PAN_NO,
                                model.UPLOAD_PAN_NO,
                                model.ADDRESS_TYPE,
                                model.COUNTRY,
                                model.STATE_TERRITORY,
                                model.CITY,
                                model.ADDRESS,
                                model.LOCALITY,
                                model.NEAR_BY_LOCATION,
                                model.PIN_CODE,
                                model.FIRST_NAME,
                                model.LAST_NAME,
                                model.MOBILE_NO,
                                model.EMAIL,
                                model.IsUser,
                                model.CONTACT_PERSON_PANNO,
                                model.PERSON_UPLOAD_PANNO,
                                model.VOTER_CARD_N0,
                                model.UPLOAD_VOTER_CARD_NO,
                                model.AADHAR_CARD_NO,
                                model.UPLOAD_AADHAR_CARD_NO,
                                model.BANK_NAME,
                                model.BANK_ACCOUNT_NO,
                                model.COMPANY_NAME_BANK_ACCNT,
                                model.IFSC_CODE,
                                model.BANK_BRANCH,
                                model.UPLOAD_CANCELLED_CHEQUE,
                                model.IsActive,
                                model.COMMENTS,
                                model.CREATE_BY,
                                model.MODIFY_BY,
                                model.DELETE_BY,
                                ACTION = "edit",
                                SupportedCategory = finalValue
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 2)
                        {
                            TempData["AddTRC"] = "Updated Successfully";

                        }
                        else
                        {
                            TempData["AddTRC"] = "Something Went Wrong";
                        }
                    }
                
               
            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("TRC", "ManageTRC");
        }
    }
}