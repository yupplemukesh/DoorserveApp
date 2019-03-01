using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Repository.Customer_Support;

namespace TogoFogo.Controllers
{
    public class CallToASPController : Controller
    {
        private readonly ICustomerSupport _customerSupport;
        public CallToASPController()
        {

            _customerSupport = new CustomerSupport();
        }
        // GET: CallToASP
        public async Task<ActionResult> Index()
        {
            var calls = await _customerSupport.GetCalls();
            calls.ClientList = new SelectList(await CommonModel.GetClientData(), "Name", "Text");
            calls.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(), "Value", "Text");
            calls.CallAllocate = new Models.Customer_Support.AllocateCallModel { ToAllocateList=new SelectList(await CommonModel.GetServiceProviders(),"Value","Text") };
            return View(calls);
        }
       
       
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
