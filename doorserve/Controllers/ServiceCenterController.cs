﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using doorserve.Models;
using doorserve.Permission;
using doorserve.Repository.ServiceCenters;
using doorserve.Models.ServiceCenter;
using AutoMapper;
using doorserve.Repository;
using doorserve.Filters;
using System.IO;
using Newtonsoft.Json;

namespace doorserve.Controllers
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
            {
                calls.Employee.EmployeeList = new SelectList(await CommonModel.GetEmployeeList(SessionModel.RefKey), "Name", "Text");
                calls.IsAscOrAsp = true;
            }
            else if (SessionModel.UserRole.Contains("Service Provider SC Admin"))
            {
                calls.Employee.EmployeeList = new SelectList(await CommonModel.GetEmployeeByProvider(SessionModel.RefKey), "Name", "Text");
                calls.IsAscOrAsp = true;
            }
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
                callStatus.RefKey = SessionModel.RefKey;
                callStatus.CompId = SessionModel.CompanyId;
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
            CallDetailsModel.ProductList = new SelectList(_dropdown.BindProduct(CallDetailsModel.DeviceBrandId.ToString()+","+ CallDetailsModel.DeviceSubCategoryId.ToString()), "Value", "Text");
            CallDetailsModel.ServiceTypeList = new SelectList( await CommonModel.GetServiceType(SessionModel.CompanyId),"Value","Text");
            CallDetailsModel.DeliveryTypeList = new SelectList(await CommonModel.GetDeliveryServiceType(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.CustomerTypeList = new SelectList(await CommonModel.GetLookup("Customer Type"), "Value", "Text");
            CallDetailsModel.ConditionList = new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text");
            CallDetailsModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "Value", "Text");
            CallDetailsModel.LocationList = new SelectList(dropdown.BindLocationByPinCode(CallDetailsModel.PinNumber), "Value", "Text");
            var providerList = dropdown.BindServiceProvider(CallDetailsModel.PinNumber);
            CallDetailsModel.CompLogo = SessionModel.LogoUrl;

            if (Convert.ToBoolean(CallDetailsModel.IsRepeat))
            {
                var prvList = providerList.Where(x => x.Value == CallDetailsModel.PrvProviderId.ToString()).ToList();
                if (prvList !=null)
                    CallDetailsModel.ProviderList = new SelectList(prvList, "Value", "Text");
                else
                    CallDetailsModel.ProviderList = new SelectList(dropdown.BindServiceProvider(CallDetailsModel.PinNumber), "Value", "Text");

            }
            else
            CallDetailsModel.ProviderList = new SelectList(dropdown.BindServiceProvider(CallDetailsModel.PinNumber), "Value", "Text");
            CallDetailsModel.Param = Param;
            CallDetailsModel.Files = new List<ProviderFileModel>();
            if (Param == "A")
            {
                if (CallDetailsModel.EmpId != null)
                    CallDetailsModel.Employee = await _empRepo.GetEmployeeById(CallDetailsModel.EmpId);
                else
                    CallDetailsModel.Employee = new EmployeeModel();
                CallDetailsModel.StatusList = new SelectList(dropdown.BindCallAppointmentStatus("ASP"), "Value", "Text");
                CallDetailsModel.AppointmentStatus = CallDetailsModel.ASPStatus;
                 CallDetailsModel.Remarks = CallDetailsModel.AspRemark;          
            }       
           else
            {
                if (Param == "P")
                    CallDetailsModel.CStatus = 11;
                CallDetailsModel.AppointmentStatus = CallDetailsModel.CStatus;
                CallDetailsModel.StatusList = new SelectList(dropdown.BindCallAppointmentStatus("Customer support"), "Value", "Text");
                CallDetailsModel.Remarks = CallDetailsModel.CRemark;
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
                return RedirectToAction("AcceptCalls");
            }
            catch (Exception ex)
            {
                var response = new ResponseModel { Response = ex.Message, IsSuccess = false };
                TempData["response"] = response;
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
        public async Task<ActionResult> UpdateCall(CallStatusDetailsModel callStatusDetails)
        {          
            if(callStatusDetails.AppointmentDate == null && callStatusDetails.Param ==null)
            {        
                var call = Request.Params["CallDetail"];
                callStatusDetails = JsonConvert.DeserializeObject<CallStatusDetailsModel>(call);
                callStatusDetails.InvoiceFile = Request.Files["InvoiceFile"];
                callStatusDetails.JobSheetFile = Request.Files["JobSheetFile"];
                callStatusDetails.Type = "A";
                string directory = "~/TempFiles/";
                string path = Server.MapPath(directory + callStatusDetails.DeviceId);
                if (callStatusDetails.InvoiceFile != null || callStatusDetails.JobSheetFile != null || callStatusDetails.InvoiceFile !=null)
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                foreach (var part in callStatusDetails.Parts)
                {
                    part.PartFile = Request.Files[part.PartNo];
                    if (part.PartFile != null)
                    {
                        if (part.PartId != null)
                            part.Action = 'I';
                        part.FileName = part.PartNo + Path.GetExtension(Path.Combine(directory, part.PartFile.FileName));
                        if (System.IO.File.Exists(path+ "/" + part.FileName))
                            System.IO.File.Delete(path + "/" + part.FileName);
                           part.PartFile.SaveAs(path + "/" + part.FileName);
                        part.PartFile = null;
                    }
                }
                
                if (callStatusDetails.InvoiceFile != null)
                {
                    callStatusDetails.InvoiceFileName = "DevicePic" + Path.GetExtension(Path.Combine(directory, callStatusDetails.InvoiceFile.FileName));
                    if (System.IO.File.Exists(directory + callStatusDetails.DeviceId + "/"+ callStatusDetails.InvoiceFileName))
                        System.IO.File.Delete(directory + callStatusDetails.DeviceId + "/"+callStatusDetails.InvoiceFileName);
                    if (callStatusDetails.InvoiceFile != null && callStatusDetails.InvoiceFile.ContentLength > 0)                       
                        callStatusDetails.InvoiceFile.SaveAs(path + "/" + callStatusDetails.InvoiceFileName);            
                }
                if(callStatusDetails.JobSheetFile!=null)
                {
                    callStatusDetails.JobSheetFileName = "JobSheet" + Path.GetExtension(Path.Combine(directory, callStatusDetails.JobSheetFile.FileName));

                    if (System.IO.File.Exists(directory + callStatusDetails.DeviceId + "/" + callStatusDetails.JobSheetFileName))
                        System.IO.File.Delete(directory + callStatusDetails.DeviceId + "/" + callStatusDetails.JobSheetFileName);
                    if (callStatusDetails.JobSheetFile != null && callStatusDetails.JobSheetFile.ContentLength > 0)                     
                        callStatusDetails.InvoiceFile.SaveAs(path + "/"+callStatusDetails.JobSheetFileName);
                }
          
            }

            if (callStatusDetails.Param == "A")
            {
                callStatusDetails.Type = "A";
                callStatusDetails.IsServiceApproved = null;
            }
            else
                callStatusDetails.Type = "C";

            if (callStatusDetails.Param == "AP")
            {
                callStatusDetails.Type = "AP";
            }
            if (callStatusDetails.Param == "CL")
            {
                callStatusDetails.Type = "CL";
                callStatusDetails.AppointmentStatus = 12;
            }
            var SessionModel = Session["User"] as SessionModel;
            callStatusDetails.UserId = SessionModel.UserId;
            var response = await _centerRepo.UpdateCallCenterCall(callStatusDetails);
                TempData["response"] = response;
            if(callStatusDetails.Type == "C")
                return RedirectToAction("index", "PendingCalls");
            else if(callStatusDetails.Type == "A")
            return RedirectToAction("AcceptCalls");  
            else
                return RedirectToAction("EscalateCalls","PendingCalls");
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

        [HttpPost]
        public JsonResult UploadFile()
        {
            string directory = "~/TempFiles/";
            HttpPostedFileBase file = Request.Files["file"];
            var  DeviceId= Request.Params["DeviceId"];
            var PartNo = Request.Params["PartNo"];

            if (System.IO.File.Exists(directory + DeviceId + "/" + PartNo+ Path.GetExtension(Path.Combine(directory, file.FileName))))
                System.IO.File.Delete(directory + DeviceId + "/" + PartNo + Path.GetExtension(Path.Combine(directory, file.FileName)));
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                string path = Server.MapPath(directory + DeviceId);
                if (!Directory.Exists(path))     
                    Directory.CreateDirectory(path);           

                file.SaveAs(path + "/"+PartNo+ Path.GetExtension(Path.Combine(directory, file.FileName)));
            }
            return Json( PartNo + Path.GetExtension(Path.Combine(directory, file.FileName)), JsonRequestBehavior.AllowGet);
        }
    }
    
}