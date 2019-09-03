using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using doorserve.Models;

namespace doorserve.Controllers
{
    public class RepairStatusController : BaseController
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        // GET: RepairStatus
        public ActionResult RepairStatus()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }
        public ActionResult UpdateRepairStatus()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<RepairStatusModel>("GetUpdaterepairStatus",
                        new { EngineerId =1}, commandType: CommandType.StoredProcedure).FirstOrDefault();


                return View(result);
            }

        }

        public ActionResult TableRepairStatus()
        {
            return View();
        }

        public ActionResult EditRepairStatus()
        {
            ViewBag.CourierName = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.PrblmObsrvd = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.SpareType = new SelectList(Enumerable.Empty<SelectList>());
            return View();
        }
       

        public ActionResult FindByCcNo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FindByCcNo(string CcNO)
        {
            using (var con = new SqlConnection(_connectionString))
            {
              
                    var result = con.Query<AllData>("GetDataByCCNO",
                   new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    //var problem = "";
                    //var problemFound = "";
                    //var QCReason = "";
                    //if (result.PrblmObsrvd != null)
                    //{
                    //    foreach (var item in result.PrblmObsrvd)
                    //    {
                    //        if (item != ',')
                    //        {
                    //            var result1 = con.Query<string>("select ProblemObserved from MstProblemObserved WHERE ProblemId =@ProblemId ", new { @ProblemId = item }, commandType: CommandType.Text).FirstOrDefault();
                    //            problem = problem + result1 + " , ";
                    //        }

                    //    }
                    //    result.PrblmObsrvd = problem;
                    //}

                    //if (result.PfelsProblemFound != null)
                    //{
                    //    foreach (var item in result.PfelsProblemFound)
                    //    {
                    //        if (item != ',')
                    //        {
                    //            var result1 = con.Query<string>("select ProblemObserved from MstProblemObserved WHERE ProblemId =@ProblemId ", new { @ProblemId = item }, commandType: CommandType.Text).FirstOrDefault();
                    //            problemFound = problemFound + result1 + " , ";
                    //        }

                    //    }
                    //    result.PfelsProblemFound = problemFound;
                    //}
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
                    //var d = con.Query<spareTestPFELSForm1>("getTableDataList",
                    //   new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).ToList();
                    //result.TableData1 = d;
                    //int spareCost1 = 0;
                    //foreach (var item in result.TableData1)
                    //{

                    //    int sc = Int32.Parse(item.TablespareTotalField1);
                    //    spareCost1 = spareCost1 + sc;
                    //}

                    //result.ApprovedSpareCost = spareCost1.ToString();
                    //var QC = con.Query<QCtableData>("Select * from mst_QC", null, commandType: CommandType.Text).ToList();
                    //result.QC_Data = QC;

                    return Json(result, JsonRequestBehavior.AllowGet);                                                           
            }
        }

        public ActionResult EditRepairStatus1()
        {


            ViewBag.CourierName = new SelectList(dropdown.BindCourier(CurrentUser.CompanyId), "Value", "Text");
            ViewBag.PrblmObsrvd = new SelectList(dropdown.BindProblemObserved(CurrentUser.CompanyId), "Value", "Text");
            ViewBag.SpareType = new SelectList(dropdown.BindSpareType(CurrentUser.CompanyId), "Value", "Text");
            ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectList>());
            return View();
        }
        [HttpPost]
        public ActionResult EditRepairStatus1(EditRepairStatus Emodel)
        {
            var value = "";
            var finalValue = "";
            var problem =Emodel.PrblmObsrvd.Length;
            for (var i=0; i<=problem-1;i++)
            {
                var Data=Emodel.PrblmObsrvd[i].FirstOrDefault();
                 value=Data + ",";
                 finalValue =finalValue + value;
            }
           
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("UpdateRepairRequest",
                        new
                        {
                            CC_NO = Emodel.CcNo,
                            Emodel.Serial_No,
                            Emodel.IMEI1,
                            Emodel.IMEI2,
                            Emodel.SE_Action,
                            Emodel.VisitDatetime,
                            Emodel.Engg_Name,
                            Emodel.Pickupdatetime,
                            Emodel.CourierName,
                            Emodel.PhysicalDamage,
                            Emodel.WarrantyVoid,
                            PrblmObsrvd=finalValue,
                            Emodel.SpareType,
                            Emodel.SpareName,
                            Emodel.Quantity,
                            Emodel.ServiceCharge,
                            Emodel.SpareCost,
                            Emodel.EstimatedCost,
                            Emodel.IsApproved,
                            Emodel.RepairStatus,
                            Emodel.CllectableAmt,
                            Emodel.PaymentMode,
                            Emodel.CashRecvd,
                            Emodel.BalanceAmt,
                            Emodel.TransAmt,
                            Emodel.TransDateTime,
                            Emodel.TransNumber,
                            Emodel.RevisitDatetime,
                            Emodel.CreatedBy,
                            Emodel.MsgToCust,
                            Emodel.SERemarks
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {
                        TempData["Message"] = "Successfully Added";
                    }
                    else {
                        TempData["Message"] = "Something Went Wrong";
                    }

                }
                return View("RepairStatus");
            }
            catch (Exception e)
            {

                throw;
            }
           
        }

        public JsonResult CourierResult(int? val)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<EditRepairStatus>("select UploadedCourierFile,BikeNumber,MobileNumber from Courier_Master WHERE CourierId=@CourierId",
                    new { @CourierId = val }, commandType: CommandType.Text).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);


            }
        }
        public JsonResult SpareName(int? spareType,int ? spareName)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<CourierValuesModel>("getDateInPOOWRR_Table",
                    new { Sparetype = spareType, Sparename = spareName }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                var url = "http://crm.doorserve.com/UploadedImages/"+result.Part_Image;
                result.Part_Image = url;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult RowStatusUpdated(string CC_NO, int? SpareCode)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("RemoveTableRowStatus",
                    new { CC_NO = CC_NO, SpareCode = SpareCode }, commandType: CommandType.StoredProcedure).FirstOrDefault();
              

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEstimatedPrice(int SpareCode )
        {
            
             using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("GetEstimatedCost",
                    new {SpareCode}, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}