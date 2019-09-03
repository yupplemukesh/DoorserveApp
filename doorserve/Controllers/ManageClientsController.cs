using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using doorserve.Models;
using doorserve.Permission;
using doorserve.Repository;
using System.Reflection;
using doorserve.Repository.Clients;
using doorserve.Filters;
using doorserve.Repository.EmailSmsServices;

namespace doorserve.Controllers
{

    public class ManageClientsController : BaseController
    {
        private readonly IClient _client;
        private readonly IBank _bank;
        private readonly IContactPerson _contactPerson;
        private readonly IServices _service;
        private readonly string _path = "/UploadedImages/Clients/";
        private readonly DropdownBindController dropdown;
        private readonly doorserve.Repository.EmailSmsTemplate.ITemplate _templateRepo;
        private readonly IEmailSmsServices _emailSmsServices;
        public ManageClientsController()
        {
            _client = new Client() ;
           dropdown= new DropdownBindController();
            _bank = new Bank();
            _contactPerson = new ContactPerson();
            _templateRepo = new doorserve.Repository.EmailSmsTemplate.Template();
            _emailSmsServices = new Repository.EmailsmsServices();
            _service = new Services();
        }

        // GET: ManageClient
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Clients)]
        public  async Task<ActionResult> Index()
        {
            Guid? ClientId = null;
            if (CurrentUser.UserTypeName.ToLower().Contains("client"))
                ClientId = CurrentUser.RefKey;
            var filter = new FilterModel { CompId= CurrentUser.CompanyId, ClientId=ClientId };
            var clients=  await _client.GetClients(filter);  
            return View(clients);
        }
        private string SaveImageFile(HttpPostedFileBase file,string folderName)
        {
            try
            {
                string path = Server.MapPath(_path+""+ folderName);
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
        [PermissionBasedAuthorize(new Actions[] {Actions.Edit }, (int)MenuCode.Manage_Clients)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddOrEditBank(BankDetailModel bank)
        {

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
            bank.UserId = CurrentUser.UserId;
            if (TempData["client"] != null)
            {
                var Client = TempData["client"] as ClientModel;
                bank.RefKey = Client.ClientId;
            }
            var response = await _bank.AddUpdateBankDetails(bank);
            TempData["response"] = response;
            if (TempData["client"] != null)
            {
                var Client = TempData["client"] as ClientModel;
                bank.bankId = new Guid(response.result);
                var name = Client.Bank.BankList.Where(x => x.Value == bank.BankNameId.ToString()).FirstOrDefault();
                bank.BankName = name.Text;
                Client.BankDetails.Add(bank);
                Client.action = 'I';
                Client.Activetab = "tab-5";
                TempData["client"] = Client;
                return View("Create", Client);
            }
            else
            {
                var Client = await GetClient(bank.RefKey);
                Client.action = 'U';
                Client.Activetab = "tab-4";
                return View("Edit", Client);
            }
        }
        [PermissionBasedAuthorize(new Actions[] {Actions.Edit }, (int)MenuCode.Manage_Clients)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddOrPersonContactDetails(OtherContactPersonModel contact)
        {
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
            var pwd = CommonModel.RandomPassword(8);
            if (contact.IsUser)
                contact.Password = Encrypt_Decript_Code.encrypt_decrypt.Encrypt(pwd, true);
            if (contact.ContactId != null)
                contact.EventAction = 'U';
            else
                contact.EventAction = 'I';
            var Client = TempData["client"] as ClientModel;
            if (TempData["client"] != null)                
                contact.RefKey =Client.ClientId;
                
           
            contact.UserId = CurrentUser.UserId;
            contact.CompanyId = CurrentUser.CompanyId;
            contact.UserTypeId = 2;
            var response = await _contactPerson.AddUpdateContactDetails(contact);                       
            if (response.IsSuccess)
            {
                if (contact.EventAction == 'U')
                {
                    if (contact.IsUser && !contact.CurrentIsUser)
                    {
                        var Templates = await _templateRepo.GetTemplateByActionId(12, CurrentUser.CompanyId);
                        CurrentUser.Email = contact.ConEmailAddress;
                        var WildCards = await CommonModel.GetWildCards(CurrentUser.CompanyId);
                        var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                        U.Val = contact.ConFirstName;
                        U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                        U.Val = pwd;
                        U = WildCards.Where(x => x.Text.ToUpper() == "USER NAME").FirstOrDefault();
                        U.Val = contact.ConEmailAddress;
                        CurrentUser.Mobile = contact.ConMobileNumber;
                        var c = WildCards.Where(x => x.Val != string.Empty).ToList();
                        if (Templates.Count> 0)
                            await _emailSmsServices.Send(Templates, c, CurrentUser);
                    }
                }
                else
                {
                    if (contact.IsUser)
                    {
                        var Templates = await _templateRepo.GetTemplateByActionId(12, CurrentUser.CompanyId);
                        CurrentUser .Email= contact.ConEmailAddress;
                        var WildCards = await CommonModel.GetWildCards(CurrentUser.CompanyId);
                        var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                        U.Val = contact.ConFirstName;
                        U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                        U.Val = pwd;
                        U = WildCards.Where(x => x.Text.ToUpper() == "USER NAME").FirstOrDefault();
                        U.Val = contact.ConEmailAddress;
                        CurrentUser.Mobile = contact.ConMobileNumber;
                        var c = WildCards.Where(x => x.Val != string.Empty).ToList();
                        if (Templates != null)
                            await _emailSmsServices.Send(Templates, c, CurrentUser);
                    }

                }
            }
            TempData["response"] = response;
            if (TempData["client"] != null)
            {
                contact.ContactId = new Guid(response.result);                
                var Location = dropdown.BindLocationNew(contact.LocationId).FirstOrDefault();
                contact.LocationName = Location.Text;
                Client.ContactPersons.Add(contact);
                Client.action = 'I';
                Client.Activetab = "tab-4";
                TempData["client"] = Client;
                return View("Create", Client);
            }
            else
            {
                Client = await GetClient(contact.RefKey);
                Client.action = 'U';
                Client.Activetab = "tab-3";
                return View("Edit", Client);
            }



        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Clients)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddOrEditService(ServiceModel service)
        {
            if (service.ServiceId != null)
                service.EventAction = 'U';
            else
                service.EventAction = 'I';
            var Client = TempData["client"] as ClientModel;
            if (TempData["client"] != null)
                service.RefKey = Client.ClientId;          
            var response = await _service.AddEditServices(service);
            TempData["response"] = response;
            if (TempData["client"] != null)
            {
                service.ServiceId = new Guid(response.result);
                service.Category = dropdown.BindCategory(CurrentUser.CompanyId).Where(x=>Convert.ToInt32(x.Value)==service.CategoryId).FirstOrDefault().Text;
                service.SubCategory = dropdown.BindSubCategory(service.CategoryId).Where(x => x.Value == service.SubCategoryId.ToString()).FirstOrDefault().Text;
                var services = await CommonModel.GetServiceType(CurrentUser.CompanyId);
                service.ServiceType= services.Where(x => x.Value == service.ServiceTypeId).FirstOrDefault().Text;
                var Deliveries = await CommonModel.GetDeliveryServiceType(CurrentUser.CompanyId);
                service.ServiceType = Deliveries.Where(x => Convert.ToInt32(x.Value) == service.DeliveryTypeId).FirstOrDefault().Text;              
                Client.Services.Add(service);
                Client.action = 'I';
                Client.Activetab = "tab-6";
                TempData["client"] = Client;
                return View("Create", Client);
            }
            else
            {
                Client = await GetClient(service.RefKey);
                Client.action = 'U';
                Client.Activetab = "tab-5";
                return View("Edit", Client);
            }



        }
        private async Task<ClientModel> GetClient(Guid? clientId)
        {
            var Client = await _client.GetClientByClientId(clientId);
            Client.Path = _path;
                           
            if (Client.Organization == null)
                Client.Organization = new OrganizationModel();
            Client.ProcessList = new SelectList(await CommonModel.GetProcesses(CurrentUser.CompanyId), "Value", "Text");
        
            Client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            Client.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            Client.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
        
            Client.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            Client.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            Client.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            Client.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            Client.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            Client.Contact.LocationList= new SelectList(Enumerable.Empty<SelectList>());
            Client.Service = new ServiceModel
            {
                SupportedCategoryList = new SelectList(dropdown.BindCategory(CurrentUser.CompanyId), "Value", "Text"),
                SupportedSubCategoryList = new SelectList(Enumerable.Empty<SelectList>()),
                ServiceList = new SelectList(await doorserve.CommonModel.GetServiceType(CurrentUser.CompanyId), "Value", "Text"),
                DeliveryServiceList = new SelectList(await doorserve.CommonModel.GetDeliveryServiceType(CurrentUser.CompanyId), "Value", "Text"),
                RefKey = clientId                    
            };
            if (CurrentUser.UserTypeName.ToLower().Contains("super admin"))
            {
                Client.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
                Client.IsSuperAdmin = true;
            }
          
            if (clientId != null)
                Client.action = 'U';
            else
                Client.action = 'I';
          
                
            return Client;
        }

        // GET: ManageClient/Create
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Clients)]
        public async Task<ActionResult> Create()
        {
            var clientModel = await GetClient(null);           
            return View(clientModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Clients)]
        public async Task<JsonResult> GetService(Guid? serviceId)
        {
            var ServiceModel = await _service.GetService(new FilterModel { ServiceId=serviceId});
            return Json (ServiceModel,JsonRequestBehavior.AllowGet);
        }
        // POST: ManageClient/Create  
        [PermissionBasedAuthorize(new Actions[] {Actions.Edit }, (int)MenuCode.Manage_Clients)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddorEditClient(ClientModel client)
        {
            var statutory = await CommonModel.GetStatutoryType();
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            var cltns = TempData["client"] as ClientModel;
            client.Organization = new OrganizationModel();

            if (TempData["client"] != null)
                client = cltns;
            else
            {
                client.ProcessList = new SelectList(await CommonModel.GetProcesses(CurrentUser.CompanyId), "Value", "Text");
                client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
                client.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
                client.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
                client.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
                client.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
                client.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
                client.Contact.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
                //client.Contact.CityList = new SelectList(await CommonModel.GetLookup("City"), "Value", "Text");
                client.Contact.LocationList = client.Contact.LocationList = new SelectList(dropdown.BindLocationByPinCode(client.Contact.PinNumber), "Value", "Text");
                if (CurrentUser.UserTypeName.ToLower().Contains("super admin"))
                {
                    client.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
                    client.IsSuperAdmin = true;
                }
                client.Service = new ServiceModel
                {
                    SupportedCategoryList = new SelectList(dropdown.BindCategory(CurrentUser.CompanyId), "Value", "Text"),
                    SupportedSubCategoryList = new SelectList(Enumerable.Empty<SelectList>()),
                    ServiceList = new SelectList(await doorserve.CommonModel.GetServiceType(CurrentUser.CompanyId), "Value", "Text"),
                    DeliveryServiceList = new SelectList(await doorserve.CommonModel.GetDeliveryServiceType(CurrentUser.CompanyId), "Value", "Text"),                 
                    ServiceId = null,
                    SubCategoryId = 0,
                    CategoryId = 0,
                    ServiceTypeId = 0,
                    DeliveryTypeId = 0,
                    Remarks = null,
                    IsActive = false

                };
              
                    client.Activetab = "tab-1";
                    client.CreatedBy = CurrentUser.UserId;
                    if(!CurrentUser.UserTypeName.ToLower().Contains("super admin"))
                    client.CompanyId = CurrentUser.CompanyId;
                              
            }
            var response = await _client.AddUpdateDeleteClient(client);
            _client.Save();
            client.ClientId = new Guid(response.result);
            client.Service.RefKey = client.ClientId;
            TempData["response"] = response;
            client.Activetab = "tab-2";
            if (client.action == 'I')
            {               
                TempData["client"] = client;
                TempData.Keep("client");
                return View("Create", client);
            }
            else
            return View("Edit", client);
        }

        [PermissionBasedAuthorize(new Actions[] {Actions.Edit }, (int)MenuCode.Manage_Clients)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddorEditOrganization(ClientModel client,OrganizationModel org)
        {
            var cltns = TempData["client"] as ClientModel;
            if (TempData["client"] != null)
            {
                client = cltns;
                client.Organization = org;                
            }
            else          
                client.Organization = org;

            if (client.Organization.OrgGSTNumberFilePath != null && client.Organization.OrgGSTFileName != null)
            {
                if(System.IO.File.Exists(Server.MapPath(_path+"Gsts/" + client.Organization.OrgGSTFileName)))
                   System.IO.File.Delete(Server.MapPath(_path+"Gsts/" + client.Organization.OrgGSTFileName));
            }
            if (client.Organization.OrgPanNumberFilePath != null && client.Organization.OrgPanFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath(_path+"PANCards/" + client.Organization.OrgPanFileName)))
                    System.IO.File.Delete(Server.MapPath(_path+"PANCards/" + client.Organization.OrgPanFileName));                
            }

            if (client.Organization.OrgGSTNumberFilePath != null)
                client.Organization.OrgGSTFileName = SaveImageFile(client.Organization.OrgGSTNumberFilePath, "Gsts");
            if (client.Organization.OrgPanNumberFilePath != null)
                client.Organization.OrgPanFileName = SaveImageFile(client.Organization.OrgPanNumberFilePath, "PANCards");
            var statutory = await CommonModel.GetStatutoryType();
            client.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            client.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
      

            client.Activetab = "tab-2";
            client.CreatedBy = CurrentUser.UserId;
            client.CompanyId = CurrentUser.CompanyId;
            var response = await _client.AddUpdateDeleteClient(client);
            _client.Save();
            client.ClientId = new Guid(response.result);
            TempData["response"] = response;
            TempData.Keep("response");
            if (client.action == 'I')
            {
                client.Activetab = "tab-3";
                TempData["client"] = client;
                TempData.Keep("client");
                return View("Create", client);

            }
            else
            {
                var clt = await GetClient(client.ClientId);
                     return View("Edit", clt);
            }
               
        }

        [PermissionBasedAuthorize(new Actions[] {Actions.Edit }, (int)MenuCode.Manage_Clients)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddOrEditClientReg(ClientModel client)
        {

       

            var cltns = TempData["client"] as ClientModel;
            client.Organization = new OrganizationModel();
            if (TempData["client"] != null)
            {
                cltns.Remarks = client.Remarks;
                cltns.IsActive = client.IsActive;    
                client = cltns;               
            }

            client.CreatedBy = CurrentUser.UserId;
            client.CompanyId = CurrentUser.CompanyId;
            client.Activetab = "tab-5";

            var response = await _client.AddUpdateDeleteClient(client);
            _client.Save();
            client.ClientId = new Guid(response.result);         
            TempData["response"] = response;
            return RedirectToAction("Index");
        }

        // GET: ManageClient/Edit/5
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Clients)]
        public async Task<ActionResult> Edit(Guid id)
        {
            TempData["client"] = null;
            var client = await  GetClient(id);
            return View(client);

        }

      
    }
}
