
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Permission;
using TogoFogo.Repository;
using System.Reflection;
using TogoFogo.Repository.ServiceProviders;
using TogoFogo.Filters;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using TogoFogo.Repository.ImportFiles;
using TogoFogo.Repository.EmailSmsServices;

namespace TogoFogo.Controllers
{

    public class ManageServiceProvidersController : Controller
    {
        private readonly IProvider _provider;
        private readonly IBank _bank;
        private readonly IContactPerson _contactPerson;
        private readonly IServices _services;
        private readonly string _path = "/UploadedImages/Providers/";
        private readonly string _fpath = "/Files/ServiceProviders/";
        private readonly DropdownBindController dropdown;
        private readonly IUploadFiles _RepoUploadFile;
        private readonly TogoFogo.Repository.EmailSmsTemplate.ITemplate _templateRepo;
        private readonly IEmailSmsServices _emailSmsServices;
        public ManageServiceProvidersController()
        {
            _provider = new Provider();
            dropdown = new DropdownBindController();
            _bank = new Bank();
            _contactPerson = new ContactPerson();
            _RepoUploadFile = new UploadFiles();
            _templateRepo = new TogoFogo.Repository.EmailSmsTemplate.Template();
            _emailSmsServices = new Repository.EmailsmsServices();
            _services = new Services();
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Service_Provider)]
        public async Task<ActionResult> Index()
        {
            var SessionModel = Session["User"] as SessionModel;
            ViewBag.PageNumber = (Request.QueryString["grid-page"] == null) ? "1" : Request.QueryString["grid-page"];         
            Guid? providerId = null;         
            if (SessionModel.UserTypeName.ToLower().Contains("provider"))
            {
               var prv= await CommonModel.GetServiceProviderIdByUserId(SessionModel.UserId);
                providerId = prv.Name;
            }

            var filter = new FilterModel { CompId = SessionModel.CompanyId, RefKey = providerId};
            var Providers = await _provider.GetProviders(filter);
            return View(Providers);
        }
        private string SaveImageFile(HttpPostedFileBase file, string folderName)
        {
            try
            {
                string path = Server.MapPath("~/UploadedImages/Providers/" + folderName);
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
        [PermissionBasedAuthorize(new Actions[] {Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]
        [HttpPost]
        public async Task<ActionResult> AddOrEditBank(BankDetailModel bank)
        {

            var SessionModel = Session["User"] as SessionModel;
            if (bank.BankCancelledChequeFilePath != null && bank.BankCancelledChequeFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path + "Banks/" + bank.BankCancelledChequeFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "Banks/" + bank.BankCancelledChequeFileName));
            }

            if (bank.BankCancelledChequeFilePath != null)
                bank.BankCancelledChequeFileName = SaveImageFile(bank.BankCancelledChequeFilePath, "Banks");
            if (bank.bankId != null)
                bank.EventAction = 'U';
            else
                bank.EventAction = 'I';
            bank.UserId = SessionModel.UserId;
            if (TempData["provider"] != null)
            {
                var _provider = TempData["provider"] as ServiceProviderModel;
                bank.RefKey = _provider.ProviderId;
            }
            var response = await _bank.AddUpdateBankDetails(bank);
            TempData["response"] = response;
            if (TempData["provider"] != null)
            {
                var Provider = TempData["Provider"] as ServiceProviderModel;
                bank.bankId = new Guid(response.result);
                var name = Provider.Bank.BankList.Where(x => x.Value == bank.BankNameId.ToString()).FirstOrDefault();
                bank.BankName = name.Text;
                Provider.BankDetails.Add(bank);
                Provider.action = 'I';
                Provider.Activetab = "tab-5";
                TempData["provider"] = Provider;
                return View("Create", Provider);
            }
            else
            {
                var Provider = await GetProvider(bank.RefKey);
                Provider.action = 'U';
                Provider.Activetab = "tab-4";
                return View("Edit", Provider);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]
        [HttpPost]
        public async Task<ActionResult> AddOrPersonContactDetails(OtherContactPersonModel contact)
        {
            var SessionModel = Session["User"] as SessionModel;
            if (contact.ConAdhaarNumberFilePath != null && contact.ConAdhaarFileName != null)
            {

                if (System.IO.File.Exists(Server.MapPath(_path + "ADHRS/" + contact.ConAdhaarFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "ADHRS/" + contact.ConAdhaarFileName));
            }
            if (contact.ConVoterIdFileName != null && contact.ConVoterIdFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path + "VoterIds/" + contact.ConVoterIdFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "VoterIds/" + contact.ConVoterIdFileName));
            }
            if (contact.ConPanFileName != null && contact.ConPanNumberFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path + "PANCards/" + contact.ConPanFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "PANCards/" + contact.ConPanFileName));
            }
            if (contact.ConAdhaarNumberFilePath != null)
                contact.ConAdhaarFileName = SaveImageFile(contact.ConAdhaarNumberFilePath, "ADHRS");
            if (contact.ConVoterIdFilePath != null)
                contact.ConVoterIdFileName = SaveImageFile(contact.ConVoterIdFilePath, "VoterIds");
            if (contact.ConPanNumberFilePath != null)
                contact.ConPanFileName = SaveImageFile(contact.ConPanNumberFilePath, "PANCards");
            var pwd = "CA5680";
            if (contact.IsUser)
                contact.Password = Encrypt_Decript_Code.encrypt_decrypt.Encrypt(pwd, true);
            contact.UserTypeId = 4;
            if (contact.ContactId != null)
                contact.Action = 'U';
            else
                contact.Action = 'I';
            contact.UserId = SessionModel.UserId;
            contact.CompanyId = SessionModel.CompanyId;
            if (TempData["provider"] != null)
            {
                var _Provider = TempData["provider"] as ServiceProviderModel;
                contact.RefKey = _Provider.ProviderId;
            }
            var response = await _contactPerson.AddUpdateContactDetails(contact);

            if (response.IsSuccess)
            {
                if (contact.Action == 'U')
                {
                    if (contact.IsUser && !contact.CurrentIsUser)
                    {
                        var Templates = await _templateRepo.GetTemplateByActionName("User Registration");
                        SessionModel.Email = contact.ConEmailAddress;
                        var WildCards = await CommonModel.GetWildCards();
                        var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                        U.Val = contact.ConFirstName;
                        U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                        U.Val = pwd;
                        U = WildCards.Where(x => x.Text.ToUpper() == "USER NAME").FirstOrDefault();
                        U.Val = contact.ConEmailAddress;
                        SessionModel.Mobile = contact.ConMobileNumber;
                        var c = WildCards.Where(x => x.Val != string.Empty).ToList();
                        if (Templates != null)
                            await _emailSmsServices.Send(Templates, c, SessionModel);
                    }
                }
                else
                {
                    if (contact.IsUser)
                    {
                        var Templates = await _templateRepo.GetTemplateByActionName("User Registration");
                        SessionModel.Email = contact.ConEmailAddress;
                        var WildCards = await CommonModel.GetWildCards();
                        var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                        U.Val = contact.ConFirstName;
                        U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                        U.Val = pwd;
                        U = WildCards.Where(x => x.Text.ToUpper() == "USER NAME").FirstOrDefault();
                        U.Val = contact.ConEmailAddress;
                        SessionModel.Mobile = contact.ConMobileNumber;
                        var c = WildCards.Where(x => x.Val != string.Empty).ToList();
                        if (Templates != null)
                            await _emailSmsServices.Send(Templates, c, SessionModel);
                    }

                }
            }

            TempData["response"] = response;

            if (TempData["Provider"] != null)
            {
                var Provider = TempData["Provider"] as ServiceProviderModel;
                contact.ContactId = new Guid(response.result);
                //var cityName = dropdown.BindLocation(contact.StateId).Where(x => x.Value == contact.CityId.ToString()).FirstOrDefault();
                //contact.City = cityName.Text;
                var Location = dropdown.BindLocationNew(contact.LocationId).FirstOrDefault();
                contact.LocationName = Location.Text;              
                Provider.ContactPersons.Add(contact);
                Provider.action = 'I';
                Provider.Activetab = "tab-4";
                TempData["provider"] = Provider;
                return View("Create", Provider);
            }
            else
            {
                var Provider = await GetProvider(contact.RefKey);
                Provider.action = 'U';
                Provider.Activetab = "tab-3";
                return View("Edit", Provider);
            }



        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Service_Provider)]
        public async Task<ActionResult> Create()
        {
            var ProviderModel = await GetProvider(null);
            return View(ProviderModel);
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]
        public async Task<ActionResult> ManageService(Guid RefKey, Guid? ServiceId)
        {
            var SessionModel = Session["User"] as SessionModel;
            var service = new ServiceModel();
            if (ServiceId != null)
            {
                service = await _services.GetService(new FilterModel { ServiceId = ServiceId });
                service.SupportedSubCategoryList = new SelectList(dropdown.BindSubCategory(service.CategoryId), "Value", "Text");

            }
            else
            {
                service.SupportedSubCategoryList = new SelectList(Enumerable.Empty<SelectList>());
                service.RefKey = RefKey;
            }

            service.SupportedCategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            service.ServiceList = new SelectList(await TogoFogo.CommonModel.GetServiceType(SessionModel.CompanyId), "Value", "Text");
            service.DeliveryServiceList = new SelectList(await TogoFogo.CommonModel.GetDeliveryServiceType(SessionModel.CompanyId), "Value", "Text");
            return View(service);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]

        [HttpPost]
        public async Task<ActionResult> ManageService(ServiceModel service)
        {
            if (service.ServiceId != null)
                service.EventAction = 'U';
            else
                service.EventAction = 'I';
            var response = await _services.AddEditServices(service);
            TempData["response"] = response;
            if (response.IsSuccess)
                return RedirectToAction("Edit", new { id = service.RefKey, @tab = "tab-6" });
            else
                return View(service);
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]

        public async Task<ActionResult> ManageServiceableAreaPinCode(Guid ServiceId)
        {
            var SessionModel = Session["User"] as SessionModel;
            var service = new ManageServiceModel();
            service.Services = await _services.GetServiceAreaPins(new FilterModel { ServiceId = ServiceId, FileId = null });
            service.Service = await _services.GetServiceOfferd(new FilterModel { ServiceId = ServiceId });
            service.Service.IsActive = false;
            service.BaseUrl = _fpath;
            service.Service.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            service.Service.StateList = new SelectList(Enumerable.Empty<SelectList>());
            service.Service.CityList = new SelectList(Enumerable.Empty<SelectList>());
            service.Service.PinCodeList = new SelectList(Enumerable.Empty<SelectList>());
            service.Files = await _RepoUploadFile.GetFiles(ServiceId);
            service.ImportModel = new ProviderFileModel();
            return View(service);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]

        public async Task<ActionResult> ServiceableAreaPinCode(Guid? ServiceId, Guid? FileId)
        {
            var SessionModel = Session["User"] as SessionModel;
            var service = await _services.GetServiceAreaPins(new FilterModel { ServiceId = ServiceId, FileId = FileId });
            return View(service);
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]

        public async Task<ActionResult> GetServiceAreaPinCode(Guid ServiceAreaId)
        {
            var SessionModel = Session["User"] as SessionModel;
            var service = await _services.GetServiceAreaPin(new FilterModel { ServiceAreaId = ServiceAreaId });
            return Json(service, JsonRequestBehavior.AllowGet);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]

        [HttpPost]
        public async Task<ActionResult> ManageServiceableAreaPinCode(ServiceOfferedModel service)
        {
            var SessionModel = Session["User"] as SessionModel;
            if (service.ServiceAreaId != null)
                service.EventAction = 'U';
            else
                service.EventAction = 'I';
            service.UserId = SessionModel.UserId;
            var response = await _services.AddOrEditServiceableAreaPin(service);

            TempData["response"] = response;

            var services = new ManageServiceModel();
            services.Services = await _services.GetServiceAreaPins(new FilterModel { ServiceId = service.ServiceId });
            services.Service = await _services.GetServiceOfferd(new FilterModel { ServiceId = service.ServiceId });
            services.Service.IsActive = false;
            services.Service.ServiceAreaId = null;
            services.Service.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            services.Service.StateList = new SelectList(dropdown.BindState(service.CountryId), "Value", "Text");
            services.Service.CityList = new SelectList(dropdown.BindDiscrictByPin(service.PinCode), "Text", "Text");
            services.Service.PinCodeList = new SelectList(dropdown.BindPinCodeParam(service.City + "," + service.StateId), "Text", "Text");
            services.Service.CountryId = service.CountryId;
            services.Service.StateId = service.StateId;
            services.Service.City = service.City;
            services.Files = new List<ProviderFileModel>();
            services.ImportModel = new ProviderFileModel();
            return View(services);
        }
        // POST: ManageClient/Create  
        [PermissionBasedAuthorize(new Actions[] { Actions.Create, Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]
        [HttpPost]
        public async Task<ActionResult> AddorEditServiceProvider(ServiceProviderModel provider)
        {
            var SessionModel = Session["User"] as SessionModel;
            var statutory = await CommonModel.GetStatutoryType();
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            var cltns = TempData["provider"] as ServiceProviderModel;
            provider.Organization = new OrganizationModel();


            provider.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
            provider.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            provider.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            provider.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            provider.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            provider.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            provider.Contact.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
            provider.Contact.CityList = new SelectList(await CommonModel.GetLookup("City"), "Value", "Text");
            //provider.Contact.LocationList = new SelectList(dropdown.BindLocation(), "Value", "Text");
            provider.Contact.LocationList = new SelectList(dropdown.BindLocationByPinCode(provider.Contact.PinNumber), "Value", "Text"); 
            provider.Service = new ServiceOfferedModel
            {
                SupportedCategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text"),
                SupportedSubCategoryList = new SelectList(dropdown.BindCountry(), "Value", "Text"),
                ServiceList = new SelectList(await TogoFogo.CommonModel.GetServiceType(SessionModel.CompanyId), "Value", "Text"),
                DeliveryServiceList = new SelectList(await TogoFogo.CommonModel.GetDeliveryServiceType(SessionModel.CompanyId), "Value", "Text"),
                CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text"),
                StateList = new SelectList(Enumerable.Empty<SelectList>()),
                CityList = new SelectList(Enumerable.Empty<SelectList>()),
                LocationList = new SelectList(Enumerable.Empty<SelectList>()),
            PinCodeList = new SelectList(Enumerable.Empty<SelectList>()),
            };

            if (TempData["provider"] != null)
            {
                cltns.CompanyId = provider.CompanyId;
                provider = cltns;
            }
            else
            {
                provider.Activetab = "tab-1";
                provider.CreatedBy = SessionModel.UserId;

            }

            if (SessionModel.UserTypeName.ToLower().Contains("super admin"))
            {
                provider.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
                provider.IsSuperAdmin = true;
            }
            else
                provider.CompanyId = SessionModel.CompanyId;
            provider.UserId = SessionModel.UserId;
            provider.Activetab = "tab-1";
            var response = await _provider.AddUpdateDeleteProvider(provider);
            _provider.Save();
            provider.ProviderId = new Guid(response.result);
            provider.Service.RefKey = provider.ProviderId;
            TempData["response"] = response;
            provider.Activetab = "tab-2";

            if (provider.action == 'I')
            {
                TempData["provider"] = provider;
                return View("Create", provider);

            }
            else
                return View("Edit", provider);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create, Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]
        [HttpPost]
        public async Task<ActionResult> AddorEditOrganization(ServiceProviderModel provider, OrganizationModel org)
        {
            var SessionModel = Session["User"] as SessionModel;
            var cltns = TempData["provider"] as ServiceProviderModel;
            if (TempData["provider"] != null)
            {
                provider = cltns;
                provider.Organization = org;
            }
            else
                provider.Organization = org;

            if (provider.Organization.OrgGSTNumberFilePath != null && provider.Organization.OrgGSTFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/UploadedImages/Providers/Gsts/" + provider.Organization.OrgGSTFileName)))
                    System.IO.File.Delete(Server.MapPath("~/UploadedImages/Providers/Gsts/" + provider.Organization.OrgGSTFileName));
            }
            if (provider.Organization.OrgPanNumberFilePath != null && provider.Organization.OrgPanFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/UploadedImages/Providers/PANCards/" + provider.Organization.OrgPanFileName)))
                    System.IO.File.Delete(Server.MapPath("~/UploadedImages/Providers/PANCards/" + provider.Organization.OrgPanFileName));
            }

            if (provider.Organization.OrgGSTNumberFilePath != null)
                provider.Organization.OrgGSTFileName = SaveImageFile(provider.Organization.OrgGSTNumberFilePath, "Gsts");
            if (provider.Organization.OrgPanNumberFilePath != null)
                provider.Organization.OrgPanFileName = SaveImageFile(provider.Organization.OrgPanNumberFilePath, "PANCards");
            var statutory = await CommonModel.GetStatutoryType();
            provider.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            provider.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            provider.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
            try
            {

                provider.Activetab = "tab-2";
                provider.CreatedBy = SessionModel.UserId;
                var response = await _provider.AddUpdateDeleteProvider(provider);
                _provider.Save();
                provider.ProviderId = new Guid(response.result);
                TempData["response"] = response;
                TempData.Keep("response");
                if (provider.action == 'I')
                {
                    provider.Activetab = "tab-3";
                    TempData["provider"] = provider;
                    TempData.Keep("provider");
                    return View("Create", provider);

                }
                else
                    return RedirectToAction("Index");



            }
            catch (Exception ex)
            {
                if (provider.action == 'I')
                    return View("Create", provider);
                else
                    return RedirectToAction("Index");
            }
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Create, Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]
        [HttpPost]
        public async Task<ActionResult> AddOrEditASPReg(ServiceProviderModel provider)
        {
            var SessionModel = Session["User"] as SessionModel;



            var cltns = TempData["provider"] as ServiceProviderModel;
            provider.Organization = new OrganizationModel();
            if (TempData["provider"] != null)
            {
                cltns.Remarks = provider.Remarks;
                cltns.IsActive = provider.IsActive;

                provider = cltns;
            }
            try
            {

                provider.Activetab = "tab-5";
                provider.CreatedBy = SessionModel.UserId;
                var response = await _provider.AddUpdateDeleteProvider(provider);
                _provider.Save();
                var _ASPModel = await GetProvider(provider.ProviderId);
                TempData["response"] = response;
                _ASPModel.Activetab = "tab-6";
                if (_ASPModel.action == 'I')
                    return View("Create", _ASPModel);
                else
                    return View("edit", _ASPModel);
            }
            catch (Exception ex)
            {
                if (provider.action == 'I')
                    return View("Create", provider);
                else
                    return View("edit", provider);
            }
        }


        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]
        public async Task<ActionResult> Edit(Guid id, string tab)
        {
            TempData["provider"] = null;
            var Provider = await GetProvider(id);
            if (!string.IsNullOrEmpty(tab))
                Provider.Activetab = tab;
            return View(Provider);

        }

        private async Task<ServiceProviderModel> GetProvider(Guid? ProviderId)
        {
            var SessionModel = Session["User"] as SessionModel;
            var Provider = await _provider.GetProviderById(ProviderId);

            Provider.Path = _path;



            if (Provider.Organization == null)
                Provider.Organization = new OrganizationModel();
            

            Provider.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            Provider.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            Provider.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            Provider.Service = new ServiceOfferedModel
            {
                SupportedCategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text"),
                SupportedSubCategoryList = new SelectList(dropdown.BindCountry(), "Value", "Text"),
                ServiceList = new SelectList(await TogoFogo.CommonModel.GetServiceType(SessionModel.CompanyId), "Value", "Text"),
                DeliveryServiceList = new SelectList(await TogoFogo.CommonModel.GetDeliveryServiceType(SessionModel.CompanyId), "Value", "Text"),
                CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text"),
                StateList = new SelectList(Enumerable.Empty<SelectList>()),
                CityList = new SelectList(Enumerable.Empty<SelectList>()),
               LocationList = new SelectList(Enumerable.Empty<SelectListItem>()),
            PinCodeList = new SelectList(Enumerable.Empty<SelectList>()),
                RefKey = ProviderId
            };


            Provider.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");

            Provider.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            Provider.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            Provider.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            Provider.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            Provider.Contact.LocationList = new SelectList(Enumerable.Empty<SelectList>());
            if (SessionModel.UserTypeName.ToLower().Contains("super admin"))
            {
                Provider.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
                Provider.IsSuperAdmin = true;

            }
            else
                Provider.CompanyId = SessionModel.CompanyId;
            if (ProviderId != null)
                Provider.action = 'U';
            else
                Provider.action = 'I';
            return Provider;
        }

        // POST: ManageClient/Edit/5
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]
        [HttpPost]
        public async Task<ActionResult> Edit(ServiceProviderModel provider, OrganizationModel org)
        {
            var SessionModel = Session["User"] as SessionModel;
            try
            {

                provider.Organization = new OrganizationModel();

                if (provider.Activetab.ToLower() == "tab-2")
                    provider.Organization = org;

                provider.CreatedBy = SessionModel.UserId;
                var response = await _provider.AddUpdateDeleteProvider(provider);
                _provider.Save();
                // TODO: Add insert logic here
                TempData["response"] = "Edited Successfully";

                // TODO: Add update logic here

                return RedirectToAction("index");


            }
            catch
            {

                return View(provider);
            }
        }
        [HttpPost]
        private string SaveFile(HttpPostedFileBase file, string folderName)
        {
            try
            {
                string path = Server.MapPath("~/Files/" + folderName);
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Service_Provider)]

        [HttpPost]
        public async Task<ActionResult> ImportProviders(ProviderFileModel provider)
        {
            var SessionModel = Session["User"] as SessionModel;
            provider.CompanyId = SessionModel.CompanyId;
            provider.UserId = SessionModel.UserId;

            if (provider.DataFile != null)
            {
                provider.FileName = SaveFile(provider.DataFile, "Providers");
                string excelPath = Server.MapPath("~/Files/Providers/" + provider.FileName);
                string conString = string.Empty;
                string extension = Path.GetExtension(provider.DataFile.FileName);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;

                }

                conString = string.Format(conString, excelPath);
                DataTable dtExcelData = new DataTable();
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();

                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    dtExcelData.Columns.AddRange(new DataColumn[25] {              
                new DataColumn("Service Provider Name", typeof(string)),
                new DataColumn("Organization Name", typeof(string)),
                new DataColumn("Organization Code", typeof(string)),
                new DataColumn("Organization IEC Number", typeof(string)),
                new DataColumn("Statutory Type", typeof(string)),
                new DataColumn("Applicable Tax Type", typeof(string)),
                new DataColumn("GST Category", typeof(string)),
                new DataColumn("GST Number", typeof(string)),
                 new DataColumn("PAN Card Number", typeof(string)),
                new DataColumn("IsServiceCenter", typeof(string)),
                new DataColumn("Contact Name", typeof(string)),
                new DataColumn("Contact Mobile", typeof(string)),
                new DataColumn("Contact Email", typeof(string)),
                new DataColumn("Contact PAN", typeof(string)),
                new DataColumn("Contact Voter Id", typeof(string)),
                new DataColumn("Contact Adhaar", typeof(string)),
                new DataColumn("Address Type", typeof(string)),
                new DataColumn("Country", typeof(string)),
                new DataColumn("State", typeof(string)),
                  new DataColumn("City", typeof(string)),
                  new DataColumn("Address", typeof(string)),
                     new DataColumn("Locality", typeof(string)),
                  new DataColumn("Near By Location", typeof(string)),
                   new DataColumn("Pin Code", typeof(string)),
                     new DataColumn("IsUser", typeof(string))
                });
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT  [Service Provider Name]," +
                     "[Organization Name],[Organization Code],[Organization IEC Number],[Statutory Type]," +
                        "[Applicable Tax Type],[GST Category],[GST Number],[PAN Card Number],[IsServiceCenter] ," +
                        "[Contact Name],[Contact Mobile],[Contact Email],[Contact PAN],[Contact Voter Id],[Contact Adhaar], " +
                        " [Address Type],[Country],[State],[City],[Address],[Locality],[Near By Location], " +
                        " [Pin Code], [IsUser] FROM [" + sheet1 + "] where  [Service Provider Name] is not null", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();

                }
                //try
                //{

                    var response = await _RepoUploadFile.UploadServiceProviders(provider, dtExcelData);
                    if (!response.IsSuccess)
                        System.IO.File.Delete(excelPath);
                    TempData["response"] = response;
                    return RedirectToAction("index");
                //}
                //catch (Exception ex)

                //{
                //    if (System.IO.File.Exists(excelPath))
                //        System.IO.File.Delete(excelPath);
                //    return RedirectToAction("index");

                //}
            }
            return RedirectToAction("index");

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]

        public async Task<ActionResult> ImportServiceableAreaPinCodes(ProviderFileModel provider)
        {
            var SessionModel = Session["User"] as SessionModel;
            provider.CompanyId = SessionModel.CompanyId;
            provider.UserId = SessionModel.UserId;

            if (provider.DataFile != null)
            {
                provider.SysFileName = SaveFile(provider.DataFile, "ServiceProviders");
                var excelPath = Server.MapPath("~/Files/ServiceProviders/");
                string conString = string.Empty;
                string extension = Path.GetExtension(provider.DataFile.FileName);
                provider.FileName = Path.GetFileName(provider.DataFile.FileName);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;

                }

                conString = string.Format(conString, excelPath + provider.SysFileName);
                DataTable dtExcelData = new DataTable();
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();

                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    dtExcelData.Columns.AddRange(new DataColumn[2] {
                new DataColumn("Pin Code", typeof(string)),
                   new DataColumn("Is Active", typeof(string))
                });
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [Pin Code],[IS Active]   FROM [" + sheet1 + "] where [Pin Code] is not null", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();

                }
                try
                {
                    var response = await _RepoUploadFile.UploadServiceableAreaPins(provider, dtExcelData);
                    if (!response.IsSuccess)
                        System.IO.File.Delete(excelPath);
                    TempData["response"] = response;

                    if(provider.type=="All")
                        return RedirectToAction("Edit", new { id = provider.RefKey, @tab = "tab-6" });
                    else
                        return RedirectToAction("ManageServiceableAreaPinCode", new { ServiceId = provider.RefKey});
                }
                catch (Exception ex)

                {
                    if (System.IO.File.Exists(excelPath))
                        System.IO.File.Delete(excelPath);
                    if (provider.type == "All")
                        return RedirectToAction("Edit", new { id = provider.RefKey, @tab = "tab-6" });
                    else
                        return RedirectToAction("ManageServiceableAreaPinCode", new { ServiceId = provider.RefKey });


                }
            }
            return RedirectToAction("index");

        }

        public ActionResult _GetUploadForm(Guid? RefKey)
        {
            return PartialView("~/Views/Common/_ImportForm.cshtml", new ProviderFileModel {type="All",RefKey=RefKey });
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.ExcelExport }, (int)MenuCode.Manage_Service_Provider)]
        public async Task<FileContentResult> ExportToExcel(Char tabIndex)
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId, tabIndex = tabIndex };            
            byte[] filecontent;
            string[] columns;
            if (tabIndex == 'T')
            {
                columns = new string[]{"ServiceProviderName"
                                ,"OrganizationName","OrganizationCode","OrganizationIECNumber","StatutoryType","ApplicableTaxType","GSTCategory", "GSTNumber","PANCardNumber",
                    "IsServiceCenter","ContactName","ContactMobile","ContactEmail","ContactPAN","ContactVoterId","ContactAdhaar",
                    "AddressType","Country","State","City","Address","Locality","NearByLocation","PinCode","IsUser"
            };
                var providerData = new List<serviceProviderData> { new serviceProviderData
                {
                    ServiceProviderName="Ambica services",                   
                    OrganizationName="Ambica Service PVT LTD",
                    OrganizationCode="ORG000015",
                    OrganizationIECNumber="IEC55588455",
                StatutoryType="Private Limited",
                ApplicableTaxType="Indian GST",
                GSTCategory="Services",
                GSTNumber="33ABCDE1234E1ZI",
                PANCardNumber="AYJPK5904C",
                IsServiceCenter ="Yes",
                ContactName="Gaurab Sharma",
                ContactMobile="8557854455",
                ContactEmail="gaurab@sharma.com",
                ContactPAN="AYJP5904C",
               AddressType="Home",
               Country="India",
               State="Uttar Pradesh",
               City="GAUTAM BUDDHA NAGAR",
               Address="Address",
               Locality="Noida",
               NearByLocation="Kailash Hospital",
               PinCode="201301",
               IsUser="No"
                }};
                filecontent = ExcelExportHelper.ExportExcel(providerData, "", false, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, "Excel.xlsx");
            }
            else
            {
                var response = await _provider.GetProvidersExcel(filter);
                columns = new string[]{"ServiceProviderCode","ServiceProviderName"
                                ,"OrganizationName","OrganizationCode","OrganizationIECNumber","StatutoryType","ApplicableTaxType","GSTCategory", "GSTNumber","PANCardNumber",
                    "IsServiceCenter","ContactName","ContactMobile","ContactEmail","ContactPAN","ContactVoterId","ContactAdhaar",
                    "AddressType","Country","State","City","Address","Locality","NearByLocation","PinCode","IsUser","ServiceableAreaPinCode" };               
                filecontent = ExcelExportHelper.ExportExcel(response, "", false, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, "Excel.xlsx");
            }
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.ExcelExport }, (int)MenuCode.Manage_Service_Provider)]
        public async Task<FileContentResult> AreaPincodeTemplate()
        {
            string[] columns = new string[] {"PinCode", "IsActive" };
            var providerData = new List<ServiceOfferedModel> { new ServiceOfferedModel
                {
                PinCode="281204",
                IsActive=true
                }};
            byte[] filecontent = ExcelExportHelper.ExportExcel(providerData, "", false, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "AreaPinCodeTemplate.xlsx");
        }

      

    }
}
