using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.Customer_Support;
using doorserve.Permission;
using doorserve.Repository.Customer_Support;
using doorserve.Repository;
using doorserve.Models.ServiceCenter;
using AutoMapper;
using doorserve.Models.ClientData;

namespace doorserve.Controllers
{
    public class PendingCallsController : Controller
    {
        private readonly ICustomerSupport _customerSupport;
        private readonly ICallLog _RepoCallLog;
        private readonly DropdownBindController _dropdown;
        public PendingCallsController()
        {

            _customerSupport = new CustomerSupport();
            _RepoCallLog = new CallLog();
            _dropdown = new DropdownBindController();
        }
        // GET: CallToASP
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Call_Allocate_To_ASP)]
        public async Task<ActionResult> Index()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel {CompId= session.CompanyId,IsExport=false};
            var calls = await _customerSupport.GetASPCalls(filter);
            calls.ClientList = new SelectList(await CommonModel.GetClientData(session.CompanyId), "Name", "Text");
            calls.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(session.CompanyId), "Value", "Text");
            calls.CallAllocate = new Models.Customer_Support.AllocateCallModel { ToAllocateList=new SelectList(await CommonModel.GetServiceProviders(session.CompanyId),"Name","Text") };
            return View(calls);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Call_Allocate_To_ASP)]
        [HttpPost]
        public async Task<ActionResult> Allocate(AllocateCallModel allocate)
        {

            try
            {
                var SessionModel = Session["User"] as SessionModel;
                allocate.AllocateTo = "ASP";
                allocate.UserId = SessionModel.UserId;
                 var response = await _customerSupport.AllocateCall(allocate);
                TempData["response"] = response;
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel { Response = ex.Message, IsSuccess = false };
                TempData["response"] = response;
                return Json("ex", JsonRequestBehavior.AllowGet);
            }
          
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.ExcelExport }, (int)MenuCode.Call_Allocate_To_ASP)]
        [HttpGet]
        public async Task<FileContentResult> ExportToExcel(char tabIndex)
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel
            {
                CompId = session.CompanyId
                ,
                tabIndex = tabIndex,
                UserId = session.UserId,
                Type = tabIndex               
            };
            var response = await _customerSupport.GetASPCalls(filter);
            var OtherRes =   new List<UploadedExcelModel>() ;
            filter.Type = tabIndex;
            if (tabIndex != 'P')
                OtherRes = await _RepoCallLog.GetExclatedCalls(filter);
   
            byte[] filecontent;
            string[] columns;
            if (tabIndex == 'P')
            {
                columns = new string[]{ "CRN","ClientName", "CreatedOn", "ServiceTypeName", "CustomerName","CustomerContactNuber","CustomerEmail",
                                "CustomerAddress","CustomerCity","CustomerPinCode","DeviceCategory",
                                 "DeviceBrand","DeviceModel","DOP","DevicePurchaseFrom"};
                filecontent = ExcelExportHelper.ExportExcel(response.PendingCalls, "", true, columns);
            }
            else if (tabIndex == 'A')
            {
       
                columns = new string[]{ "CRN","ClientName", "CreatedOn", "ServiceTypeName", "CustomerName","CustomerContactNuber","CustomerEmail",
                                "CustomerAddress","CustomerCity","CustomerPinCode","DeviceCategory",
                                 "DeviceBrand","DeviceModel","DOP","DevicePurchaseFrom"};
                filecontent = ExcelExportHelper.ExportExcel(OtherRes, "", true, columns);
            }
            else if (tabIndex == 'E')
            {
                columns = new string[]{ "CRN","ClientName", "CreatedOn", "ServiceTypeName", "CustomerName","CustomerContactNuber","CustomerEmail",
                                "CustomerAddress","CustomerCity","CustomerPinCode","DeviceCategory",
                                 "DeviceBrand","DeviceModel","DOP","DevicePurchaseFrom"};
                filecontent = ExcelExportHelper.ExportExcel(OtherRes, "", true, columns);
            }
            else if (tabIndex == 'C')
            {
                columns = new string[]{ "CRN","ClientName", "CreatedOn", "ServiceTypeName", "CustomerName","CustomerContactNuber","CustomerEmail",
                                "CustomerAddress","CustomerCity","CustomerPinCode","DeviceCategory",
                                 "DeviceBrand","DeviceModel","DOP","DevicePurchaseFrom"};
                filecontent = ExcelExportHelper.ExportExcel(OtherRes, "", true, columns);
            }
            else
            {
                columns = new string[]{ "CRN","ClientName", "CreatedOn", "ServiceTypeName", "CustomerName","CustomerContactNuber","CustomerEmail",
                                "CustomerAddress","CustomerCity","CustomerPinCode","DeviceCategory",
                                 "DeviceBrand","DeviceModel","DOP","DevicePurchaseFrom","ProviderName"};
                filecontent = ExcelExportHelper.ExportExcel(response.AllocatedCalls, "", true, columns);
            }
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Excel.xlsx");

        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Call_Allocate_To_ASP)]
        public async Task<ActionResult> Create()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId, IsExport = false };
            // var Newcalls = await _customerSupport.GetASPCalls(filter);
            var Newcalls = new CallDetailsModel();
            Newcalls.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(session.CompanyId), "Value", "Text");
            
            // IsAssingedCall = true,
            Newcalls.DeliveryTypeList = new SelectList(await CommonModel.GetDeliveryServiceType(session.CompanyId), "Value", "Text");
            Newcalls.BrandList = new SelectList(_dropdown.BindBrand(session.CompanyId), "Value", "Text");
            Newcalls.CategoryList = new SelectList(_dropdown.BindCategory(session.CompanyId), "Value", "Text");
            Newcalls.SubCategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
            Newcalls.ProductList = new SelectList(Enumerable.Empty<SelectListItem>());
            Newcalls.CustomerTypeList = new SelectList(await CommonModel.GetLookup("Customer Type"), "Value", "Text");
            Newcalls.ConditionList = new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text");
            // calls.IsClient = IsClient,
            Newcalls.StatusList = new SelectList(await CommonModel.GetStatusTypes("Customer support"), "Value", "Text");
            Newcalls.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            Newcalls.LocationList = new SelectList(Enumerable.Empty<SelectListItem>());
            //Newcalls.ClientId = 101;
            Newcalls.DataSourceId = 102;



            return View(Newcalls);
           
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Call_Allocate_To_ASP)]
        [HttpPost]
        public async Task<ActionResult> Create(CallDetailsModel uploads)
        {
            var session = Session["User"] as SessionModel;

            uploads.UserId = session.UserId;
            uploads.CompanyId = session.CompanyId;
            //if (session.UserRole.ToLower().Contains("client"))
               // uploads.ClientId = session.RefKey;
            uploads.EventAction = 'I';
            var response = await _RepoCallLog.AddOrEditCallLog(uploads);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Esclated_Calls)]
        public async Task<ActionResult> EscalateCalls(char? Type)
        {
            var session = Session["User"] as SessionModel;
            if (Type ==null)
                Type = 'A';
            var filter = new FilterModel { CompId = session.CompanyId ,Type= Convert.ToChar(Type),UserId=session.UserId };

            var calls = new EsclatedCallsViewModel();
            calls.Calls= await _RepoCallLog.GetExclatedCalls(filter);
            calls.Type = Convert.ToChar(Type);
            return View(calls);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Esclated_Calls)]
        public async Task<ActionResult> Cancel()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId };
            var _calls = await _RepoCallLog.GetCancelRequestedData(filter);
            return View(_calls);
        }
    }
}
