using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;


namespace TogoFogo.Controllers
{
    
    public class Trc_PFELSController : Controller
    {
        private readonly string _connectionString =
         ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
 
        // GET: Trc_PFELS
        public ActionResult Index()
        {
            ViewBag.RecvdBrand = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.RecvdModel = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Engg_Name = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ReceivedDevice = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SpareType = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.CurrentStatus = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.CallStatus = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }
        public ActionResult FindPFELS()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindPFELS(string CcNO)
        {
            new AllData();
            var finalValue = "";
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetDataByCCNO", new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();

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

                result.Problem = finalValue;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PFELSForm()
        {
            var SessionModel = Session["User"] as SessionModel;
            ViewBag.ReceivedDevice = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            ViewBag.RecvdBrand = new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            ViewBag.RecvdModel = new SelectList(dropdown.BindProduct(SessionModel.CompanyId), "Value", "Text");
            ViewBag.Engg_Name = new SelectList(dropdown.BindEngineer(SessionModel.CompanyId), "Value", "Text");
            ViewBag.SpareType = new SelectList(dropdown.BindSpareType(SessionModel.CompanyId), "Value", "Text");
            ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(dropdown.BindProblemObserved(SessionModel.CompanyId), "Value", "Text");
            ViewBag.CurrentStatus = new SelectList(dropdown.BindStatusMaster(),"Value","Text");
            ViewBag.CallStatus = new SelectList(dropdown.BindCall_Status_Master(), "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult PFELSForm(ReceiveMaterials m)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    if (m.TableData != null)
                    {
                        foreach (var item in m.TableData)
                        {
                            //item.spareCcNo
                            //Delete procedure InsertDataPFELSTable
                            var result = con.Query<int>("Insert_Maintain_SpareTable_Data_NEW",
                                   new
                                   {
                                       CC_NO = item.TablespaceCC_NOField,
                                       SpareType=item.TablespareTypeField,
                                       SpareCode=item.TablespareCodeField,                                       
                                       SpareName=item.TablespareNameField,
                                       SpareQuantity = item.TablespareQuantityField
                                       
                                   }

                         , commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                     
                    }
                    var value = "";
                    var finalValue = "";
                    if (m.ProblemFound != null)
                    {
                        var problem = m.ProblemFound.Length;
                        for (var i = 0; i <= problem - 1; i++)
                        {
                            var Data = m.ProblemFound[i].FirstOrDefault();
                            value = Data + ",";
                            finalValue = finalValue + value;
                        }
                    }
                   
                    if (m.JobNumber != null)
                    {
                        var result = con.Query<int>("PfelsProc",
                                  new
                                  {
                                      m.CC_NO,
                                      m.CallStatus,
                                      JobDate=m.JOBDate,
                                      EngineerName=m.Engg_Name,
                                      m.WarrantyStickerTempered,
                                      m.CurrentStatus,
                                      m.JobNumber,
                                      m.DeviceWaterDamaged,
                                      DeviceWarrantyVoid=m.WarrantyVoid,
                                      ProblemFound= finalValue,
                                      OS_Software_Reinstall=m.OS_SoftwareReinstall,
                                      Customer_Data_Backup=m.CustomerDataBackup,
                                      Current_OS_Software_Name=m.CurrentOS_Software,
                                      Installed_OS=m.InstalledOS_Software,
                                      m.EngineerRemarks,
                                      m.DeviceServiceCharge

                                  }

                        , commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 1)
                        {
                            TempData["Message"] = "Added Successfully";
                        }
                        else if (result == 2)
                        {
                            TempData["Message"] = "Updated Successfully";
                        }
                        else
                        {
                            TempData["Message"] = "Something Went Wrong";
                        }
                    }
                   
                }
                return RedirectToAction("Index", "Trc_PFELS");
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public ActionResult TablePFELS()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForTrc_PFELS",
                   new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }

        }
    }
}