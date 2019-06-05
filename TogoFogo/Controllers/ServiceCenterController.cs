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
using TogoFogo.Models.ServiceCenter;
using AutoMapper;
using TogoFogo.Repository;
using TogoFogo.Filters;

namespace TogoFogo.Controllers
{
    public class ServiceCenterController : Controller
    {
        private readonly ICenter _centerRepo;
        private readonly IEmployee _empRepo;
        private readonly DropdownBindController _dropdown;

        public ServiceCenterController()
        {

            _centerRepo = new Center();
            _dropdown = new DropdownBindController();
            _empRepo = new Employee();
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
        public ActionResult PFPForm(ReceiveMaterials m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var con = new SqlConnection(_connectionString))
                    {
                        var SessionModel = Session["User"] as SessionModel;
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
                                SessionModel.CompanyId
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
                var SessionModel = Session["User"] as SessionModel;
                var result = con.Query<AllData>("GetTableDataForPendingPacking",
                   new { SessionModel.CompanyId }, commandType: CommandType.StoredProcedure).ToList();
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
            var SessionModel = Session["User"] as SessionModel;
            ViewBag.ReceivedDevice = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            ViewBag.RecvdBrand = new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            ViewBag.RecvdModel = new SelectList(dropdown.BindProduct(SessionModel.CompanyId), "Value", "Text");
            ViewBag.Engg_Name = new SelectList(dropdown.BindEngineer(SessionModel.CompanyId), "Value", "Text");
            ViewBag.SpareType = new SelectList(dropdown.BindSpareType(SessionModel.CompanyId), "Value", "Text");
            ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(dropdown.BindProblemObserved(SessionModel.CompanyId), "Value", "Text");
            ViewBag.QCPersonName = new SelectList(dropdown.BindEngineer(SessionModel.CompanyId), "Value", "Text");
            ViewBag.QCFailReason = new SelectList(dropdown.BindQC(SessionModel.CompanyId), "Value", "Text");
            ViewBag.CourierID = new SelectList(dropdown.BindCourier(SessionModel.CompanyId), "Value", "Text");
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
                        var SessionModel = Session["User"] as SessionModel;
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
                                m.Courier_Type,
                                SessionModel.CompanyId
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
            var session = Session["User"] as SessionModel;
            ViewBag.ReceivedDevice = new SelectList(dropdown.BindCategory(session.CompanyId), "Value", "Text");
            ViewBag.RecvdBrand = new SelectList(dropdown.BindBrand(session.CompanyId), "Value", "Text");
            ViewBag.RecvdModel = new SelectList(dropdown.BindProduct(session.CompanyId), "Value", "Text");
            ViewBag.Engg_Name = new SelectList(dropdown.BindEngineer(session.CompanyId), "Value", "Text");
            ViewBag.SpareType = new SelectList(dropdown.BindSpareType(session.CompanyId), "Value", "Text");
            ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(dropdown.BindProblemObserved(session.CompanyId), "Value", "Text");
            ViewBag.QCPersonName = new SelectList(dropdown.BindEngineer(session.CompanyId), "Value", "Text");
            ViewBag.QCFailReason = new SelectList(dropdown.BindQC(session.CompanyId), "Value", "Text");
            ViewBag.CourierID = new SelectList(dropdown.BindCourier(session.CompanyId), "Value", "Text");
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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Open_Calls)]
        public async Task<ActionResult> AcceptCalls()
        {
            var SessionModel = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = SessionModel.CompanyId,IsExport=false};
            if (SessionModel.UserRole.Contains("Service Provider SC Admin"))
                filter.ProviderId = SessionModel.RefKey;
            if (SessionModel.UserTypeName.ToLower().Contains("center"))
                filter.RefKey = SessionModel.RefKey;

            var calls = await _centerRepo.GetCallDetails(filter);
            calls.Employee = new EmployeeModel();        
            if (SessionModel.UserTypeName.ToLower().Contains("center"))
            calls.Employee.EmployeeList = new SelectList(await CommonModel.GetEmployeeList(SessionModel.RefKey), "Name", "Text");
          else  if(SessionModel.UserRole.Contains("Service Provider SC Admin"))
                calls.Employee.EmployeeList = new SelectList(await CommonModel.GetEmployeeByProvider(SessionModel.RefKey), "Name", "Text");
            else
                calls.Employee.EmployeeList = new SelectList(await CommonModel.GetEmployeeListByCompany(SessionModel.CompanyId), "Name", "Text");


            return View(calls);
        }
      

        public async Task<ActionResult> TechnicianDetails(Guid EmpId)
        {

            var techDetails = await _empRepo.GetEmployeeById(EmpId);
            
            return Json(techDetails, JsonRequestBehavior.AllowGet);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Open_Calls)]
        [HttpPost]
        public async Task<ActionResult> CallStatus(CallStatusModel callStatus)
        {

            try
            {
                var SessionModel = Session["User"] as SessionModel;
                callStatus.UserId = SessionModel.UserId;
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

        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Open_Calls)]
        [HttpPost]
        public async Task<ActionResult> AssignCalls(EmployeeModel assignCall)
        {

            try
            {
                var SessionModel = Session["User"] as SessionModel;
                assignCall.UserId = SessionModel.UserId;
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
        //GetCallDetailByID
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Open_Calls)]
        public async Task<ActionResult> ManageServiceProvidersDetails(string CRN, string Param)
        {
            var SessionModel = Session["User"] as SessionModel;
            var CallDetailsModel = await _centerRepo.GetCallsDetailsById(CRN);
            CallDetailsModel.BrandList = new SelectList(_dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.CategoryList = new SelectList(_dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.SubCategoryList = new SelectList(_dropdown.BindSubCategory(CallDetailsModel.DeviceCategoryId), "Value", "Text");
            CallDetailsModel.ProductList = new SelectList(_dropdown.BindProduct(CallDetailsModel.DeviceBrandId), "Value", "Text");
            CallDetailsModel.ServiceTypeList = new SelectList( await CommonModel.GetServiceType(SessionModel.CompanyId),"Value","Text");
            CallDetailsModel.DeliveryTypeList = new SelectList(await CommonModel.GetDeliveryServiceType(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.CustomerTypeList = new SelectList(await CommonModel.GetLookup("Customer Type"), "Value", "Text");
            CallDetailsModel.ConditionList = new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text");
            CallDetailsModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "Value", "Text");
            CallDetailsModel.CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text");
            CallDetailsModel.StateList = new SelectList(dropdown.BindState(CallDetailsModel.CountryId), "Value", "Text");
            CallDetailsModel.CityList = new SelectList(dropdown.BindLocation(CallDetailsModel.StateId), "Value", "Text");
            CallDetailsModel.Param = Param;
            CallDetailsModel.StatusId = 11;
            if (Param == "A")
            {
                CallDetailsModel.Employee = new EmployeeModel();

                if (SessionModel.UserTypeName.ToLower().Contains("center"))
                    CallDetailsModel.Employee.EmployeeList = new SelectList(await CommonModel.GetEmployeeList(SessionModel.RefKey), "Name", "Text");
                //var list = await CommonModel.GetEmployeeList(user.CompanyId);
                else if (SessionModel.UserRole.Contains("Service Provider SC Admin"))
                    CallDetailsModel.Employee.EmployeeList = new SelectList(await CommonModel.GetEmployeeByProvider(SessionModel.RefKey), "Name", "Text");
                else
                CallDetailsModel.Employee.EmployeeList = new SelectList(await CommonModel.GetEmployeeListByCompany(SessionModel.CompanyId), "Name", "Text");
                

            }

            return View(CallDetailsModel);
        }


        //
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Open_Calls)]
        [HttpPost]
        public async Task<ActionResult> ManageServiceProvidersDetails(CallStatusDetailsModel callStatusDetails)
        {
            try
            {
                var SessionModel = Session["User"] as SessionModel;

                callStatusDetails.UserId = SessionModel.UserId;
                var response = await _centerRepo.UpdateCallsStatusDetails(callStatusDetails);
                TempData["response"] = response;
                //return Json("Ok", JsonRequestBehavior.AllowGet);
                return RedirectToAction("AcceptCalls");
            }
            catch (Exception ex)
            {
                var response = new ResponseModel { Response = ex.Message, IsSuccess = false };
                TempData["response"] = response;
                TempData.Keep("response");
                //return Json("ex", JsonRequestBehavior.AllowGet);
                return RedirectToAction("AcceptCalls");
            }

        }


        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Open_Calls)]
        [HttpPost]
        public async Task<ActionResult> CallStatusDetails(CallStatusDetailsModel callStatusDetails)
        {
            try
            {
                var SessionModel = Session["User"] as SessionModel;
                callStatusDetails.UserId = SessionModel.UserId;
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Open_Calls)]
        [HttpPost]
        public async Task<ActionResult> SavetechnicianDetails(CallStatusDetailsModel callStatusDetails)
        {
            var SessionModel = Session["User"] as SessionModel;

            callStatusDetails.UserId = SessionModel.UserId;
                var response = await _centerRepo.SaveTechnicianDetails(callStatusDetails);
                TempData["response"] = response;
            return RedirectToAction("AcceptCalls");            

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.ExcelExport }, (int)MenuCode.Open_Calls)]
        [HttpGet]
        public async Task<FileContentResult> ExportToExcel(char tabIndex)
        {
            var SessionModel = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = SessionModel.CompanyId, tabIndex = tabIndex, IsExport = true };

            var response = await _centerRepo.GetCallDetails(filter);
            byte[] filecontent;
            string[] columns;
            if (tabIndex == 'P')
            {
                columns = new string[]{"CRN","ClientName","CreatedOn","ServiceTypeName","CustomerName","CustomerContactNumber",
                    "CustomerEmail","CustomerAddress","CustomerCity","CustomerPincode","DeviceCategory","DeviceBrand",
                    "DeviceModel","DOP","PurchaseFrom","ServiceCenterName"};
                filecontent = ExcelExportHelper.ExportExcel(response.PendingCalls, "", true, columns);

            }
            else if (tabIndex == 'A')
            {
                columns = new string[]{"CRN","ClientName","CreatedOn","ServiceTypeName","CustomerName","CustomerContactNumber",
                    "CustomerEmail","CustomerAddress","CustomerCity","CustomerPincode","DeviceCategory","DeviceBrand",
                    "DeviceModel","DOP","PurchaseFrom","ServiceCenterName"};
                filecontent = ExcelExportHelper.ExportExcel(response.AcceptedCalls, "", true, columns);
            }
            else
            {
                columns = new string[]{"CRN","ClientName","CreatedOn","ServiceTypeName","CustomerName","CustomerContactNumber",
                    "CustomerEmail","CustomerAddress","CustomerCity","CustomerPincode","DeviceCategory","DeviceBrand",
                    "DeviceModel","DOP","PurchaseFrom","ProviderName","ServiceCenterName"};
                filecontent = ExcelExportHelper.ExportExcel(response.AssignedCalls, "", true, columns);
            }

            return File(filecontent, ExcelExportHelper.ExcelContentType, "Excel.xlsx");

        }

       
        [HttpPost]
        public async Task<ActionResult> EditAppointment(CallDetailsModel Appointment)
        {
            try
            {
                var response = await _centerRepo.EditCallAppointment(Appointment);
                TempData["response"] = response;
                return RedirectToAction("AcceptCalls");
            }
            catch (Exception ex)
            {
                var response = new ResponseModel { Response = ex.Message, IsSuccess = false };
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("AcceptCalls");

            }


        }
    }
    
}