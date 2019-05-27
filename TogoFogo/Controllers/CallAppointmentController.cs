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
using TogoFogo.Models;
using TogoFogo.Permission;
using TogoFogo.Repository.ServiceCenters;
using TogoFogo.Models.ServiceCenter;
using AutoMapper;
using TogoFogo.Repository;
using TogoFogo.Filters;


namespace TogoFogo.Controllers
{
    public class CallAppointmentController : Controller

    {
        private readonly ICenter _centerRepo;      
        private readonly DropdownBindController _dropdown;

        public CallAppointmentController()
            {
            _centerRepo = new Center();          
            _dropdown = new DropdownBindController();
        }
    // GET: CallAppointment
        public async Task<ActionResult> Index()
        {
            var filter = new FilterModel { CompId = SessionModel.CompanyId };
            var Appointcalls = await _centerRepo.GetCallDetails(filter);

            return View(Appointcalls);
        }

        public async Task<ActionResult> Edit(string CRN)
        {
            var CalAppintmentModel = await _centerRepo.GetCallsDetailsById(CRN);
            CalAppintmentModel.BrandList = new SelectList(_dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            CalAppintmentModel.CategoryList = new SelectList(_dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            CalAppintmentModel.ProductList = new SelectList(_dropdown.BindProduct(CalAppintmentModel.DeviceBrandId), "Value", "Text");
            CalAppintmentModel.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(SessionModel.CompanyId), "Value", "Text");
            CalAppintmentModel.DeliveryTypeList = new SelectList(await CommonModel.GetDeliveryServiceType(SessionModel.CompanyId), "Value", "Text");
            CalAppintmentModel.CustomerTypeList = new SelectList(await CommonModel.GetLookup("Customer Type"), "Value", "Text");
            CalAppintmentModel.ConditionList = new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text");
            CalAppintmentModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "Value", "Text");
            CalAppintmentModel.CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text");
            CalAppintmentModel.StateList = new SelectList(_dropdown.BindState(CalAppintmentModel.CountryId), "Value", "Text");
            CalAppintmentModel.CityList = new SelectList(_dropdown.BindLocation(CalAppintmentModel.StateId), "Value", "Text");
            return PartialView(Mapper.Map<CallDetailsModel>(CalAppintmentModel));
            //UpdateAppointmentDetail
        }
        [HttpPost]
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