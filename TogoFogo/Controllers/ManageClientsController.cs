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
using TogoFogo.Repository.Clients;

namespace TogoFogo.Controllers
{

    public class ManageClientsController : Controller
    {
        private readonly IClients _client;
       private readonly DropdownBindController dropdown;
        public ManageClientsController()
        {
            _client = new Clients() ;
           dropdown= new DropdownBindController();
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
                string path = Server.MapPath("~/Uploaded Images/"+ folderName);
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
        public  ActionResult AddorUpdateContact(ContactPersonModel model)
        {
            var Addresses =  CommonModel.GetAddressTypes();
            model.AddressTypelist = new SelectList(Addresses, "Value", "Text");
            model.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            return PartialView("~/views/common/_AddEditContactPerson.cshtml", model);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrEditBank(BankDetailModel bank)
        {

            if (TempData["client"] != null)
            {
                var client = TempData["client"] as ClientModel;
                bank.Action = 'I';
                var clnt = await _client.AddUpdateBankDetails(bank);
                var clientModel = await _client.GetClientByClientId(bank.ClientId);
               
                    clientModel.Activetab = "tab-5";
                    TempData["client"] = clientModel;
                    clientModel.action = 'I';
                    TempData.Keep("client");
                    return View("Create", client);
               
            }
            else
            {
                var clnt = await _client.AddUpdateBankDetails(bank);
                return RedirectToAction("edit", bank.ClientId);
            }

        }
        [HttpPost]
        public async Task<ActionResult> AddOrPersonContactDetails(ContactPersonModel contact)
        {
         
            
            if ( TempData["client"] !=null)
            {
              
                var client = TempData["client"] as ClientModel;
                contact.Action = 'I';
                var clnt = await _client.AddUpdateContactDetails(contact);
                var clientModel = await _client.GetClientByClientId(contact.ClientId);
              
                    clientModel.action = 'I';
                    clientModel.Activetab = "tab-4";
                    TempData["client"] = clientModel;
                    TempData.Keep("client");
                    return View("Create", clientModel);
               
             
                
            }
            return RedirectToAction("edit", contact.ClientId);
        }

        // GET: ManageClient/Create
        public async Task<ActionResult> Create()
        {
            var clientModel = new ClientModel() {Organization=new OrganizationModel(),
              
            };
          
            var processes = await CommonModel.GetProcesses();
            clientModel.ProcessList = new SelectList(processes, "Value", "Text");
            clientModel.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            clientModel.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            clientModel.Organization.SatatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            clientModel.Organization.AplicationTaxTypeList =  new SelectList(applicationTaxTypeList, "Value", "Text");
            clientModel.ServiceList = await TogoFogo.CommonModel.GetServiceType();
            clientModel.DeliveryServiceList = await TogoFogo.CommonModel.GetDeliveryServiceType();
            clientModel.action = 'I';
            return View(clientModel);
        }

        // POST: ManageClient/Create    
        [HttpPost]
        public async Task<ActionResult> Create(ClientModel client, OrganizationModel org)
        {
            client.action = 'I';
            var cltns = TempData["client"] as ClientModel;
   
            if (org.OrgName == null || cltns ==null)
            {

                
                client.Organization = new OrganizationModel();

                client.Activetab = "tab-2";

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
            else if(cltns.Activetab== "tab-1")
            {
                client = cltns;
                client.Organization = org;
                client.Activetab = "tab-2";

            }
            else if (cltns.Activetab == "tab-2")
            { 
                client = cltns;
                client.Organization = org;
                client.Activetab = "tab-3";

            }

            TempData["client"] = client;
            TempData.Keep("client");

            client.ProcessList = new SelectList(await CommonModel.GetProcesses(), "Value", "Text");     
            client.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            client.Organization.SatatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            client.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            try
            {


                    client.CreatedBy = Convert.ToInt32(Session["User_ID"]);              
                    var response = await _client.AddUpdateDeleteClient(client);
                    _client.Save();
                    client.ClientId = new Guid(response.result);
                    // TODO: Add insert logic here
                    TempData["response"] = response;
                    TempData.Keep("response");
                if (client.Activetab == "tab-5")
                    return RedirectToAction("Index");
                else
                    return View(client);
                              

            }
            catch(Exception ex)
            {

               

                return View(client);
            }
        }

        // GET: ManageClient/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            TempData["client"] = null;
            var client = await _client.GetClientByClientId(id);
            var _deviceCat = client._deviceCategories.Split(',');
            var processes = await CommonModel.GetProcesses();
            client.ProcessList = new SelectList(processes, "Value", "Text");
            client.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            client.Organization.SatatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            client.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            client.ServiceList = await TogoFogo.CommonModel.GetServiceType();
            client.DeliveryServiceList = await TogoFogo.CommonModel.GetDeliveryServiceType();

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
        public async Task<ActionResult> Edit(ClientModel client)
        {
            client.action = 'U';  
            client.SupportedCategoryList = new SelectList(dropdown.BindCategorySelectpicker(), "Value", "Text");          
            try
            {
                if (ModelState.IsValid)
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

                 
                    client.CreatedBy = Convert.ToInt32(Session["User_ID"]);
                    var response = await _client.AddUpdateDeleteClient(client);
                    _client.Save();
                    // TODO: Add insert logic here
                    TempData["response"] = "Edited Successfully";
                    TempData.Keep("response");
                    // TODO: Add update logic here

                    return RedirectToAction("edit",client.ClientId);
                }
                else
                {
                 

                    return View(client);
                }
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
