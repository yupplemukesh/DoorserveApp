using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;

namespace TogoFogo.Controllers
{
    public class Trc_PFQCController : Controller
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
        // GET: Trc_PFQC
        public ActionResult Index()
        {
            ViewBag.ReceivedDevice = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.RecvdBrand = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.RecvdModel = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Engg_Name = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ReceivedDevice = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SpareType = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.QCPersonName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.QCFailReason = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }
        public ActionResult FindPFQC()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindPFQC(string CcNO)
        {
            new AllData();
            var finalValue = "";
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetDataByCCNO", new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                var Auto_Table = con.Query<New_Auto_Fill_Table>("Select * from Maintain_SpareTable_Data where CC_NO=@CC_NO", new { @CC_NO = CcNO }, commandType: CommandType.Text).ToList();
                if (Auto_Table != null)
                {
                    result.New_Auto_Table = Auto_Table;
                }
                if (result.ChildtableDataProblem == null)
                {
                    var Problem = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem", new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).ToList();
                    result.ChildtableDataProblem = Problem;

                    foreach (var item in result.ChildtableDataProblem)
                    {
                        var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item.ProblemId }, commandType: CommandType.Text).FirstOrDefault();

                        finalValue = finalValue + " , " + result1;
                    }
                    finalValue = finalValue.Trim().TrimStart(',');
                }
                //var QCReason = "";
                //if (result.QC_Fail_Reason != null)
                //{
                //    foreach (var item in result.QC_Fail_Reason)
                //    {
                //        if (item != ',')
                //        {
                //            var result1 = con.Query<string>("SELECT QCProblem from MST_QC WHERE QCId=@QCId ", new { @QCId = item }, commandType: CommandType.Text).FirstOrDefault();
                //            QCReason = QCReason + result1 + " , ";
                //        }

                //    }
                //    result.QC_Fail_Reason = QCReason;
                //}
                if (result.QC_Data == null)
                {
                    var QC = con.Query<QCtableData>("SELECT * from MST_QC",null, commandType: CommandType.Text).ToList();
                    result.QC_Data = QC;
                }
                result.Problem = finalValue;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PFQCForm()
        {
            var SessionModel = Session["User"] as SessionModel;
            ViewBag.ReceivedDevice = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            ViewBag.RecvdBrand = new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            ViewBag.RecvdModel = new SelectList(dropdown.BindProduct(SessionModel.CompanyId), "Value", "Text");
            ViewBag.Engg_Name = new SelectList(dropdown.BindEngineer(SessionModel.CompanyId), "Value", "Text");
            ViewBag.SpareType = new SelectList(dropdown.BindSpareType(SessionModel.CompanyId), "Value", "Text");
            ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(dropdown.BindProblemObserved(SessionModel.CompanyId), "Value", "Text");
            ViewBag.QCPersonName = ViewBag.Engg_Name;
            ViewBag.QCFailReason = new SelectList(dropdown.BindQC(SessionModel.CompanyId), "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult PFQCForm(ReceiveMaterials m)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var rest = "";
                    if (m.Table_Pass_Fail != null)
                    {
                       
                        int re = 0;
                        foreach (var item in m.Table_Pass_Fail)
                        {
                            var result = con.Query<int>("Insert_into_QCInformation",
                                  new
                                  {
                                      CC_NO = item.TablePassFailCC_No,
                                      QC_ProblemID = item.TablePassFail_ProblemId,
                                      QC_Pass = item.TablePassFail_Pass,
                                      QC_Fail = item.TablePassFail_Fail

                                  }
                             
                        , commandType: CommandType.StoredProcedure).FirstOrDefault();
                            re=re+result;
                        }
                        
                        if (re != 0)
                        {
                            var result = con.Query<string>("Update_Current_Status",
                                 new
                                 {
                                     CC_NO=m.CC_NO

                                 }

                       , commandType: CommandType.StoredProcedure).FirstOrDefault();
                            rest = result;
                        }
                       
                    }
                    if (m.CC_NO != null && rest != null)
                    {
                        if (m.QUTrust_Certificate1 != null)
                        {
                            m.QUTrust_Certificate = SaveImageFile(m.QUTrust_Certificate1);

                        }
                        var value = "";
                        var finalValue = "";
                        if (m.QCFailReason != null)
                        {
                            var problem = m.QCFailReason.Length;
                            for (var i = 0; i <= problem - 1; i++)
                            {
                                var Data = m.QCFailReason[i].FirstOrDefault();
                                value = Data + ",";
                                finalValue = finalValue + value;
                            }
                        }
                      
                        var result1 = con.Query<int>("Insert_into_Pending_for_Quality_Check",
                                 new
                                 {
                                      m.CC_NO,
                                     Is_Device_Tested_by_QUTrust = m.QUTrust,
                                     QUTrust_Certificate_Number = m.QUTrustCertificateNumber,
                                     QUTrust_Score = m.QUTrustScore,
                                      m.QUTrust_Certificate,
                                     Functionality_Test = m.FunctionalityTest,
                                     QC_Fail_Reason = finalValue,
                                     Is_Device_Functioning_Normally = m.QC_IsDeviceFunctioningNormally,
                                     Is_Device_is_looking_Equal_To_New = m.QC_IsDeviceislookingEqualToNew,
                                     Final_QC_Status =m.FinalQCStatus,
                                     QC_Remarks = m.QCRemarks
                                 }
                       , commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result1 == 1)
                        {
                            TempData["Message"] = "Submitted Successfully";
                        }
                        else
                        {
                            TempData["Message"] = "Something went wrong";
                        }
                    }
                }
                return RedirectToAction("Index", "Trc_PFQC");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public ActionResult TablePFQC()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForPendingQC",
                   new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }

        }
    }
}