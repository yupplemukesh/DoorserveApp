//using DataLayer.DataFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using TogoFogo.Filters;
using TogoFogo.Models;
using TogoFogo.Models.Customer_Support;
using TogoFogo.Permission;
using TogoFogo.Repository.Customer_Support;
namespace TogoFogo.Controllers
{
    public class CallToASCController : Controller
    {
        private readonly ICustomerSupport _customerSupport;

        public CallToASCController()
        {

            _customerSupport = new CustomerSupport();
        }
        // GET: CallToASC
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Service_Provider)]
        public async Task<ActionResult> Index()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel();

            if (session.UserTypeName.ToLower().Contains("provider"))
                filter.ProviderId = session.RefKey;

            filter.CompId = session.CompanyId;
            filter.IsExport = false;
            var calls = await _customerSupport.GetASCCalls(filter);
            calls.ClientList = new SelectList(await CommonModel.GetClientData(session.CompanyId), "Name", "Text");
            calls.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(session.CompanyId), "Value", "Text");
            calls.ServiceProviderList = new SelectList(await CommonModel.GetServiceProviders(session.CompanyId), "Name", "Text");
            if (session.UserTypeName.ToLower().Contains("provider"))
            calls.CallAllocate = new Models.Customer_Support.AllocateCallModel { ToAllocateList = new SelectList(await CommonModel.GetServiceCenters(session.RefKey), "Name", "Text") };
        else
                calls.CallAllocate = new Models.Customer_Support.AllocateCallModel { ToAllocateList = new SelectList(await CommonModel.GetServiceComp(session.CompanyId), "Name", "Text") };

            return View(calls);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create}, (int)MenuCode.Service_Provider)]
        [HttpPost]
        public async Task<ActionResult> Allocate(AllocateCallModel allocate)
        {

            try
            {
                var session = Session["User"] as SessionModel;
                allocate.AllocateTo = "ASC";
                allocate.UserId = session.UserId;
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
        [PermissionBasedAuthorize(new Actions[] { Actions.ExcelExport }, (int)MenuCode.Service_Provider)]
        [HttpGet]
        public async Task<FileContentResult> ExportToExcel(char tabIndex)
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel {CompId= session.CompanyId,tabIndex=tabIndex,IsExport=true};
            var response = await _customerSupport.GetASCCalls(filter);
            byte[] filecontent;
            string[] columns;
            if (tabIndex == 'P')
            {
         
                columns = new string []{ "CRN","ClientName", "CreatedOn", "ServiceTypeName", "CustomerName","CustomerContactNuber","CustomerEmail",
                                "CustomerAddress","CustomerCity","CustomerPinCode","DeviceCategory",
                                 "DeviceBrand","DeviceModel","DOP","PurchaseFrom"};
                filecontent = ExcelExportHelper.ExportExcel(response.PendingCalls, "", true, columns);
               
            }
            else
            {
                columns = new string[]{ "CRN","ClientName", "CreatedOn", "ServiceTypeName", "CustomerName","CustomerContactNuber","CustomerEmail",
                                "CustomerAddress","CustomerCity","CustomerPinCode","DeviceCategory",
                                 "DeviceBrand","DeviceModel","DOP","PurchaseFrom"};
                filecontent = ExcelExportHelper.ExportExcel(response.AllocatedCalls, "", true, columns);
                
            }
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Excel.xlsx");

        }
    }
}