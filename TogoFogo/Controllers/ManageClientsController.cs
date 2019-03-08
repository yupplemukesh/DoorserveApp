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
using TogoFogo.Repository;
using TogoFogo.Repository.Clients;

namespace TogoFogo.Controllers
{

    public class ManageClientsController : Controller
    {
        private readonly IClient _client;
        private readonly IBank _bank;
        private readonly IContactPerson _contactPerson;

        private readonly DropdownBindController dropdown;
        public ManageClientsController()
        {
            _client = new Client() ;
           dropdown= new DropdownBindController();
            _bank = new Bank();
            _contactPerson = new ContactPerson();
        }

        // GET: ManageClient
        public  async Task<ActionResult> Index()
        {
            var clients = await _client.GetClients();
            return View(clients);
        }
        private string SaveImageFile(HttpPostedFileBase file,string folderName)
        {
            try
            {
                string path = Server.MapPath("~/Uploaded Images/Clients/"+ folderName);
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

        public ActionResult AddorEditBankDetails(BankDetailModel model)
        {
            var banks=  CommonModel.GetBanks();
            model.BankList = new SelectList(banks, "Value", "Text");
            if (model.bankId != null)
                model.Action = 'U';
            else
                model.Action = 'I';

            if (TempData["client"] != null)
                TempData.Keep("client");
            return PartialView("~/views/common/_AddOrUpdateBankDetails.cshtml",model);
        }

        public ActionResult DeleteBank(BankDetailModel model)
        {
            var bnk = TempData["BankDetails"] as List<BankDetailModel>;
            bnk.Remove(model);
            TempData["BankDetails"] = bnk;
            TempData.Keep("BankDetails");
            return Json("Sucessfully", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteContact(ContactPersonModel model)
        {
            var conts = TempData["ContactPersons"] as List<ContactPersonModel>;
            conts.Remove(model);
            TempData["ContactPersons"] = conts;
            TempData.Keep("ContactPersons");
            return Json("Sucessfully", JsonRequestBehavior.AllowGet);
        }

        public  ActionResult AddorUpdateContact(string model)
        {
            var contactPerson = JsonConvert.DeserializeObject<ContactPersonModel>(model);
         
            var Addresses = CommonModel.GetAddressTypes();
            if (contactPerson.ConAddress == null)
                contactPerson.ConAddress = new AddressDetail();
            contactPerson.ConAddress.AddressTypelist = new SelectList(Addresses, "Value", "Text");
            contactPerson.ConAddress.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");

            if (contactPerson.ContactId != null)
            {
                contactPerson.Action = 'U';
                contactPerson.ConAddress.StateList = new SelectList(dropdown.BindState(contactPerson.ConAddress.CountryId), "Value", "Text");
                contactPerson.ConAddress.CityList = new SelectList(dropdown.BindLocation(contactPerson.ConAddress.StateId), "Value", "Text");

            }
            else
            {

                contactPerson.Action = 'I';
            
                contactPerson.ConAddress.CityList = new SelectList(Enumerable.Empty<SelectListItem>());
                contactPerson.ConAddress.StateList = new SelectList(Enumerable.Empty<SelectListItem>());

            }


            if (TempData["client"] != null)
                TempData.Keep("client");
                return PartialView("~/views/common/_AddEditContactPerson.cshtml", contactPerson);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrEditBank(BankDetailModel bank)
        {

            if (bank.BankCancelledChequeFilePath != null && bank.BankCancelledChequeFileName != null)
            {  
                if(System.IO.File.Exists(Server.MapPath("~/Uploaded Images/Clients/Banks/" + bank.BankCancelledChequeFileName)))
                System.IO.File.Delete(Server.MapPath("~/Uploaded Images/Clients/Banks/" + bank.BankCancelledChequeFileName));
            } 

            if (bank.BankCancelledChequeFilePath != null)
                bank.BankCancelledChequeFileName = SaveImageFile(bank.BankCancelledChequeFilePath, "Banks");
            

            bank.UserId = Convert.ToInt32(Session["User_ID"]);
            if (TempData["client"] != null)
            {             
                var client = TempData["client"] as ClientModel;
                var Response = await _bank.AddUpdateBankDetails(bank);
                var clientModel = await _client.GetClientByClientId(bank.RefKey);
                clientModel.ProcessList = client.ProcessList;
                clientModel.SupportedCategoryList = client.SupportedCategoryList;
                clientModel.DeviceCategories = client.DeviceCategories;
                clientModel.Organization.GstCategoryList = client.Organization.GstCategoryList;
                clientModel.Organization.StatutoryList = client.Organization.StatutoryList;
                clientModel.Organization.AplicationTaxTypeList = client.Organization.AplicationTaxTypeList;
                clientModel.ServiceList = client.ServiceList;
                clientModel.DeliveryServiceList = client.DeliveryServiceList;
                clientModel.Activetab = "tab-5";                    
                    TempData["client"] = clientModel;
                    TempData.Keep("client");
                    TempData["response"] = Response;
                    TempData.Keep("response");
                    return View("Create", clientModel);
               
            }
            else
            {
               
                var response=  await _bank.AddUpdateBankDetails(bank);
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("edit", "manageClients", new { id = bank.RefKey });
            }

        }
        [HttpPost]
        public async Task<ActionResult> AddOrPersonContactDetails(ContactPersonModel contact)
        {


            if (contact.ConAdhaarNumberFilePath != null && contact.ConAdhaarFileName != null)
            {

                if  (System.IO.File.Exists(Server.MapPath("~/Uploaded Images/Clients/ADHRS/" + contact.ConAdhaarFileName)))
                     System.IO.File.Delete(Server.MapPath("~/Uploaded Images/Clients/ADHRS/" + contact.ConAdhaarFileName));
            }
            if (contact.ConVoterIdFileName != null && contact.ConVoterIdFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Uploaded Images/Clients/VoterIds/" + contact.ConVoterIdFileName)))
                    System.IO.File.Delete(Server.MapPath("~/Uploaded Images/Clients/VoterIds/" + contact.ConVoterIdFileName));
            }
            if (contact.ConPanFileName != null && contact.ConPanNumberFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Uploaded Images/Clients/PANCards/" + contact.ConPanFileName)))
                    System.IO.File.Delete(Server.MapPath("~/Uploaded Images/Clients/PANCards/" + contact.ConPanFileName));      
            }
         
            if (contact.ConAdhaarNumberFilePath != null)
                contact.ConAdhaarFileName = SaveImageFile(contact.ConAdhaarNumberFilePath, "ADHRS");
            if (contact.ConVoterIdFilePath != null)
                contact.ConVoterIdFileName = SaveImageFile(contact.ConVoterIdFilePath, "VoterIds");
            if (contact.ConPanNumberFilePath != null)
                contact.ConPanFileName = SaveImageFile(contact.ConPanNumberFilePath, "PANCards");
            if (TempData["client"] != null)
            {
                var client = TempData["client"] as ClientModel;
                if (contact.ContactId != null)
                    contact.Action = 'U';
                else
                {
                    contact.Action = 'I';
                    contact.RefKey = client.ClientId ?? Guid.Empty;
                }            
                var response = await _contactPerson.AddUpdateContactDetails(contact);
                var clientModel = await _client.GetClientByClientId(contact.RefKey);
                clientModel.ProcessList = client.ProcessList;
                clientModel.DeviceCategories = client.DeviceCategories;
                clientModel.SupportedCategoryList = client.SupportedCategoryList;
                clientModel.Organization.GstCategoryList = client.Organization.GstCategoryList;              
                clientModel.Organization.StatutoryList = client.Organization.StatutoryList;          
                clientModel.Organization.AplicationTaxTypeList = client.Organization.AplicationTaxTypeList;
                clientModel.ServiceList = client.ServiceList;
                clientModel.DeliveryServiceList = client.DeliveryServiceList;
                try
                {
                   
                    

                    clientModel.action = 'I';
                    clientModel.Activetab = "tab-4";
                    TempData["client"] = clientModel;
                    TempData.Keep("client");
                    TempData["response"] = response;
                    TempData.Keep("response");
                    return View("Create", clientModel);
                }
                catch
                {

                    TempData["client"] = clientModel;
                    TempData.Keep("client");
                    return View("Create", clientModel);
                }



            }
            else
            {
               


                if (contact.ContactId != null)
                    contact.Action = 'U';
                else
                    contact.Action = 'I';
                var response = await _contactPerson.AddUpdateContactDetails(contact);
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("edit", "manageClients", new { id=contact.RefKey});
   
            }
        }

        // GET: ManageClient/Create
        public async Task<ActionResult> Create()
        {
            var clientModel = new ClientModel()
            {
                Organization = new OrganizationModel(),

            };
            
                var processes = await CommonModel.GetProcesses();
                clientModel.ProcessList = new SelectList(processes, "Value", "Text");
                clientModel.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
                clientModel.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
                var statutory = await CommonModel.GetStatutoryType();
                clientModel.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
                var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
                clientModel.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
                clientModel.ServiceList = await CommonModel.GetServiceType();
                clientModel.DeliveryServiceList = await CommonModel.GetDeliveryServiceType();
                clientModel.action = 'I';
            
            return View(clientModel);
        }

        // POST: ManageClient/Create    
        [HttpPost]
        public async Task<ActionResult> AddorEditClient(ClientModel client)
        {

          
            var cltns = TempData["client"] as ClientModel;
            client.Organization = new OrganizationModel();
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
            client.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            client.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            client.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            try
            {
                    client.Activetab = "tab-1";
                    client.CreatedBy = Convert.ToInt32(Session["User_ID"]);              
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
                if(System.IO.File.Exists(Server.MapPath("~/Uploaded Images/Clients/Gsts/" + client.Organization.OrgGSTFileName)))
                   System.IO.File.Delete(Server.MapPath("~/Uploaded Images/Clients/Gsts/" + client.Organization.OrgGSTFileName));
            }
            if (client.Organization.OrgPanNumberFilePath != null && client.Organization.OrgPanFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Uploaded Images/Clients/PANCards/" + client.Organization.OrgPanFileName)))
                    System.IO.File.Delete(Server.MapPath("~/Uploaded Images/Clients/PANCards/" + client.Organization.OrgPanFileName));                
            }

            if (client.Organization.OrgGSTNumberFilePath != null)
                client.Organization.OrgGSTFileName = SaveImageFile(client.Organization.OrgGSTNumberFilePath, "Gsts");
            if (client.Organization.OrgPanNumberFilePath != null)
                client.Organization.OrgPanFileName = SaveImageFile(client.Organization.OrgPanNumberFilePath, "PANCards");
            var statutory = await CommonModel.GetStatutoryType();
            client.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            client.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            try
            {

                client.Activetab = "tab-2";
                client.CreatedBy = Convert.ToInt32(Session["User_ID"]);
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
        public async Task<ActionResult> Edit(Guid id)
        {
            TempData["client"] = null;
            var client = await _client.GetClientByClientId(id);
            if (client.Organization == null)
                client.Organization = new OrganizationModel();
            var _deviceCat = client._deviceCategories.Split(',');
            var processes = await CommonModel.GetProcesses();
            client.ProcessList = new SelectList(processes, "Value", "Text");
            client.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            client.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            client.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            client.ServiceList = await TogoFogo.CommonModel.GetServiceType();
            client.DeliveryServiceList = await TogoFogo.CommonModel.GetDeliveryServiceType();
            client.action = 'U';
            List<int> List        =new  List<int>(); 
            for (int i = 0; i < _deviceCat.Length; i++)
            {
                List.Add(Convert.ToInt16(_deviceCat[i]));

            }
            var _DeliveryService = client.ServiceDeliveryTypes.Split(',');
            for (int i = 0; i < _DeliveryService.Length; i++)
            {
                    var item = client.DeliveryServiceList.Where(x => x.Value == Convert.ToInt32( _DeliveryService[i])).FirstOrDefault();
                    if(item !=null)                
                    item.IsChecked = true;

             }

            var _serviceType = client.ServiceTypes.Split(',');
            for (int i = 0; i < _serviceType.Length; i++)
            {
                var item = client.ServiceList.Where(x => x.Value == Convert.ToInt32(_serviceType[i])).FirstOrDefault();
                if (item != null)
                    item.IsChecked = true;
            }
            client.DeviceCategories = List;
            client.action = 'U';
            return View(client);

        }

        // POST: ManageClient/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ClientModel client, OrganizationModel org)
        {
            
            try
            {

                client.Organization = new OrganizationModel();
                if (client.Activetab.ToLower() == "tab-1")
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
                else if (client.Activetab.ToLower() == "tab-2")
                    client.Organization = org;
                
                    client.CreatedBy = Convert.ToInt32(Session["User_ID"]);
                    var response = await _client.AddUpdateDeleteClient(client);
                    _client.Save();
                    // TODO: Add insert logic here
                    TempData["response"] = "Edited Successfully";
                    TempData.Keep("response");
                    // TODO: Add update logic here

                    return RedirectToAction("index");
                
               
            }
            catch
            {
                
                return View(client);
            }
        }


       
        // GET: ManageClient/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var client = new ClientModel();
                client.ClientId = id;
                var response = await _client.AddUpdateDeleteClient(client);
                _client.Save();
            }
            catch
            {
            }

            return RedirectToAction("Index");
        }
    }
}
