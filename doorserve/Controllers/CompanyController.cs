using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using doorserve.Models;
using doorserve.Models.Company;
using System.Threading.Tasks;
using doorserve.Repository;
using System.Reflection;
using doorserve.Permission;
using doorserve.Repository.EmailSmsServices;
using doorserve.Filters;

namespace doorserve.Controllers
{
    public class ManageCompanyController : BaseController
    {
       private readonly DropdownBindController _dropdown;
       private readonly ICompany _compRepo;
       private readonly IContactPerson _ContactPersonRepo;
       private readonly IOrganization _OrgRepo;
       private readonly IBank _BankRepo;

        private readonly string _path = "/UploadedImages/Companies/";
        // GET: Company
        private readonly doorserve.Repository.EmailSmsTemplate.ITemplate _templateRepo;
        private readonly IEmailSmsServices _emailSmsServices;
        public ManageCompanyController()
        {
            _dropdown = new DropdownBindController();
            _compRepo = new Company();
            _ContactPersonRepo = new ContactPerson();
            _OrgRepo = new Organization();
            _BankRepo = new Bank();
            _templateRepo = new doorserve.Repository.EmailSmsTemplate.Template();
            _emailSmsServices = new Repository.EmailsmsServices();

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_company)]
        public async  Task<ActionResult> Index()
        {

            var _com = await _compRepo.GetCompanyDetails(CurrentUser.CompanyId);
            return View(_com);
        }
        private string SaveImageFile(HttpPostedFileBase file, string folderName)
        {
            try
            {
                string path = Server.MapPath(_path + folderName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Guid.NewGuid();
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                return savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        [PermissionBasedAuthorize(new Actions[] {Actions.Create}, (int)MenuCode.Manage_company)]
        public async Task<ActionResult> Create()
        {
            var CompanyData = new CompanyModel();
            CompanyData.Path = _path;
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
            CompanyData.Contact.LocationList = new SelectList(Enumerable.Empty<SelectListItem>());
            CompanyData.Agreement.ServiceList = await CommonModel.GetServiceType(null);
            CompanyData.Agreement.DeliveryServiceList = await CommonModel.GetDeliveryServiceType(null);
            return View(CompanyData);
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_company)]
        public async Task<ActionResult> Edit(Guid CompId)
        {
            TempData["Comp"] = null;
            var comp = await GetCompany(CompId);
            comp.ActiveTab = "tab-1";
            return View(comp);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_company)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddOrEditCompany(CompanyModel comp)
        {
            comp.CreatedBy = CurrentUser.UserId;
            if (comp.CompanyLogo != null && comp.CompanyPath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path+"Logo/" + comp.CompanyLogo)))
                    System.IO.File.Delete(Server.MapPath(_path+ "Logo/"+ comp.CompanyLogo));
            }
            if (comp.CompanyPath != null)
                comp.CompanyLogo= SaveImageFile(comp.CompanyPath,"Logo");
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
            comp.Contact.LocationList = new SelectList(_dropdown.BindLocation(), "Value", "Text");
            comp.Agreement.ServiceList = await CommonModel.GetServiceType(null);
            comp.Agreement.DeliveryServiceList = await CommonModel.GetDeliveryServiceType(null);
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
        private async Task<CompanyModel> GetCompany(Guid? CompanyId)
        {
            var   comp = await _compRepo.GetCompanyDetailByCompanyId(CompanyId);
            comp.CompanyFile = _path + "Logo/" + comp.CompanyLogo;
            comp.Action = 'U';
            comp.Path = _path;
            comp.CompanyTypeList = new SelectList(await CommonModel.GetLookup("Company Type"), "Value", "Text");
            comp.Organization = await _OrgRepo.GetOrganizationByRefKey(CompanyId);
            if (comp.Organization == null)
                comp.Organization = new OrganizationModel();
            if (!string.IsNullOrEmpty(comp.Organization.OrgGSTFileName))
                comp.Organization.OrgGSTFileUrl = _path + "Gsts/" + comp.Organization.OrgGSTFileName;
            if (!string.IsNullOrEmpty(comp.Organization.OrgPanFileName))
                comp.Organization.OrgPanFileUrl = _path + "PanCards/" + comp.Organization.OrgPanFileName;

            comp.Organization.GstCategoryList = new SelectList(await CommonModel.GetGstCategory(), "Value", "Text");
            comp.Organization.AplicationTaxTypeList = new SelectList(await CommonModel.GetLookup("Application Tax Type"), "Value", "Text");
            comp.Organization.StatutoryList = new SelectList(await CommonModel.GetLookup("Statutory Type"), "Value", "Text");
            comp.Contacts = await _ContactPersonRepo.GetContactPersonsByRefKey(comp.CompanyId);
            comp.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            comp.Contact.CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text");
            comp.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            comp.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            comp.Contact.LocationList = new SelectList(Enumerable.Empty<SelectListItem>());
            comp.BankDetails = await _BankRepo.GetBanksByRefKey(CompanyId);
            comp.BankDetail.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            comp.Agreement = await _compRepo.GetAgreement(CompanyId);
            if (comp.Agreement == null)
                comp.Agreement = new AgreementModel();
            comp.Agreement.ServiceList = await CommonModel.GetServiceType(null);
            comp.Agreement.DeliveryServiceList = await CommonModel.GetDeliveryServiceType(null);
            if(!string.IsNullOrEmpty(comp.Agreement.AgreementFile))
            comp.Agreement.AgreementFileUrl = _path + "Agreements/" + comp.Agreement.AgreementFile;
            if (!string.IsNullOrEmpty(comp.Agreement.CancelledChequeFile))
                comp.Agreement.CancelledChequeFileUrl = _path + "Cheques/" + comp.Agreement.CancelledChequeFile;
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
        [PermissionBasedAuthorize(new Actions[] {Actions.Edit }, (int)MenuCode.Manage_company)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddorEditOrganizaion(OrganizationModel organization)
        {

            if (organization.OrgGSTFileName != null && organization.OrgGSTNumberFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path + "Gsts/" + organization.OrgGSTFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "Gsts/" + organization.OrgGSTNumberFilePath));
            }

            if (organization.OrgPanFileName != null && organization.OrgPanNumberFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path + "PanCards/" + organization.OrgPanFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "PanCards/" + organization.OrgPanFileName));
            }
            if (organization.OrgGSTNumberFilePath != null)
                organization.OrgGSTFileName = SaveImageFile(organization.OrgGSTNumberFilePath, "Gsts");
            if (organization.OrgPanNumberFilePath != null)
                organization.OrgPanFileName = SaveImageFile(organization.OrgPanNumberFilePath, "PanCards");
            if (organization.OrgId == null)
                organization.Action = 'I';
            else
                organization.Action = 'U';

            organization.GstCategoryList = new SelectList(await CommonModel.GetGstCategory(), "Value", "Text");
            organization.AplicationTaxTypeList = new SelectList(await CommonModel.GetLookup("Application Tax Type"), "Value", "Text");
            organization.StatutoryList = new SelectList(await CommonModel.GetLookup("Statutory Type"), "Value", "Text");
            organization.UserId = CurrentUser.UserId;

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
        [PermissionBasedAuthorize(new Actions[] {Actions.Edit }, (int)MenuCode.Manage_company)]
        [HttpPost]
        public async Task<ActionResult> AddOrEditContactPerson(OtherContactPersonModel contact)
        {
            if (contact.ConVoterIdFileName != null && contact.ConVoterIdFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path + "VoterIds/" + contact.ConVoterIdFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "VoterIds/" + contact.ConVoterIdFileName));
            }

            if (contact.ConAdhaarFileName != null && contact.ConAdhaarNumberFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path + "ADHRs/" + contact.ConAdhaarFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "ADHRs/" + contact.ConAdhaarFileName));
            }
            if (contact.ConPanFileName != null && contact.ConPanNumberFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path + "PanCards/" + contact.ConPanFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "PanCards/" + contact.ConPanFileName));
            }
            if (contact.ConVoterIdFilePath != null)
                contact.ConVoterIdFileName = SaveImageFile(contact.ConVoterIdFilePath, "/VoterIds");
            if (contact.ConPanNumberFilePath != null)
                contact.ConPanFileName = SaveImageFile(contact.ConPanNumberFilePath, "PanCards/");
            if (contact.ConAdhaarNumberFilePath != null)
                contact.ConAdhaarFileName = SaveImageFile(contact.ConAdhaarNumberFilePath, "ADHRs/");

            var pwd = CommonModel.RandomPassword(8);
            if (contact.IsUser)
                contact.Password = Encrypt_Decript_Code.encrypt_decrypt.Encrypt(pwd, true);
        
            contact.UserTypeId = 1;
            contact.UserId = CurrentUser.UserId;
            contact.CompanyId = contact.RefKey;

            if (contact.ContactId == null)
                contact.EventAction = 'I';
            else
                contact.EventAction = 'U';
            CompanyModel comp = new CompanyModel();
            var response = await _ContactPersonRepo.AddUpdateContactDetails(contact);
            if (response.IsSuccess)
            {
                contact.ContactId = new Guid(response.result);
                if (contact.EventAction == 'U')
                {
                    if (contact.IsUser && !contact.CurrentIsUser)
                    {
                        var Templates = await _templateRepo.GetTemplateByActionId(12, CurrentUser.CompanyId);
                        if (Templates.Count > 0)
                        {
                            CurrentUser.Email = contact.ConEmailAddress;
                            var WildCards = await CommonModel.GetWildCards(null);
                            var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                            U.Val = contact.ConFirstName;
                            U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                            U.Val = pwd;
                            U = WildCards.Where(x => x.Text.ToUpper() == "USER NAME").FirstOrDefault();
                            U.Val = contact.ConEmailAddress;
                            CurrentUser.Mobile = contact.ConMobileNumber;
                            var c = WildCards.Where(x => x.Val != string.Empty).ToList();
                                await _emailSmsServices.Send(Templates, c, CurrentUser);
                        }
                    }
                }
                else
                {
                    if (contact.IsUser)
                    {
                        var Templates = await _templateRepo.GetTemplateByActionId(12, CurrentUser.CompanyId);
                        if (Templates.Count > 0)
                        {
                            CurrentUser.Email = contact.ConEmailAddress;
                            var WildCards = await CommonModel.GetWildCards(null);
                            var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                            U.Val = contact.ConFirstName;
                            U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                            U.Val = pwd;
                            U = WildCards.Where(x => x.Text.ToUpper() == "USER NAME").FirstOrDefault();
                            U.Val = contact.ConEmailAddress;
                            CurrentUser.Mobile = contact.ConMobileNumber;
                            var c = WildCards.Where(x => x.Val != string.Empty).ToList();
                            await _emailSmsServices.Send(Templates, c, CurrentUser);
                        }
                    }

                }
               
            }
            if (TempData["Comp"] != null)
            {
                comp = TempData["Comp"] as CompanyModel;
                var Location = _dropdown.BindLocationNew(contact.LocationId).FirstOrDefault();
                contact.LocationName = Location.Text;
                comp.Contacts.Add(contact);
                comp.ActiveTab = "tab-4";
                comp.Action = 'I';
                TempData["Comp"] = comp;
                comp.Contact = new OtherContactPersonModel
                {
                    AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text"),
                    RefKey = comp.CompanyId,
                    CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text"),
                    StateList = new SelectList(Enumerable.Empty<SelectList>()),
                    // CityList = new SelectList(Enumerable.Empty<SelectList>())
                   LocationList = new SelectList(Enumerable.Empty<SelectListItem>())
            };            
            }
            else
                comp.Action = 'U';
            TempData["response"] = response;
            if (comp.Action == 'I')          
                return View("Create", comp);       
            else
            {
                comp = await GetCompany(contact.RefKey);
                comp.ActiveTab = "tab-3";
                return View("Edit", comp);

            }  

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_company)]
        [HttpPost]
        public async Task<ActionResult> AddOrEditBank(BankDetailModel bank)
        {
            if (bank.BankCancelledChequeFileName != null && bank.BankCancelledChequeFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path + "Cheques/" + bank.BankCancelledChequeFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "Cheques/" + bank.BankCancelledChequeFileName));
            }

            if (bank.BankCancelledChequeFilePath!= null)
                bank.BankCancelledChequeFileName = SaveImageFile(bank.BankCancelledChequeFilePath, "Cheques/");
            if (bank.bankId == null)
                bank.EventAction = 'I';
            else
                bank.EventAction = 'U';
            CompanyModel comp = new CompanyModel();
            if (TempData["Comp"] != null)
            {
                comp = TempData["Comp"] as CompanyModel;
                comp.ActiveTab = "tab-5";
                comp.Action = 'I';
                comp.BankDetails.Add(bank);
                comp.BankDetail = new BankDetailModel {
                                        RefKey = comp.CompanyId,
                    BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text")
            };
                TempData["Comp"] = comp;
            }
            else
                comp.Action = 'U';
            bank.UserId = CurrentUser.UserId;
            var response = await _BankRepo.AddUpdateBankDetails(bank);
            TempData["response"] = response;
            if (comp.Action == 'I')
                return View("Create", comp);
            else
            {
                comp = await GetCompany(bank.RefKey);
                comp.ActiveTab = "tab-4";
                return View("Edit", comp);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_company)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddorEditAgreement(AgreementModel agreement)
        {
            CompanyModel comp = new CompanyModel();
            if (TempData["Comp"] != null)
            {
                comp = TempData["Comp"] as CompanyModel;
                comp.ActiveTab = "tab-6";
                comp.Agreement = agreement;
                TempData["Comp"] = comp;
                comp.Action = 'I';

                if (agreement.ServiceList.Where(x => x.IsChecked).Count() == 0 || agreement.DeliveryServiceList.Where(x => x.IsChecked).Count() == 0)
                    return View("create", comp);
            }
            else
            {
                comp = await GetCompany(agreement.RefKey);
                comp.Action = 'U';
                comp.ActiveTab = "tab-5";
                if (agreement.ServiceList.Where(x => x.IsChecked).Count() == 0 || agreement.DeliveryServiceList.Where(x => x.IsChecked).Count() == 0)
                    return View("edit", comp);
            }           
            if (agreement.CancelledChequeFile != null && agreement.CancelledChequeFile != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path + "Cheques/" + agreement.CancelledChequeFile)))
                    System.IO.File.Delete(Server.MapPath(_path + "Cheques/" + agreement.CancelledChequeFile));
            }
            if (agreement.AgreementFile != null && agreement.AgreementPath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path + "Agreements/" + agreement.AgreementFile)))
                    System.IO.File.Delete(Server.MapPath(_path + "Agreements/" + agreement.AgreementFile));
            }
            if (agreement.CancelledChequePath != null)
                agreement.CancelledChequeFile = SaveImageFile(agreement.CancelledChequePath, "Cheques/");
            if (agreement.AgreementPath != null)
                agreement.AgreementFile = SaveImageFile(agreement.AgreementPath, "Agreements/");
            if (agreement.AGRId == null)
                agreement.Action = 'I';
            else
                agreement.Action = 'U';
           
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

            agreement.CreatedBy = CurrentUser.UserId;
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_company)]
        [HttpPost]

        public async Task<ActionResult> Registration(CompanyModel comp)
        {
            var CMP = comp;
            comp.ActiveTab = "tab-6";
            if (TempData["Comp"] != null)
            {
                comp = TempData["Comp"] as CompanyModel;
                comp.Remarks = CMP.Remarks;
                comp.IsActive = CMP.IsActive;           
                comp.Action = 'I';
                TempData["Comp"] = comp;
            }
            else
                comp.Action = 'U';
            comp.CreatedBy = CurrentUser.UserId;
            var response = await _compRepo.AddUpdateDeleteCompany(comp);
            TempData["response"] = response;
            return RedirectToAction("Index");

        }
    }
}