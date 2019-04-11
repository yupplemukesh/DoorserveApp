using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Models.Company;
using System.Threading.Tasks;

namespace TogoFogo.Controllers
{
    public class CompanyController : Controller
    {
       private readonly DropdownBindController _dropdown;

        // GET: Company

        public CompanyController()
        {
            _dropdown = new DropdownBindController();

        }

        public ActionResult Index()
        {
            var _company = new CompanyModel();
           // _company.Organization.StatutoryList=new SelectList()
            return View(_company);
        }
        public async Task<ActionResult> Create()
        {
            var CompanyData = new CompanyModel();
            CompanyData.CompanyTypeList= new SelectList(await CommonModel.GetLookup("Company Type"),"Value","Text");
            CompanyData.Organization.GstCategoryList = new SelectList(await CommonModel.GetGstCategory(), "Value", "Text");
            CompanyData.Organization.AplicationTaxTypeList = new SelectList(await CommonModel.GetLookup("Application Tax Type"), "Value", "Text");
            CompanyData.Organization.StatutoryList = new SelectList(await CommonModel.GetLookup("Statutory Type"), "Value", "Text");
            CompanyData.BankDetail.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            CompanyData.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            CompanyData.Contact.CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text");
            CompanyData.Agreement.PayableTypeList = await CommonModel.GetLookup("Payment Type");
            CompanyData.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            CompanyData.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            return View(CompanyData);
        }     
    }
}