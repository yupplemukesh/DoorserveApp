using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Permission;
using TogoFogo.Repository.ServiceCenters;
using GridMvc.Html;
using TogoFogo.Models.Customer_Support;
using TogoFogo.Models.ServiceCenter;
using AutoMapper;

namespace TogoFogo.Controllers
{
    public class ServiceCenterController : Controller
    {
        private readonly ICenter _centerRepo;
        private readonly DropdownBindController _dropdown;
        public ServiceCenterController()
        {

            _centerRepo = new Center();
            _dropdown = new DropdownBindController();
        }
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

                return RedirectToAction("index", "ServiceCenter");
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
        public ActionResult PrintShippingLabel(string CC_NO)
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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Calls Details")]
        public async Task<ActionResult> AcceptCalls()

        {
            var calls = await _centerRepo.GetCallDetails();

            calls.CallDetails = new Models.ServiceCenter.CallDetailsModel();
            calls.employee = new EmployeeModel();
            calls.CallDetails.StatusList = new SelectList(await CommonModel.GetStatusTypes(), "Value", "Text");
            calls.employee.EmployeeList = new SelectList(await CommonModel.GetEmployeeList(), "Value", "Text");
            Guid? providerId = null;
            if (Session["RoleName"].ToString().Contains("provider"))
                providerId = await CommonModel.GetProviderIdByUser(Convert.ToInt32(Session["User_ID"]));

            return View(calls);
        }
        //public async Task<ActionResult> GetEmployeeDetailsById(int EmpId)
        //{
        //    EmployeeModel emp = new EmployeeModel();
        //    emp =  await CommonModel.GetEmployeeDetailById(EmpId);
        //    return Json(emp, JsonRequestBehavior.AllowGet);


        //}

        [HttpPost]
        public async Task<ActionResult> TechnicianDetails(string EmpId)
        {

            var techDetails = await _centerRepo.GetTechnicianDetails(EmpId);
            var techDetailsModel = Mapper.Map<EmployeeModel>(techDetails);            
            return Json(techDetailsModel, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public async Task<ActionResult> CallStatus(CallStatusModel callStatus)
        {

            try
            {

                callStatus.UserId = Convert.ToInt32(Session["User_ID"]);
                var response = await _centerRepo.UpdateCallsStatus(callStatus);
                TempData["response"] = response;
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel { Response = ex.Message, IsSuccess = false };
                TempData["response"] = response;
                TempData.Keep("response");
                return Json("ex", JsonRequestBehavior.AllowGet);
            }

        }

        //[HttpPost]
        //public async Task<ActionResult> AssignCalls(AssignCallsModel assignCall)
        //{

        //    try
        //    {

        //        assignCall.UserId = Convert.ToInt32(Session["User_ID"]);
        //        var response = await _centerRepo.AssignCallsDetails(assignCall);
        //        TempData["response"] = response;
        //        return Json("Ok", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new ResponseModel { Response = ex.Message, IsSuccess = false };
        //        TempData["response"] = response;
        //        TempData.Keep("response");
        //        return Json("ex", JsonRequestBehavior.AllowGet);
        //    }

        //}
        [HttpPost]
        public async Task<ActionResult> AssignCalls(EmployeeModel assignCall)
        {

            try
            {

                assignCall.UserID = Convert.ToInt32(Session["User_ID"]);
                var response = await _centerRepo.AssignCallsDetails(assignCall);
                TempData["response"] = response;
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel { Response = ex.Message, IsSuccess = false };
                TempData["response"] = response;
                TempData.Keep("response");
                return Json("ex", JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> ManageServiceProvidersDetails(string CRN)
        {
            var callDetails = await _centerRepo.GetCallsDetailsById(CRN);
            var callDetailsModel = Mapper.Map<CallDetailsModel>(callDetails);
            callDetailsModel.BrandList = new SelectList(_dropdown.BindBrand(), "Value", "Text");
            callDetailsModel.CategoryList = new SelectList(_dropdown.BindCategory(), "Value", "Text");
            callDetailsModel.ProductList = new SelectList(Enumerable.Empty<SelectListItem>());
            callDetailsModel.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(), "Value", "Text");
            callDetailsModel.DeliveryTypeList = new SelectList(await CommonModel.GetDeliveryServiceType(), "Value", "Text");
            callDetailsModel.CustomerTypeList = new SelectList(await CommonModel.GetLookup("Customer Type"), "Value", "Text");
            callDetailsModel.ConditionList = new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text");
            callDetailsModel.address = new AddressDetail
            {
                AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text"),
                CityList = new SelectList(Enumerable.Empty<SelectListItem>()),
                StateList = new SelectList(Enumerable.Empty<SelectListItem>()),
                CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text"),
            };
            return View(callDetailsModel);
        }
        [HttpPost]
        public async Task<ActionResult> CallStatusDetails(CallStatusDetailsModel callStatusDetails)
        {

            try
            {

                callStatusDetails.UserId = Convert.ToInt32(Session["User_ID"]);
                var response = await _centerRepo.UpdateCallsStatusDetails(callStatusDetails);
                TempData["response"] = response;
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel { Response = ex.Message, IsSuccess = false };
                TempData["response"] = response;
                TempData.Keep("response");
                return Json("ex", JsonRequestBehavior.AllowGet);
            }

        }
    }
}