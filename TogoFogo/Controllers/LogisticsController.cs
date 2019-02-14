﻿using Dapper;
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
   
    public class LogisticsController : Controller
    {
        private readonly string _connectionString =
           ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // File Save Code
        private string SaveImageFile(HttpPostedFileBase file)
        {
            try
            {
                string path = Server.MapPath("~/Uploaded Images");
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
        // GET: Logistics
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FindPFRAWBA()
        {
            return View();
        }
       
        //[HttpPost]
        //public ActionResult PFRAWBAForm(PFRAWBAModel m)
        //{
        //    using (var con = new SqlConnection(_connectionString))
        //    {
               
        //        var result = con.Query<int>("InsertServiceActionDetails"
        //            , new
        //            {
                    
        //               CC_No= m.CC_NO,
        //                m.SE_Action,
        //                m.Pickupdatetime,
        //                m.CourierName,
        //               MsgToCusto= m.MessageCusto,
        //                SE_Remarks= m.SERemarks,
                       
        //                CreatedBy = "",
                       
        //                Action = "add"
        //            },
        //            commandType: CommandType.StoredProcedure).FirstOrDefault();
        //        if (result == 1)
        //        {
        //            TempData["Message"] = "Updated Successfully";
        //        }

        //        return RedirectToAction("Index", "Logistics");
        //    }
        //}

        // Update Reverse AWB Status
        public ActionResult UpdateReverseAWB()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            using (var con = new SqlConnection(_connectionString))
            {
              var result = con.Query<UREVASmodel>("GetCourierCount", null, commandType: CommandType.StoredProcedure).ToList();
              return View(result);
            }
           
        }
        public ActionResult UREVASform( string CourierId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = new UREVASmodel();
                 result = con.Query<UREVASmodel>("Select CourierName from Courier_Master where CourierId=@CourierId ", new { @CourierId= CourierId }, commandType: CommandType.Text).FirstOrDefault();
                var result1 = con.Query<UREVASTable>("UREVAS_Page_Single_Table", null, commandType: CommandType.StoredProcedure).ToList();
                result.AksTable = result1;
                return View(result);
            }
        }
        public ActionResult Single_UREVAS_Table()
        {
            return View();
        }
        public ActionResult FinalUpdateReverseAWB(string AWB)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UREVASmodel>("UREVAS_Page_Single_Table1", new { AWBNumber = AWB }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                //result = con.Query<UREVASmodel>("Select * from Courier_Master where CourierId=@CourierId ", new { @CourierId = CourierId }, commandType: CommandType.Text).FirstOrDefault();
                //var result1 = con.Query<UREVASTable>("UREVAS_Page_Single_Table", null, commandType: CommandType.StoredProcedure).ToList();
                //result.AksTable = result1;
                //return View(result);
                return View(result);
            }
        }
        [HttpPost]
        public ActionResult FinalUpdateReverseAWB(UREVASmodel m)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("Insert_into_Reverse_AWB_Allocation_From_UREVAS",
                    new { UREVAS_ReverseAWBStatus =m.ReverseAWBStatus, UREVAS_Status=m.Status, UREVAS_Remarks=m.Remarks,CC_NO=m.CC_NO }
                , commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (result == 1)
                {
                    return RedirectToAction("UpdateReverseAWB", "Logistics");
                }
                return View();
            }
        }
        [HttpPost]
        public ActionResult UREVASform(UREVASmodel m)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("SubmitDataByUREVAS", new { m.CC_NO,m.ReverseAWBStatus }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 1)
                {
                    TempData["Message"] = "Updated Successfully";
                }
                else
                {
                    TempData["Message"] = "Something Went Wrong";
                }
            }
            return RedirectToAction("UpdateReverseAWB", "Logistics");
        }
        public ActionResult UpdateAWBStatus()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UREVASmodel>("GetDataUREVAS", null, commandType: CommandType.StoredProcedure).ToList();
                var result1 = con.Query<int>("select COUNT(courierId) from Repair_Request_Details", null, commandType: CommandType.Text).FirstOrDefault();
                ViewBag.TotalOpenAWBStatus = result1;
                return View(result);
            }
        }
        public ActionResult AWBStatusform(string AWBNumber)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UREVASmodel>("GetSingleDataUpdateAWBStatus", new { AWBNumber }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return View(result);
            }
        }
        [HttpPost]
        public ActionResult AWBStatusform(UREVASmodel m)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("Insert_Into_UpdateAWBStatusTable", new { m.AWBNumber,m.AWBStatus,m.CourierName }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 1)
                {
                    TempData["Message"] = "Updated Successfully";
                }
                else
                {
                    TempData["Message"] = "Something Went Wrong";
                }
            }
            return RedirectToAction("UpdateAWBStatus");
        }
        public ActionResult Reverse_AWB_Allocation()
        {
            ViewBag.ServiceProviderName = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ReverseAWB_AllocationModel>("getDataInRewerse_Awb_Allocation", null, commandType: CommandType.StoredProcedure).ToList();
               
                return View(result);
            }
        }
        public ActionResult PFRAWBAForm(string CC_NO)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                ViewBag.ServiceProviderName = new SelectList(dropdown.BindServiceProvider(), "Value", "Text");
                ViewBag.CourierName = new SelectList(dropdown.BindCourier(), "Value", "Text");
                ViewBag.CallStatus = new SelectList(dropdown.BindCall_Status_Master(), "Value", "Text");
                var result = con.Query<ReverseAWB_AllocationModel>("GetDataIn_Reverse_AWB_Allocation",
                        new { CC_NO = CC_NO }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                var Problem = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",
                   new { CC_NO }, commandType: CommandType.StoredProcedure).ToList();
                var finalValue = "";
                foreach (var item in Problem)
                {
                    var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item.ProblemId }, commandType: CommandType.Text).FirstOrDefault();

                    finalValue =finalValue+ " , "+result1;
                }
                finalValue = finalValue.Trim().TrimStart(',');
                result.Problem = finalValue;
                result.CallStatus = result.CallStatus;
                return View(result);
            }

        }
        [HttpPost]
        public ActionResult PFRAWBAForm(ReverseAWB_AllocationModel m)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("Insert_Into_Reverse_AWB_Allocation"
                   , new
                   {

                       m.CC_NO,
                       ServiceEngineerAction = m.SE_Action,
                       m.ReversePickupDate,
                       CourierId = m.CourierName,
                       MessageToCustomer = m.MessageCusto,
                       Remarks = m.SE_Remarks,
                   },
                   commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 1)
                {
                    TempData["Message"] = "Updated Successfully";
                }

                return RedirectToAction("Reverse_AWB_Allocation");
            }

        }
        public ActionResult UAWBSB()
        {
            ViewBag.ServiceProviderName = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ReverseAWB_AllocationModel>("getDataInRewerse_Awb_Allocation", null, commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }
        }
        public ActionResult UAWBSBForm()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                //ViewBag.ServiceProviderName = new SelectList(dropdown.BindServiceProvider(), "Value", "Text");
                //ViewBag.CourierName = new SelectList(dropdown.BindCourier(), "Value", "Text");
                //ViewBag.CallStatus = new SelectList(dropdown.BindStatusMaster(), "Value", "Text");
                //var result = con.Query<ReverseAWB_AllocationModel>("GetDataIn_Reverse_AWB_Allocation",
                //        new { CC_NO = CC_NO }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                //var Problem = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",
                //   new { CC_NO }, commandType: CommandType.StoredProcedure).ToList();
                //var finalValue = "";
                //foreach (var item in Problem)
                //{
                //    var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item.ProblemId }, commandType: CommandType.Text).FirstOrDefault();

                //    finalValue = string.Join(",", result1);
                //}
                //result.Problem = finalValue;
                return View();
            }
        }
        public ActionResult URSSE()
        {
            ViewBag.ServiceProviderName = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            using (var con = new SqlConnection(_connectionString))
            {
                //var result = con.Query<ReverseAWB_AllocationModel>("getDataInRewerse_Awb_Allocation", null, commandType: CommandType.StoredProcedure).ToList();
                var result = con.Query<MainTableURSSE>("URSSE_pendingCases", null, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
        }
        public ActionResult URSSEForm()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ViewBag.CallStatus = new SelectList(dropdown.BindCall_Status_Master(), "Value", "Text");
            ViewBag.CourierName = new SelectList(dropdown.BindCourier(), "Value", "Text");
            ViewBag.EngineerName = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            using (var con = new SqlConnection(_connectionString))
            {
                var result = new URSSEModel();
                var result1 = con.Query<URSSE_Page_Model>("URSSE_Page_Single_Table", null, commandType: CommandType.StoredProcedure).ToList();
                result.URSSE_Table = result1;
                return View(result);
               
            }
        }
        public ActionResult SingleURSSEForm(string CC_NO)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetDataByCCNO",
                   new { CC_NO = CC_NO }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                var problem = "";
                if (result.PrblmObsrvd != null)
                {
                    foreach (var item in result.PrblmObsrvd)
                    {
                        if (item != ',')
                        {
                            var result1 = con.Query<string>("select ProblemObserved from MstProblemObserved WHERE ProblemId =@ProblemId ", new { @ProblemId = item }, commandType: CommandType.Text).FirstOrDefault();
                            problem = problem + result1 + " , ";
                        }
                    }
                    result.PrblmObsrvd = problem;
                }
                var Problem = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",
                  new { CC_NO }, commandType: CommandType.StoredProcedure).ToList();
                var finalValue = "";
                foreach (var item in Problem)
                {
                    var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item.ProblemId }, commandType: CommandType.Text).FirstOrDefault();
                   
                    item.ProblemName = result1;
                    finalValue = finalValue + " , " + result1;
                }
                finalValue = finalValue.Trim().TrimStart(',');
                result.Problem = finalValue;
                if (Problem != null)
                {
                    result.ChildtableDataProblem = Problem;
                }
                else
                {
                    new List<GetProblem_Child_Order_problem>();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
                
        }
        [HttpPost]
        public ActionResult URSSE_Child_Form(URSSEModel m)
        {
            if (ModelState.IsValid)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Insert_into_URSSE_Page_transactions "
                        , new {
                            CC_NO=m.CcNo,
                            SerialNumber= m.Serial_No,
                            m.IMEI1,
                            m.IMEI2,
                            m.RepairStatus,
                            m.TotalCost,
                            m.CollectableAmount,
                            m.PaymentMode,
                            m.CashReceived,
                            m.TransactionAmount,
                            m.TransactionDateTime,
                            m.TransactionNumber,
                            m.ReVisitDateTime
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {
                        TempData["Message"] = "Updated Successfully";
                    }
                }
            }
            return RedirectToAction("URSSEForm", "Logistics");
        }
    }
}