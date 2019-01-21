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
    public class ServiceCenterController : Controller
    {
        private readonly string _connectionString =
           ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // GET: ServiceCenter
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
            ViewBag.CourierName = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }
        public ActionResult FindPFP()
        {
            return View();
        }
        public ActionResult PFPForm()
        {
            ViewBag.ReceivedDevice = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ViewBag.RecvdBrand = new SelectList(dropdown.BindBrand(), "Value", "Text");
            ViewBag.RecvdModel = new SelectList(dropdown.BindProduct(), "Value", "Text");
            ViewBag.Engg_Name = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            ViewBag.SpareType = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(dropdown.BindProblemObserved(), "Value", "Text");
            ViewBag.QCPersonName = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            ViewBag.QCFailReason = new SelectList(dropdown.BindQC(), "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult PFPForm(ReceiveMaterials m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var con = new SqlConnection(_connectionString))
                    {
                        var result = con.Query<int>("Insert_into_Pending_For_Packing",
                            new
                            {
                                m.CC_NO,
                                m.Materials_packed,
                                m.Is_Repaking,
                                m.Number_of_Times_RePacked,
                                m.JOB_Date,
                                m.Is_QUTrust_Certificate_Printed,
                                m.Is_Functionality_Report_Printed,
                                m.Notice_Number_and_Level,
                                m.Packaging_Material,
                                m.Length,
                                m.Height,
                                Purpose = m.PurPose,
                                m.Previous_AWB_Number,
                                m.Previous_Courier_Name,
                                JOBNumber = m.JobNumber,
                                m.Packer_Name,
                                m.Packer_ID,
                                m.Packaging_Material_Size,
                                m.Width,
                                m.Weight,
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 1)
                        {
                            TempData["Message"] = "Successfully Added";
                        }
                        else
                        {
                            TempData["Message"] = "Something Went Wrong";
                        }

                    }
                }
                
                return RedirectToAction("index","ServiceCenter");
            }
            catch (Exception)
            {
                return RedirectToAction("index", "ErrorPage");
               
            }
           
        }
        public ActionResult TablePFP()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForPendingPacking",
                   new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }

        }
        public ActionResult PFBI()
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
        public ActionResult FindPFBI()
        {
            return View();
        }
        public ActionResult PFBIForm()
        {
            ViewBag.ReceivedDevice = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ViewBag.RecvdBrand = new SelectList(dropdown.BindBrand(), "Value", "Text");
            ViewBag.RecvdModel = new SelectList(dropdown.BindProduct(), "Value", "Text");
            ViewBag.Engg_Name = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            ViewBag.SpareType = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(dropdown.BindProblemObserved(), "Value", "Text");
            ViewBag.QCPersonName = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            ViewBag.QCFailReason = new SelectList(dropdown.BindQC(), "Value", "Text");
            ViewBag.CourierID = new SelectList(dropdown.BindCourier(), "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult PFBIForm(ReceiveMaterials m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var con = new SqlConnection(_connectionString))
                    {
                        var result = con.Query<int>("Insert_Into_Billing_Invoicing_Information",
                            new
                            {
                                m.CC_NO,
                                m.Device_Shipping_Attempt_Number,
                                m.Previous_Shipping_Date,
                                m.Bill_Invoice_Number,
                                m.Service_Charge,
                                m.Advance_Amount_Paid,
                                m.Due_Amount,
                                m.Bill_Invoice_Amount,
                                m.CourierID,
                                m.Material_Type,
                                m.Notice_Shipping_Attempt_Number,
                                m.Previous_AWB_Status,
                                m.Bill_Invoice_Date,
                                m.Spare_Parts_Cost,
                                m.Advance_Payment_Date,
                                m.Collectable_Amount,
                                m.Schedule_Courier_Pickup_Date,
                                m.Courier_Type
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 1)
                        {
                            TempData["Message"] = "Successfully Added";
                        }
                        else
                        {
                            TempData["Message"] = "Something Went Wrong";
                        }

                    }
                }

                return RedirectToAction("index", "ServiceCenter");
            }
            catch (Exception e)
            {
                return RedirectToAction("index", "ErrorPage");

            }
        }
        public ActionResult TablePFBI()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForPendingBilling",
                   new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }

        }
        public ActionResult PrintShippingLabel( string CC_NO )
        {
           
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("Get_Data_For_PrintShippingLabel", new { @CC_NO = CC_NO }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return View(result);
            }
           
        }
        public ActionResult Re_Print_Invoice_Bill()
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
        public ActionResult Find_Re_Print_Invoice_Bill()
        {
            return View();
        }
        public ActionResult Form_Re_Print_Invoice_Bill()
        {
            ViewBag.ReceivedDevice = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ViewBag.RecvdBrand = new SelectList(dropdown.BindBrand(), "Value", "Text");
            ViewBag.RecvdModel = new SelectList(dropdown.BindProduct(), "Value", "Text");
            ViewBag.Engg_Name = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            ViewBag.SpareType = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(dropdown.BindProblemObserved(), "Value", "Text");
            ViewBag.QCPersonName = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            ViewBag.QCFailReason = new SelectList(dropdown.BindQC(), "Value", "Text");
            ViewBag.CourierID = new SelectList(dropdown.BindCourier(), "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult Form_Re_Print_Invoice_Bill(ReceiveMaterials m)
        {
            return View();
        }
        public ActionResult TableForm_Re_Print_Invoice_Bill()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForAllPages",
                   new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }

        }
    }
}