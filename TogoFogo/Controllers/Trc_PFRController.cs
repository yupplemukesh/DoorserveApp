using Dapper;
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
    public class Trc_PFRController : Controller
    {
        private readonly string _connectionString =
        ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        // GET: Trc_PFR
        public ActionResult Index()
        {
            ViewBag.RecvdBrand = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.RecvdModel = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Engg_Name = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ReceivedDevice = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SpareType = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.QCPersonName = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }
        public ActionResult FindPFR()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindPFR(string CcNO)
        {
            new AllData();
            var finalValue = "";
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetDataByCCNO", new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                var Auto_Table=con.Query<New_Auto_Fill_Table>("Select * from Maintain_SpareTable_Data where CC_NO=@CC_NO",new { @CC_NO= CcNO }, commandType: CommandType.Text).ToList();
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

                        finalValue = string.Join(",", result1);
                    }

                }

                result.Problem = finalValue;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PFRForm()
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
            return View();
        }
        [HttpPost]
        public ActionResult PFRForm(ReceiveMaterials m)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    if (m.TableData1 != null)
                    {
                        foreach (var item in m.TableData1)
                        {
                            var DeleteQuery = con.Execute("delete Maintain_SpareTable_Data where CC_NO = @CC_NO", new { @CC_NO = item.TablespaceCC_NOField1 }, commandType: CommandType.Text);
                            var result = con.Query<int>("Insert_Maintain_SpareTable_Data_NEW",
                                   new
                                   {
                    
                                       CC_NO = item.TablespaceCC_NOField1,
                                       SpareType = item.TablespareTypeField1,
                                       SpareCode = item.TablespareCodeField1,
                                       SpareName = item.TablespareNameField1,
                                       SpareQuantity = item.TablespareQuantityField1,
                                       Price = item.TablesparePriceField1,
                                       Total = item.TablespareTotalField1,
                                       SparePhoto=item.TablesparePartPhoto1
                                   }

                         , commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                    }
                    if (m.CC_NO != null)
                    {
                        var result1 = con.Query<int>("Add_In_Repair_Request_Details_from_Trc_PFR",
                          new
                          {
                              m.CC_NO,
                              m.ApprovedSpareCost,
                              m.NeedApprovalofSpareCost,
                              m.AdditionalSpareCost,
                              m.TotalEstimatedSpareCost,
                              m.CurrentRepairCostApproved,
                              m.ApprovedRepairCost,
                              m.NeedApprovalofRepairCost,
                              m.TotalEstimatedRepairCost,
                              m.IsRepairCostApproved,
                              m.IsApprovalRequiredforAdditionalCost,
                              m.EngineerRemarks,
                              m.FinalRepairStatus,
                              m.IsDeviceFunctioningNormally,
                              m.IsDeviceislookingEqualToNew,
                              m.QCPersonName,
                              m.PendingJOBsoftheQCPerson,
                              m.IsRepairCost_is_Less_than_Approved_Cost
                          }, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
                return RedirectToAction("Index", "Trc_PFR");
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }

        public ActionResult TablePFR()
        {
            var ccno = "";
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForPendingRepair",
                   new { }, commandType: CommandType.StoredProcedure).ToList();
                
                foreach (var item in result)
                {
                    ccno = item.CC_NO;
                    var result1 = con.Query<Get_Spare_Type>("select * from Maintain_SpareTable_Data where RowStatus='Inserted' AND CC_NO=@CC_NO",
                    new { @CC_NO = ccno }, commandType: CommandType.Text).ToList();
                    var r = "";
                    var problemFound = "";
                    foreach (var item1 in result1)
                    {
                        
                        r =r+ item1.SpareName + ',';
                    }
                    item.getServiceCharge = r;
                    if (item.probF != null)
                    {
                        foreach (var i in item.probF)
                        {
                            if (i != ',')
                            {
                                var result2 = con.Query<string>("select ProblemObserved from MstProblemObserved WHERE ProblemId =@ProblemId ", new { @ProblemId = i }, commandType: CommandType.Text).FirstOrDefault();
                                problemFound = problemFound + result2 + " , ";
                            }
                        }
                       
                    }
                    item.probF = problemFound;
                }
               

                return View(result);
            }

        }
    }
}