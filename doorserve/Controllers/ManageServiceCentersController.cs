using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Permission;
using doorserve.Repository;
using doorserve.Repository.ServiceCenters;
using System.Reflection;
using doorserve.Repository.ServiceProviders;
using doorserve.Repository.EmailSmsServices;
using doorserve.Repository.ImportFiles;
using System.Data.OleDb;
using System.Configuration;
using System.Data;

namespace doorserve.Controllers
{

    public class ManageServiceCentersController : Controller
    {
        private readonly ICenter _Center;
        private readonly IProvider _Provider;
        private readonly IBank _bank;
        private readonly IContactPerson _contactPerson;
        private readonly string _path = "/UploadedImages/Centers/";
        private readonly string _fpath = "/Files/ServiceCenters/";
        private readonly DropdownBindController dropdown;
        private readonly doorserve.Repository.EmailSmsTemplate.ITemplate _templateRepo;
        private readonly IEmailSmsServices _emailSmsServices;
        private readonly IServices _services;
        private readonly IUploadFiles _RepoUploadFile;

        public ManageServiceCentersController()
        {
            _Center = new Center() ;
             dropdown= new DropdownBindController();
            _bank = new Bank();
            _contactPerson = new ContactPerson();
            _Provider = new Provider();
            _templateRepo = new doorserve.Repository.EmailSmsTemplate.Template();
            _emailSmsServices = new Repository.EmailsmsServices();
            _services = new Services();
            _RepoUploadFile = new UploadFiles();
        }


        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Service_Center_TRC)]
        public  async Task<ActionResult> Index()
        {
            var SessionModel = Session["User"] as SessionModel;
            Guid? ProviderId = null;
            Guid? CenterId = null;
            if (SessionModel.UserTypeName.ToLower().Contains("provider"))
                ProviderId = SessionModel.RefKey;
           if (SessionModel.UserTypeName.ToLower().Contains("center"))
                CenterId = SessionModel.RefKey;
            var filter = new FilterModel { CompId = SessionModel.CompanyId,RefKey= CenterId, ProviderId= ProviderId };
            var Centers = await _Center.GetCenters(filter);           
            return View(Centers);
        }
        private string SaveImageFile(HttpPostedFileBase file,string folderName)
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
        [PermissionBasedAuthorize(new Actions[] {Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddOrEditBank(BankDetailModel bank)
        {
            var SessionModel = Session["User"] as SessionModel;
            if (bank.BankCancelledChequeFilePath != null && bank.BankCancelledChequeFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path+"Banks/" + bank.BankCancelledChequeFileName)))
                    System.IO.File.Delete(Server.MapPath(_path+"Banks/" + bank.BankCancelledChequeFileName));
            }

            if (bank.BankCancelledChequeFilePath != null)
                bank.BankCancelledChequeFileName = SaveImageFile(bank.BankCancelledChequeFilePath, "Banks");
            if (bank.bankId != null)
                bank.EventAction = 'U';
            else
                bank.EventAction = 'I';
              bank.UserId = SessionModel.UserId;
            if (TempData["center"] != null)
            {
                var _center = TempData["center"] as ServiceCenterModel;
                bank.RefKey = _center.CenterId;
            }
            var response = await _bank.AddUpdateBankDetails(bank);
            TempData["response"] = response;
            if (TempData["center"] != null)
            {
                var Center = TempData["center"] as ServiceCenterModel;
                bank.bankId = new Guid(response.result);
                var name = Center.Bank.BankList.Where(x => x.Value == bank.BankNameId.ToString()).FirstOrDefault();
                bank.BankName = name.Text;
                Center.BankDetails.Add(bank);
                Center.action = 'I';
                Center.Activetab = "tab-5";
                TempData["center"] = Center;
                return View("Create", Center);
            }
            else
            {
                var Center = await GetCenter(bank.RefKey);
                Center.action = 'U';
                Center.Activetab = "tab-4";
                return View("Edit", Center);
            }       
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddOrPersonContactDetails(OtherContactPersonModel contact)
        {
            var SessionModel = Session["User"] as SessionModel;

            if (contact.ConAdhaarNumberFilePath != null && contact.ConAdhaarFileName != null)    
            {   

                if (System.IO.File.Exists(Server.MapPath(_path+"ADHRS/" + contact.ConAdhaarFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "ADHRS/" + contact.ConAdhaarFileName));
            }
            if (contact.ConVoterIdFileName != null && contact.ConVoterIdFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path+"VoterIds/" + contact.ConVoterIdFileName)))
                    System.IO.File.Delete(Server.MapPath(_path+"VoterIds/" + contact.ConVoterIdFileName));
            }
            if (contact.ConPanFileName != null && contact.ConPanNumberFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path+"PANCards/" + contact.ConPanFileName)))
                    System.IO.File.Delete(Server.MapPath(_path+"PANCards/" + contact.ConPanFileName));
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
            contact.UserTypeId = 3;
            if (contact.ContactId != null)
                contact.Action = 'U';
            else
                contact.Action = 'I';
            contact.UserId = SessionModel.UserId;
            contact.CompanyId = SessionModel.CompanyId;
            
            if (TempData["center"] != null)
            {
                var _center = TempData["center"] as ServiceCenterModel;
                contact.RefKey = _center.CenterId;
            }

        
            var response = await _contactPerson.AddUpdateContactDetails(contact);

            if (response.IsSuccess)
            {
                if (contact.Action == 'U')
                {
                    if (contact.IsUser && !contact.CurrentIsUser)
                    {
                        var Templates = await _templateRepo.GetTemplateByActionName("User Registration",SessionModel.CompanyId);
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
                        var Templates = await _templateRepo.GetTemplateByActionName("User Registration",SessionModel.CompanyId);
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
            if (TempData["center"] != null)
            {
                var Center = TempData["center"] as ServiceCenterModel;
                contact.ContactId = new Guid(response.result);
                var Location = dropdown.BindLocationNew(contact.LocationId).FirstOrDefault();
                contact.LocationName = Location.Text;
                Center.ContactPersons.Add(contact);
                Center.action = 'I';
                Center.Activetab = "tab-4";
                TempData["center"] = Center;
                return View("Create", Center);
            }
            else
            {
                var Center = await GetCenter(contact.RefKey);                
                Center.action = 'U';
                Center.Activetab = "tab-3";
                return View("Edit", Center);
            }

          

               }
        private async Task<ServiceCenterModel> GetCenter(Guid ? centerId)
        {

            var SessionModel = Session["User"] as SessionModel;
            var Center = await _Center.GetCenterById(centerId);
            Center.Path = _path;

            if (SessionModel.UserRole.ToLower().Contains("provider"))
            {
                var providerId = SessionModel.RefKey;
                var provider = await _Provider.GetProviderById(providerId);
                Center.IsProvider = true;
            }

            else if (SessionModel.UserRole.ToLower().Contains("company"))
            {
                Center.IsCompany = true;
                Center.ProviderList = new SelectList(await CommonModel.GetServiceProviders(SessionModel.CompanyId), "Name", "Text");

            }
            else
            {
                Center.IsProvider = false;
                Center.IsCompany = false;
                Center.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
                if(Center.CenterId !=null)
                    Center.ProviderList = new SelectList(await CommonModel.GetServiceProviders(Center.CompanyId), "Name", "Text");
                else
                Center.ProviderList= new SelectList(Enumerable.Empty<SelectList>());
            }

            if (Center.Organization == null)
                Center.Organization = new OrganizationModel();
            Center.SupportedCategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            Center.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            Center.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            Center.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            Center.ServiceList = await doorserve.CommonModel.GetServiceType(SessionModel.CompanyId);
            Center.DeliveryServiceList = await doorserve.CommonModel.GetDeliveryServiceType(SessionModel.CompanyId);
            Center.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            Center.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            Center.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            Center.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            Center.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            Center.Contact.LocationList = new SelectList(Enumerable.Empty<SelectListItem>());

            if (centerId != null)
                Center.action = 'U';
            
            else
                Center.action = 'I';
            List<int> List = new List<int>();
            if (!string.IsNullOrEmpty(Center._deviceCategories))
            {
                var _deviceCat = Center._deviceCategories.Split(',');
                for (int i = 0; i < _deviceCat.Length; i++)
                {
                    List.Add(Convert.ToInt16(_deviceCat[i]));
                }
            }
            if (!string.IsNullOrEmpty(Center.ServiceDeliveryTypes))
            {
                var _DeliveryService = Center.ServiceDeliveryTypes.Split(',');
                for (int i = 0; i < _DeliveryService.Length; i++)
                {
                    var item = Center.DeliveryServiceList.Where(x => x.Value == Convert.ToInt32(_DeliveryService[i])).FirstOrDefault();
                    if (item != null)
                        item.IsChecked = true;

                }
            }
            if (!string.IsNullOrEmpty(Center.ServiceTypes))
            {
                var _serviceType = Center.ServiceTypes.Split(',');
                for (int i = 0; i < _serviceType.Length; i++)
                {
                    var item = Center.ServiceList.Where(x => x.Value == Convert.ToInt32(_serviceType[i])).FirstOrDefault();
                    if (item != null)
                        item.IsChecked = true;
                }
            }
            Center.DeviceCategories = List;
            return Center;
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Service_Center_TRC)]
        public async Task<ActionResult> Create()
        {
            var CenterModel = await GetCenter(null);
            return View(CenterModel);
        }
        [PermissionBasedAuthorize(new Actions[] {Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddorEditServiceCenter(ServiceCenterModel Center)
        {
            var SessionModel = Session["User"] as SessionModel;
            var statutory = await CommonModel.GetStatutoryType();
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            var cltns = TempData["center"] as ServiceCenterModel;     
            Center.Organization = new OrganizationModel();           
            Center.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
            Center.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            Center.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            Center.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            Center.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            Center.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            Center.Contact.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
            Center.Contact.CityList = new SelectList(await CommonModel.GetLookup("City"), "Value", "Text");
            Center.Contact.LocationList = new SelectList(dropdown.BindLocation(), "Value", "Text");
            Center.ProviderList = new SelectList(await CommonModel.GetServiceProviders(SessionModel.CompanyId), "Name", "Text");
            Center.Service = new ServiceOfferedModel
            {
                SupportedCategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text"),
                SupportedSubCategoryList = new SelectList(dropdown.BindCountry(), "Value", "Text"),
                ServiceList = new SelectList(await doorserve.CommonModel.GetServiceType(SessionModel.CompanyId), "Value", "Text"),
                DeliveryServiceList = new SelectList(await doorserve.CommonModel.GetDeliveryServiceType(SessionModel.CompanyId), "Value", "Text"),
                CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text"),
                StateList = new SelectList(Enumerable.Empty<SelectList>()),
                CityList = new SelectList(Enumerable.Empty<SelectList>()),
                LocationList = new SelectList(Enumerable.Empty<SelectList>()),
                PinCodeList = new SelectList(Enumerable.Empty<SelectList>()),
            };


            if (SessionModel.UserTypeName.ToLower().Contains("super admin"))
            {
                Center.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
                
            }
            else if (SessionModel.UserTypeName.ToLower().Contains("company"))
            {
                Center.IsCompany = true;
                Center.ProviderList = new SelectList(await CommonModel.GetServiceProviders(SessionModel.CompanyId), "Name", "Text");
                Center.CompanyId = SessionModel.CompanyId;
            }
            

            
            if (TempData["center"] != null)
            {
                Center = cltns;
            }


            // if (!SessionModel.UserTypeName.ToLower().Contains("super admin"))
            //Center.CompanyId = SessionModel.CompanyId;
            Center.Activetab = "tab-1";
            Center.CreatedBy = SessionModel.UserId;            
            var response = await _Center.AddUpdateDeleteCenter(Center);
            _Center.Save();
            Center.CenterId = new Guid(response.result);
            Center.Service.RefKey = Center.CenterId;

            TempData["response"] = response;
            TempData.Keep("response");

            if (Center.action == 'I')
            {
                Center.Activetab = "tab-2";
                TempData["center"] = Center;
                TempData.Keep("center");
                return View("Create", Center);

            }
            else
                return View("Edit", Center);
        }

        [PermissionBasedAuthorize(new Actions[] {Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddorEditOrganization(ServiceCenterModel Center,OrganizationModel org)
        {
            var SessionModel = Session["User"] as SessionModel;
            var cltns = TempData["center"] as ServiceCenterModel;
            if (TempData["center"] != null)
            {
                Center = cltns;
                Center.Organization = org;                
            }
            else          
                Center.Organization = org;

            if (Center.Organization.OrgGSTNumberFilePath != null && Center.Organization.OrgGSTFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path+"Gsts/" + Center.Organization.OrgGSTFileName)))
                    System.IO.File.Delete(Server.MapPath(_path+_path+"Gsts/" + Center.Organization.OrgGSTFileName));
            }
            if (Center.Organization.OrgPanNumberFilePath != null && Center.Organization.OrgPanFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path+"PANCards/" + Center.Organization.OrgPanFileName)))
                    System.IO.File.Delete(Server.MapPath(_path + "PANCards/" + Center.Organization.OrgPanFileName));
            }

            if (Center.Organization.OrgGSTNumberFilePath != null)
                Center.Organization.OrgGSTFileName = SaveImageFile(Center.Organization.OrgGSTNumberFilePath, "Gsts");
            if (Center.Organization.OrgPanNumberFilePath != null)
                Center.Organization.OrgPanFileName = SaveImageFile(Center.Organization.OrgPanNumberFilePath, "PANCards");
            var statutory = await CommonModel.GetStatutoryType();
            Center.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            Center.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            Center.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
        

            try
            {

                Center.Activetab = "tab-2";
                Center.CreatedBy = SessionModel.UserId;
                var response = await _Center.AddUpdateDeleteCenter(Center);
                _Center.Save();
                Center.CenterId = new Guid(response.result);
                TempData["response"] = response;
                TempData.Keep("response");
                if (Center.action == 'I')
                {
                    Center.Activetab = "tab-3";
                    TempData["center"] = Center;
                    TempData.Keep("center");
                    return View("Create",Center);

                }
                else
                    return RedirectToAction("Index");



            }
            catch (Exception ex)
            {
                if (Center.action == 'I')
                    return View("Create",Center);
                 else
                    return RedirectToAction("Index");
            }
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddOrEditClientReg(ServiceCenterModel Center)
        {

            var SessionModel = Session["User"] as SessionModel;


            var cltns = TempData["center"] as ServiceCenterModel;
            Center.Organization = new OrganizationModel();
            if (TempData["center"] != null)
            {
                cltns.Remarks = Center.Remarks;
                cltns.IsActive = Center.IsActive;            
                Center = cltns;               
            }                    
            try
            {

                Center.Activetab = "tab-5";
                Center.CreatedBy = SessionModel.UserId;
                var response = await _Center.AddUpdateDeleteCenter(Center);
                _Center.Save();
                Center.CenterId = new Guid(response.result);
                TempData["response"] = response;
                TempData.Keep("response");              
               return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                if (Center.action == 'I')
                    return View("Create", Center);
                else
                    return RedirectToAction("Index");
            }
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        public async Task<ActionResult> Edit(Guid id)
        {
            TempData["center"] = null;
            var Center = await GetCenter(id);





            return View(Center);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
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
            service.ServiceList = new SelectList(await doorserve.CommonModel.GetServiceType(SessionModel.CompanyId), "Value", "Text");
            service.DeliveryServiceList = new SelectList(await doorserve.CommonModel.GetDeliveryServiceType(SessionModel.CompanyId), "Value", "Text");
            return View(service);

        }


        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]

        [HttpPost]
        [ValidateModel]
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

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]

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

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]

        public async Task<ActionResult> ServiceableAreaPinCode(Guid? ServiceId, Guid? FileId)
        {
            var SessionModel = Session["User"] as SessionModel;
            var service = await _services.GetServiceAreaPins(new FilterModel { ServiceId = ServiceId, FileId = FileId });
            return View(service);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]

        public async Task<ActionResult> GetServiceAreaPinCode(Guid ServiceAreaId)
        {
            var SessionModel = Session["User"] as SessionModel;
            var service = await _services.GetServiceAreaPin(new FilterModel { ServiceAreaId = ServiceAreaId });
            return Json(service, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateModel]
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

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]

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
               // new DataColumn("Country", typeof(string)),
               // new DataColumn("State", typeof(string)),
               // new DataColumn("District", typeof(string)),
                new DataColumn("Pin Code", typeof(string)),
                   new DataColumn("Is Active", typeof(string))
                });
                    //using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [Country],[State], [District],[Pin Code],[IS Active] FROM [" + sheet1 + "] where [Country] is not null", excel_con))
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [Pin Code],[IS Active] FROM [" + sheet1 + "] where [Pin Code] is not null", excel_con))
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
                    return RedirectToAction("ManageServiceableAreaPinCode", new { ServiceId = provider.RefKey });
                }
                catch (Exception ex)

                {
                    if (System.IO.File.Exists(excelPath))
                        System.IO.File.Delete(excelPath);
                    return RedirectToAction("ManageServiceableAreaPinCode", new { ServiceId = provider.RefKey });


                }
            }
            return RedirectToAction("index");

        }

        public ActionResult _GetUploadForm()
        {
            return PartialView("~/Views/Common/_ImportForm.cshtml", new ProviderFileModel());
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.ExcelExport }, (int)MenuCode.Manage_Service_Center_TRC)]
        public async Task<FileContentResult> AreaPincodeTemplate()
        {
            string[] columns = new string[] { "PinCode", "IsActive" };
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
