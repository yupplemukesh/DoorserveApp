using Newtonsoft.Json;
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
using TogoFogo.Repository.Clients;
using TogoFogo.Filters;

namespace TogoFogo.Controllers
{

    public class ManageClientsController : Controller
    {
        private readonly IClient _client;
        private readonly IBank _bank;
        private readonly IContactPerson _contactPerson;
        private readonly string _path = "/UploadedImages/Clients/";
        private readonly DropdownBindController dropdown;
        private SessionModel user;
        public ManageClientsController()
        {
            _client = new Client() ;
           dropdown= new DropdownBindController();
            _bank = new Bank();
            _contactPerson = new ContactPerson();
        }

        // GET: ManageClient
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Clients")]
        public  async Task<ActionResult> Index()
        {
            user =  Session["User"] as SessionModel;
            var filter = new FilterModel { CompId=user.CompanyId};
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

        [HttpPost]
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
                bank.Action = 'U';
            else
                bank.Action = 'I';
            bank.UserId = Convert.ToInt32(Session["User_ID"]);
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
        [HttpPost]
        public async Task<ActionResult> AddOrPersonContactDetails(ContactPersonModel contact)
        {
            user = Session["User"] as SessionModel;
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
            if (contact.IsUser)
                contact.Password = Encrypt_Decript_Code.encrypt_decrypt.Encrypt("CA5680", true);
            if (contact.ContactId != null)
                contact.Action = 'U';
            else
                contact.Action = 'I';

            if (TempData["client"] != null)
            {
                var Client = TempData["client"] as ClientModel;
                contact.RefKey = Client.ClientId;
            }
            contact.UserId = user.UserId;
            contact.CompanyId = user.CompanyId;
            contact.UserTypeId = 2;
            var response = await _contactPerson.AddUpdateContactDetails(contact);                       
            if (response.IsSuccess)
            {
                var mpc = new Email_send_code();
                Type type = mpc.GetType();
                var Status = (int)type.InvokeMember("sendmail_update",
                                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                                        BindingFlags.NonPublic, null, mpc,
                                        new object[] { contact.ConEmailAddress, contact.Password, contact.ConEmailAddress });
            }
            TempData["response"] = response;
            if (TempData["client"] != null)
            {
                var Client = TempData["client"] as ClientModel;
                contact.ContactId = new Guid(response.result);
                var cityName =  dropdown.BindLocation(contact.StateId).Where(x=> x.Value==contact.CityId.ToString()).FirstOrDefault();
                contact.City = cityName.Text;
                Client.ContactPersons.Add(contact);
                Client.action = 'I';
                Client.Activetab = "tab-4";
                TempData["client"] = Client;
                return View("Create", Client);
            }
            else
            {
                var Client = await GetClient(contact.RefKey);
                Client.action = 'U';
                Client.Activetab = "tab-3";
                return View("Edit", Client);
            }



        }
        private async Task<ClientModel> GetClient(Guid? clientId)
        {
            user = Session["User"] as SessionModel;
            var Client = await _client.GetClientByClientId(clientId);
            Client.Path = _path;
            var processes = await CommonModel.GetProcesses();           
                Client.ProcessList = new SelectList(processes, "Value", "Text");
            if (Client.Organization == null)
                Client.Organization = new OrganizationModel();
            Client.SupportedCategoryList = new SelectList(dropdown.BindCategory(user.CompanyId), "Value", "Text");
            Client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(user.CompanyId), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            Client.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            Client.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            Client.ServiceList = await TogoFogo.CommonModel.GetServiceType(user.CompanyId);
            Client.DeliveryServiceList = await TogoFogo.CommonModel.GetDeliveryServiceType(user.CompanyId);
            Client.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            Client.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            Client.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            Client.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            Client.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            if (clientId != null)
                Client.action = 'U';
            else
                Client.action = 'I';
            List<int> List = new List<int>();
            if (!string.IsNullOrEmpty(Client._deviceCategories))
            {
                var _deviceCat = Client._deviceCategories.Split(',');
                for (int i = 0; i < _deviceCat.Length; i++)
                {
                    List.Add(Convert.ToInt16(_deviceCat[i]));
                }
            }
            if (!string.IsNullOrEmpty(Client.ServiceDeliveryTypes))
            {
                var _DeliveryService = Client.ServiceDeliveryTypes.Split(',');
                for (int i = 0; i < _DeliveryService.Length; i++)
                {
                    var item = Client.DeliveryServiceList.Where(x => x.Value == Convert.ToInt32(_DeliveryService[i])).FirstOrDefault();
                    if (item != null)
                        item.IsChecked = true;

                }
            }
            if (!string.IsNullOrEmpty(Client.ServiceTypes))
            {
                var _serviceType = Client.ServiceTypes.Split(',');
                for (int i = 0; i < _serviceType.Length; i++)
                {
                    var item = Client.ServiceList.Where(x => x.Value == Convert.ToInt32(_serviceType[i])).FirstOrDefault();
                    if (item != null)
                        item.IsChecked = true;
                }
            }
            Client.DeviceCategories = List;
            return Client;
        }

        // GET: ManageClient/Create
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Clients")]
        public async Task<ActionResult> Create()
        {
            var clientModel = await GetClient(null);           
            return View(clientModel);
        }

        // POST: ManageClient/Create    
        [HttpPost]
        public async Task<ActionResult> AddorEditClient(ClientModel client)
        {
            user = Session["User"] as SessionModel;

            var statutory = await CommonModel.GetStatutoryType();
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            var cltns = TempData["client"] as ClientModel;
            client.Organization = new OrganizationModel();
            if (client.ServiceList.Where(x => x.IsChecked == true).Count() < 1 || client.DeliveryServiceList.Where(x => x.IsChecked == true).Count() < 1)
            {
                client.ProcessList = new SelectList(await CommonModel.GetProcesses(), "Value", "Text");
                client.SupportedCategoryList = new SelectList(dropdown.BindCategory(user.CompanyId), "Value", "Text");
                client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(user.CompanyId), "Value", "Text");
                client.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
                client.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
                client.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
                client.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
                client.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
                client.Contact.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
                client.Contact.CityList = new SelectList(await CommonModel.GetLookup("City"), "Value", "Text");

                if (client.action == 'I')
                    return View("Create", client);
                else
                    return View("Edit", client);
            }
            if (TempData["client"] != null )
            {
                    client = cltns;
             
                    string _servicetype = "";
                    foreach (var item in client.ServiceList)
                    {
                        if (item.IsChecked)
                            _servicetype = _servicetype + "," + item.Value;

                    }
                    _servicetype = _servicetype.TrimEnd(',');
                    _servicetype = _servicetype.TrimStart(',');

                    string __deliveryType = "";
                    foreach (var item in client.DeliveryServiceList)
                    {
                        if (item.IsChecked)
                            __deliveryType = __deliveryType + "," + item.Value;

                    }
                    __deliveryType = __deliveryType.TrimStart(',');
                    __deliveryType = __deliveryType.TrimEnd(',');
                    client.ServiceTypes = _servicetype;
                    client.ServiceDeliveryTypes = __deliveryType;
               
            }
            else
            {
              
                    string _servicetype = "";
                    foreach (var item in client.ServiceList)
                    {
                        if (item.IsChecked)
                            _servicetype = _servicetype + "," + item.Value;

                    }
                    _servicetype = _servicetype.TrimEnd(',');
                    _servicetype = _servicetype.TrimStart(',');

                    string __deliveryType = "";
                    foreach (var item in client.DeliveryServiceList)
                    {
                        if (item.IsChecked)
                            __deliveryType = __deliveryType + "," + item.Value;

                    }
                    __deliveryType = __deliveryType.TrimStart(',');
                    __deliveryType = __deliveryType.TrimEnd(',');
                    client.ServiceTypes = _servicetype;
                    client.ServiceDeliveryTypes = __deliveryType;
                
            }
            client.ProcessList = new SelectList(await  CommonModel.GetProcesses(), "Value", "Text");
            client.SupportedCategoryList = new SelectList(dropdown.BindCategory(user.CompanyId), "Value", "Text");
            client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(user.CompanyId), "Value", "Text");
            client.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            client.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            client.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            client.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            client.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            client.Contact.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
            client.Contact.CityList = new SelectList(await CommonModel.GetLookup("City"), "Value", "Text");
            try
            {
                    client.Activetab = "tab-1";
                    client.CreatedBy = user.UserId;
                    client.CompanyId = user.CompanyId;
                    var response = await _client.AddUpdateDeleteClient(client);
                    _client.Save();
                    client.ClientId = new Guid(response.result);
                TempData["response"] = response;
                TempData.Keep("response");
                if (client.action == 'I')
                {
                    client.Activetab = "tab-2";
                    TempData["client"] = client;
                    TempData.Keep("client");
                    return View("Create",client);
                }
               else
                    return RedirectToAction("Index");
                                                                                         
            }
            catch(Exception ex)
            {
                if(client.action=='I')
                return View("Create", client);
                else
                    return View("Edit", client);
            }
        }


        [HttpPost]
        public async Task<ActionResult> AddorEditOrganization(ClientModel client,OrganizationModel org)
        {
            user = Session["User"] as SessionModel;
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
            client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(user.CompanyId), "Value", "Text");
            try
            {

                client.Activetab = "tab-2";
                client.CreatedBy = user.UserId;
                client.CompanyId = user.CompanyId;
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
                    return View("Create",client);

                }
                else
                    return RedirectToAction("Index");



            }
            catch (Exception ex)
            {
                if (client.action == 'I')
                    return View("Create",client);
                 else
                    return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<ActionResult> AddOrEditClientReg(ClientModel client)
        {

            if (client.IsUser && !string.IsNullOrEmpty(client.Password))
                client.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(client.Password, true);

            var cltns = TempData["client"] as ClientModel;
            client.Organization = new OrganizationModel();
            if (TempData["client"] != null)
            {
                cltns.Remarks = client.Remarks;
                cltns.IsActive = client.IsActive;
                cltns.IsUser = client.IsUser;
                cltns.UserName = client.UserName;
                cltns.Password = client.Password;
                client = cltns;               
            }                    
            try
            {

                client.Activetab = "tab-5";
                client.CreatedBy = Convert.ToInt32(Session["User_ID"]);
                var response = await _client.AddUpdateDeleteClient(client);
                _client.Save();
                client.ClientId = new Guid(response.result);
                TempData["response"] = response;
                TempData.Keep("response");              
               return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                if (client.action == 'I')
                    return View("Create", client);
                else
                    return RedirectToAction("Index");
            }
        }

        // GET: ManageClient/Edit/5
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Clients")]
        public async Task<ActionResult> Edit(Guid id)
        {
            TempData["client"] = null;
            var client = await  GetClient(id);
            return View(client);

        }

      
    }
}
