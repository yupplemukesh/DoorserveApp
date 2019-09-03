using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using doorserve.Models;
using doorserve.Permission;
using doorserve.Repository.ServiceCenters;
using doorserve.Models.ServiceCenter;
using AutoMapper;
using doorserve.Repository;
using doorserve.Filters;


namespace doorserve.Controllers
{
    public class CallAppointmentController : BaseController

    {
        private readonly ICenter _centerRepo;      
        private readonly DropdownBindController _dropdown;

        public CallAppointmentController()
            {
            _centerRepo = new Center();          
            _dropdown = new DropdownBindController();
        }
        // GET: CallAppointment
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Schedule_Appointment)]
        public async Task<ActionResult> Index()
        { 
            var filter = new FilterModel { CompId = CurrentUser.CompanyId };
            var Appointcalls = await _centerRepo.GetCallDetails(filter);

            return View(Appointcalls);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Schedule_Appointment)]
        public async Task<ActionResult> Edit(string CRN)
        {
            var CalAppintmentModel = await _centerRepo.GetCallsDetailsById(CRN);
            CalAppintmentModel.BrandList = new SelectList(_dropdown.BindBrand(CurrentUser.CompanyId), "Value", "Text");
            CalAppintmentModel.CategoryList = new SelectList(_dropdown.BindCategory(CurrentUser.CompanyId), "Value", "Text");
            CalAppintmentModel.ProductList = new SelectList(_dropdown.BindProduct(CalAppintmentModel.DeviceBrandId), "Value", "Text");
            CalAppintmentModel.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(CurrentUser.CompanyId), "Value", "Text");
            CalAppintmentModel.DeliveryTypeList = new SelectList(await CommonModel.GetDeliveryServiceType(CurrentUser.CompanyId), "Value", "Text");
            CalAppintmentModel.CustomerTypeList = new SelectList(await CommonModel.GetLookup("Customer Type"), "Value", "Text");
            CalAppintmentModel.ConditionList = new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text");
            CalAppintmentModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "Value", "Text");
            CalAppintmentModel.CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text");
            CalAppintmentModel.StatusList = new SelectList(_dropdown.BindCallStatusNew(), "Value", "Text");
            return PartialView(Mapper.Map<CallDetailsModel>(CalAppintmentModel));
            //UpdateAppointmentDetail
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Schedule_Appointment)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Edit(CallDetailsModel Appointment)
        {
            try
            {
                var response =  await _centerRepo.EditCallAppointment(Appointment);
                TempData["response"] = response;
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                var response = new ResponseModel { Response = ex.Message, IsSuccess = false };
                TempData["response"] = response;
                TempData.Keep("response");               
                return RedirectToAction("Index");

            }
           
                
        }

    }
}