using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Models.Company;
using System.Threading.Tasks;
using TogoFogo.Repository;

namespace TogoFogo.Controllers
{
    public class ManageCompanyController : Controller
    {
       private readonly DropdownBindController _dropdown;
       private readonly ICompany _compRepo;
       private readonly IContactPerson _ContactPersonRepo;
       private readonly IOrganization _OrgRepo;
       private readonly IBank _BankRepo;


        // GET: Company

        public ManageCompanyController()
        {
            _dropdown = new DropdownBindController();
            _compRepo = new Company();
            _ContactPersonRepo = new ContactPerson();
            _OrgRepo = new Organization();
            _BankRepo = new Bank();
        }

        public async  Task<ActionResult> Index()
        {
            var _com = await _compRepo.GetCompanyDetails();
            return View(_com);
        }
        public async Task<ActionResult> Create()
        {
            var CompanyData = new CompanyModel();
            CompanyData.ActiveTab = "tab-1";
            CompanyData.Action = 'I';
            CompanyData.CompanyTypeList= new SelectList(await CommonModel.GetLookup("Company Type"),"Value","Text");
            CompanyData.Organization.GstCategoryList = new SelectList(await CommonModel.GetGstCategory(), "Value", "Text");
            CompanyData.Organization.AplicationTaxTypeList = new SelectList(await CommonModel.GetLookup("Application Tax Type"), "Value", "Text");
            CompanyData.Organization.StatutoryList = new SelectList(await CommonModel.GetLookup("Statutory Type"), "Value", "Text");
            CompanyData.BankDetail.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            CompanyData.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            CompanyData.Contact.CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text");
            CompanyData.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            CompanyData.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            CompanyData.Agreement.ServiceList = await CommonModel.GetServiceType();
            CompanyData.Agreement.DeliveryServiceList = await CommonModel.GetDeliveryServiceType();
            return View(CompanyData);
        }

        public async Task<ActionResult> Edit(Guid CompId)
        {
            var comp = await GetCompany(CompId);
            comp.ActiveTab = "tab-1";
            return View(comp);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditCompany(CompanyModel comp)
        {
            comp.CreatedBy = Convert.ToInt32(Session["User_ID"]);
 
            var response = await _compRepo.AddUpdateDeleteCompany(comp);
            comp.ActiveTab = "tab-2";
            TempData["response"] = response;
            comp.CompanyId = new Guid(response.result);
            comp.CompanyTypeList = new SelectList(await CommonModel.GetLookup("Company Type"), "Value", "Text");
            comp.Organization.GstCategoryList = new SelectList(await CommonModel.GetGstCategory(), "Value", "Text");
            comp.Organization.AplicationTaxTypeList = new SelectList(await CommonModel.GetLookup("Application Tax Type"), "Value", "Text");
            comp.Organization.StatutoryList = new SelectList(await CommonModel.GetLookup("Statutory Type"), "Value", "Text");
            comp.BankDetail.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            comp.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            comp.Contact.CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text");
            comp.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            comp.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            comp.Agreement.ServiceList = await CommonModel.GetServiceType();
            comp.Agreement.DeliveryServiceList = await CommonModel.GetDeliveryServiceType();
            if (comp.Action == 'I')
            {
                comp.ActiveTab = "tab-2";
                comp.Organization.RefKey = comp.CompanyId;
                TempData["Comp"] = comp;
                TempData.Keep("Comp");
                return View("Create", comp);
            }
            else
            {
                comp = await GetCompany(comp.CompanyId);
                comp.ActiveTab = "tab-2";
                return View("Edit", comp);
            }
        
        }
        private async Task<CompanyModel> GetCompany(Guid CompanyId)
        {
            var   comp = await _compRepo.GetCompanyDetailByCompanyId(CompanyId);
            comp.Action = 'U';
            comp.CompanyTypeList = new SelectList(await CommonModel.GetLookup("Company Type"), "Value", "Text");
            comp.Organization = await _OrgRepo.GetOrganizationByRefKey(CompanyId);
            if (comp.Organization == null)
                comp.Organization = new OrganizationModel();
            comp.Organization.GstCategoryList = new SelectList(await CommonModel.GetGstCategory(), "Value", "Text");
            comp.Organization.AplicationTaxTypeList = new SelectList(await CommonModel.GetLookup("Application Tax Type"), "Value", "Text");
            comp.Organization.StatutoryList = new SelectList(await CommonModel.GetLookup("Statutory Type"), "Value", "Text");
            comp.Contacts = await _ContactPersonRepo.GetContactPersonsByRefKey(comp.CompanyId);
            comp.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            comp.Contact.CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text");
            comp.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            comp.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            comp.BankDetails = await _BankRepo.GetBanksByRefKey(CompanyId);
            comp.BankDetail.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            comp.Agreement = await _compRepo.GetAgreement(CompanyId);
            if (comp.Agreement == null)
                comp.Agreement = new AgreementModel();
            comp.Agreement.ServiceList = await CommonModel.GetServiceType();
            comp.Agreement.DeliveryServiceList = await CommonModel.GetDeliveryServiceType();
            if (!string.IsNullOrEmpty(comp.Agreement.DeliveryTypes))
            {
                var _DeliveryService = comp.Agreement.DeliveryTypes.Split(',');
                for (int i = 0; i < _DeliveryService.Length; i++)
                {
                    var item = comp.Agreement.DeliveryServiceList.Where(x => x.Value == Convert.ToInt32(_DeliveryService[i])).FirstOrDefault();
                    if (item != null)
                        item.IsChecked = true;

                }
            }

            if (!string.IsNullOrEmpty(comp.Agreement.ServiceTypes))
            {
                var _serviceType = comp.Agreement.ServiceTypes.Split(',');
                for (int i = 0; i < _serviceType.Length; i++)
                {
                    var item = comp.Agreement.ServiceList.Where(x => x.Value == Convert.ToInt32(_serviceType[i])).FirstOrDefault();
                    if (item != null)
                        item.IsChecked = true;
                }
            }

            return comp;
        }

        [HttpPost]
        public async Task<ActionResult> AddorEditOrganizaion(OrganizationModel organization)
        {
            if (organization.OrgId == null)
                organization.Action = 'I';
            else
                organization.Action = 'U';
            organization.UserId = Convert.ToInt32(Session["User_ID"]);
            CompanyModel comp = new CompanyModel();
           if (TempData["Comp"] !=null)
            {
                comp = TempData["Comp"] as CompanyModel;
                comp.Action = 'I';
                comp.Organization = organization;
                TempData["Comp"] = comp;
                comp.ActiveTab = "tab-3";
            }
           else
                comp.Action = 'U';
            var response = await _OrgRepo.AddUpdateOrgnization(organization);
            TempData["response"] = response;
            if (comp.Action == 'I')
                return View("Create", comp);
            else
            {
                comp = await GetCompany(organization.RefKey);
                comp.ActiveTab = "tab-3";
                return View("Edit", comp);

            }      

        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditContactPerson(ContactPersonModel contact)
        {
            if (contact.ContactId == null)
                contact.Action = 'I';
            else
                contact.Action = 'U';
            CompanyModel comp = new CompanyModel();
          
            var response = await _ContactPersonRepo.AddUpdateContactDetails(contact);
            if (TempData["Comp"] != null)
            {
                comp = TempData["Comp"] as CompanyModel;
                comp.Contacts.Add(contact);
                comp.ActiveTab = "tab-4";
                comp.Action = 'I';
                TempData["Comp"] = comp;
            }
            else
                comp.Action = 'U';
            TempData["response"] = response;
            if (comp.Action == 'I')          
                return View("Create", comp);       
            else
            {
                comp = await GetCompany(contact.RefKey);
                comp.ActiveTab = "tab-4";
                return View("Edit", comp);

            }  

        }

        [HttpPost]
        public async Task<ActionResult> AddOrEditBank(BankDetailModel bank)
        {
            if (bank.bankId == null)
                bank.Action = 'I';
            else
                bank.Action = 'U';
            CompanyModel comp = new CompanyModel();
            if (TempData["Comp"] != null)
            {
                comp = TempData["Comp"] as CompanyModel;
                comp.ActiveTab = "tab-5";
                comp.Action = 'I';
                comp.BankDetails.Add(bank);
                TempData["Comp"] = comp;
            }
            else
                comp.Action = 'U';
            bank.UserId = Convert.ToInt32(Session["User_ID"]);
            var response = await _BankRepo.AddUpdateBankDetails(bank);
            TempData["response"] = response;
            if (comp.Action == 'I')
                return View("Create", comp);
            else
            {
                comp = await GetCompany(bank.RefKey);
                comp.ActiveTab = "tab-5";
                return View("Edit", comp);


            }

        }

        [HttpPost]
        public async Task<ActionResult> AddorEditAgreement(AgreementModel agreement)
        {
            if (agreement.AGRId == null)
                agreement.Action = 'I';
            else
                agreement.Action = 'U';
            CompanyModel comp = new CompanyModel();
            if (TempData["Comp"] != null)
            {
                comp = TempData["Comp"] as CompanyModel;
                comp.ActiveTab = "tab-6";
                comp.Agreement = agreement;
                TempData["Comp"] = comp;
                comp.Action = 'I';
            }
            else
                comp.Action = 'U';
            foreach (var item in agreement.ServiceList)
            {
                if (item.IsChecked)
                    agreement.ServiceTypes = agreement.ServiceTypes + "," + item.Value;

            }
            agreement.ServiceTypes = agreement.ServiceTypes.Trim(',');
            foreach (var item in agreement.DeliveryServiceList)
            {
                if (item.IsChecked)
                    agreement.DeliveryTypes = agreement.DeliveryTypes + "," + item.Value;

            }
            agreement.DeliveryTypes = agreement.DeliveryTypes.Trim(',');


            agreement.CreatedBy = Convert.ToInt32(Session["User_ID"]);
            var response = await _compRepo.AddOrEditAgreeement(agreement);
            TempData["response"] = response;
            if(comp.Action=='I')
            return View("Create", comp);
            else
            {
                comp = await GetCompany(agreement.RefKey);
                comp.ActiveTab = "tab-6";
                return View("Edit", comp);

            }

        }

        [HttpPost]
        public async Task<ActionResult> Registration(CompanyModel comp)
        {

            var CMP = comp;
            comp.ActiveTab = "tab-6";
            if (TempData["Comp"] != null)
            {
                comp = TempData["Comp"] as CompanyModel;
                comp.Comments = CMP.Comments;
                comp.IsActive = CMP.IsActive;           
                comp.Action = 'I';
                TempData["Comp"] = comp;
            }
            else
                comp.Action = 'U';
            comp.CreatedBy = Convert.ToInt32(Session["User_ID"]);
            var response = await _compRepo.AddUpdateDeleteCompany(comp);
            TempData["response"] = response;
            return RedirectToAction("Index");

        }
    }
}