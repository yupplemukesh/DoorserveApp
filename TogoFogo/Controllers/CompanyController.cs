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
        readonly DropdownBindController dbc = new DropdownBindController();
        // GET: Company
        public ActionResult Index()
        {
            var _company = new CompanyModel();
           // _company.Organization.StatutoryList=new SelectList()
            return View(_company);
        }
        public async Task<ActionResult> Create()
        {
            var CompanyData = new CompanyModel();
            CompanyData.Organization.GstCategoryList = new SelectList(await CommonModel.GetGstCategory(), "Value", "Text");
            CompanyData.Organization.AplicationTaxTypeList = new SelectList(await CommonModel.GetLookup("Application Tax Type"), "Value", "Text");
            CompanyData.Organization.StatutoryList = new SelectList(await CommonModel.GetLookup("Statutory Type"), "Value", "Text");
            CompanyData.BankDetail.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            CompanyData.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            CompanyData.Contact.CountryList = new SelectList(dbc.BindCountry(), "Value", "Text");
            CompanyData.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            CompanyData.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            return View(CompanyData);
        }

        /*public ActionResult _AddOrUpdateBankDetails()
        {
            return View();
        }*/
    }
}