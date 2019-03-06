﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using TogoFogo.Models;
using TogoFogo.Models.Customer_Support;
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

        public async Task<ActionResult> Index()
        {
            var calls = await _customerSupport.GetASCCalls();
            calls.ClientList = new SelectList(await CommonModel.GetClientData(), "Name", "Text");
            calls.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(), "Value", "Text");
            calls.ServiceProviderList = new SelectList(await CommonModel.GetServiceProviders(), "Value", "Text");
            calls.CallAllocate = new Models.Customer_Support.AllocateCallModel { ToAllocateList = new SelectList(await CommonModel.GetServiceCenters(), "Value", "Text") };
            return View(calls);
        }
        [HttpPost]
        public async Task<ActionResult> Allocate(AllocateCallModel allocate)
        {

            try
            {
                allocate.AllocateTo = "ASC";
                allocate.UserId = Convert.ToInt32(Session["User_ID"]);
                var response = await _customerSupport.AllocateCall(allocate);
                TempData["response"] = response;
                TempData.Keep("response");
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