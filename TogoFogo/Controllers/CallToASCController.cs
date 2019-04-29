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
        private SessionModel user;
        public CallToASCController()
        {

            _customerSupport = new CustomerSupport();
        }
        // GET: CallToASC
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Call Allocate To ASC")]
        public async Task<ActionResult> Index()
        {
            user = Session["User"] as SessionModel;
            var filter = new FilterModel();
            filter.CompId = user.CompanyId;
            var calls = await _customerSupport.GetASCCalls(filter);
            calls.ClientList = new SelectList(await CommonModel.GetClientData(user.CompanyId), "Name", "Text");
            calls.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(user.CompanyId), "Value", "Text");
            calls.ServiceProviderList = new SelectList(await CommonModel.GetServiceProviders(user.CompanyId), "Name", "Text");
            Guid? providerId = null;
            if (user.UserRole.ToLower().Contains("provider"))
                providerId = await CommonModel.GetProviderIdByUser(user.UserId);
            calls.CallAllocate = new Models.Customer_Support.AllocateCallModel { ToAllocateList = new SelectList(await CommonModel.GetServiceCenters(providerId), "Name", "Text") };
            return View(calls);
        }
        [HttpPost]
        public async Task<ActionResult> Allocate(AllocateCallModel allocate)
        {

            try
            {
                user = Session["User"] as SessionModel;
                allocate.AllocateTo = "ASC";
                allocate.UserId = user.UserId;
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

        [HttpGet]
        public async Task<FileContentResult> ExportToExcel(char tabIndex)
        {

            user = Session["User"] as SessionModel;
            var filter = new FilterModel {CompId=user.CompanyId
                ,tabIndex=tabIndex
            };
            var response = await _customerSupport.GeteExportASCCalls(filter);
            byte[] filecontent;
            string[] columns;
            if (tabIndex == 'P')
            {
                columns = new string []{ "CRN","ClientName", "CreatedOn", "ServiceTypeName", "CustomerName","CustomerContactNuber","CustomerEmail",
                                "CustomerAddress","CustomerCity","CustomerPinCode","DeviceCategory",
                                 "DeviceBrand","DeviceModel","DOP","DevicePurchaseFrom","ProviderName"};
                filecontent = ExcelExportHelper.ExportExcel(response, "", true, columns);
               
            }
            else
            {
                columns = new string[]{ "CRN","ClientName", "CreatedOn", "ServiceTypeName", "CustomerName","CustomerContactNuber","CustomerEmail",
                                "CustomerAddress","CustomerCity","CustomerPinCode","DeviceCategory",
                                 "DeviceBrand","DeviceModel","DOP","DevicePurchaseFrom","ProviderName","ServiceCenterName"};
                filecontent = ExcelExportHelper.ExportExcel(response, "", true, columns);
                
            }
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Excel.xlsx");

        }
    }
}