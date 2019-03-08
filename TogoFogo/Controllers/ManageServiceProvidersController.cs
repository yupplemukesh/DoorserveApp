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
using TogoFogo.Repository.ServiceProviders;

namespace TogoFogo.Controllers
{

    public class ManageServiceProvidersController : Controller
    {
        private readonly IProvider _provider;
        private readonly IBank _bank;
        private readonly IContactPerson _contactPerson;

        private readonly DropdownBindController dropdown;
        public ManageServiceProvidersController()
        {
            _provider = new Provider() ;
             dropdown= new DropdownBindController();
            _bank = new Bank();
            _contactPerson = new ContactPerson();
        }

        // GET: ManageClient
        public  async Task<ActionResult> Index()
        {
            var Providers = await _provider.GetProviders();
            return View(Providers);
        }
        private string SaveImageFile(HttpPostedFileBase file,string folderName)
        {
            try
            {
                string path = Server.MapPath("~/Uploaded Images/Providers/"+ folderName);
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

            if (TempData["provider"] != null)
                TempData.Keep("provider");
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


            if (TempData["provider"] != null)
                TempData.Keep("provider");
                return PartialView("~/views/common/_AddEditContactPerson.cshtml", contactPerson);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrEditBank(BankDetailModel bank)
        {

            if (bank.BankCancelledChequeFilePath != null && bank.BankCancelledChequeFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Uploaded Images/Providers/Banks/" + bank.BankCancelledChequeFileName)))
                    System.IO.File.Delete(Server.MapPath("~/Uploaded Images/Providers/Banks/" + bank.BankCancelledChequeFileName));
            }

            if (bank.BankCancelledChequeFilePath != null)
                bank.BankCancelledChequeFileName = SaveImageFile(bank.BankCancelledChequeFilePath, "Banks");

            bank.UserId = Convert.ToInt32(Session["User_ID"]);
            if (TempData["provider"] != null)
            {             
                var provider = TempData["provider"] as ServiceProviderModel;
                var Response = await _bank.AddUpdateBankDetails(bank);
                var ProviderModel = await _provider.GetProviderById(bank.RefKey);
                ProviderModel.ProcessList = provider.ProcessList;
                ProviderModel.SupportedCategoryList = provider.SupportedCategoryList;
                ProviderModel.Organization.GstCategoryList = provider.Organization.GstCategoryList;
                ProviderModel.Organization.StatutoryList = provider.Organization.StatutoryList;
                ProviderModel.DeviceCategories = provider.DeviceCategories;
                ProviderModel.Organization.AplicationTaxTypeList = provider.Organization.AplicationTaxTypeList;
                ProviderModel.ServiceList = provider.ServiceList;
                ProviderModel.DeliveryServiceList = provider.DeliveryServiceList;
                ProviderModel.Activetab = "tab-5";                    
                    TempData["provider"] = ProviderModel;
                    TempData.Keep("provider");
                    TempData["response"] = Response;
                    TempData.Keep("response");
                    return View("Create", ProviderModel);
               
            }
            else
            {
               
                var response=  await _bank.AddUpdateBankDetails(bank);
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("edit", new { id = bank.RefKey });
            }

        }
        [HttpPost]
        public async Task<ActionResult> AddOrPersonContactDetails(ContactPersonModel contact)
        {


            if (contact.ConAdhaarNumberFilePath != null && contact.ConAdhaarFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Uploaded Images/ServiceProviders/ADHRS/" + contact.ConAdhaarFileName)))
                    System.IO.File.Delete(Server.MapPath("~/Uploaded Images/ServiceProviders/ADHRS/" + contact.ConAdhaarFileName));
            }
            if (contact.ConVoterIdFileName != null && contact.ConVoterIdFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Uploaded Images/ServiceProviders/VoterIds/" + contact.ConVoterIdFileName)))
                    System.IO.File.Delete(Server.MapPath("~/Uploaded Images/ServiceProviders/VoterIds/" + contact.ConVoterIdFileName));
            }
            if (contact.ConPanFileName != null && contact.ConPanNumberFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Uploaded Images/ServiceProviders/PANCards/" + contact.ConPanFileName)))
                    System.IO.File.Delete(Server.MapPath("~/Uploaded Images/ServiceProviders/PANCards/" + contact.ConPanFileName));
            }

            if (contact.ConAdhaarNumberFilePath != null)
                contact.ConAdhaarFileName = SaveImageFile(contact.ConAdhaarNumberFilePath, "ADHRS");
            if (contact.ConVoterIdFilePath != null)
                contact.ConVoterIdFileName = SaveImageFile(contact.ConVoterIdFilePath, "VoterIds");
            if (contact.ConPanNumberFilePath != null)
                contact.ConPanFileName = SaveImageFile(contact.ConPanNumberFilePath, "PANCards");
            if (TempData["provider"] != null)
            {
                var provider = TempData["provider"] as ServiceProviderModel;
                if (contact.ContactId != null)
                    contact.Action = 'U';
                else
                {
                    contact.Action = 'I';
                    contact.RefKey = provider.ProviderId ?? Guid.Empty;
                }            
                var response = await _contactPerson.AddUpdateContactDetails(contact);
                var ProviderModel = await _provider.GetProviderById(contact.RefKey);
                ProviderModel.ProcessList = provider.ProcessList;
                ProviderModel.DeviceCategories = provider.DeviceCategories;
                ProviderModel.SupportedCategoryList = provider.SupportedCategoryList;
                ProviderModel.Organization.GstCategoryList = provider.Organization.GstCategoryList;              
                ProviderModel.Organization.StatutoryList = provider.Organization.StatutoryList;          
                ProviderModel.Organization.AplicationTaxTypeList = provider.Organization.AplicationTaxTypeList;
                ProviderModel.ServiceList = provider.ServiceList;
                ProviderModel.DeliveryServiceList = provider.DeliveryServiceList;
                try
                {
                   
                    

                    ProviderModel.action = 'I';
                    ProviderModel.Activetab = "tab-4";
                    TempData["provider"] = ProviderModel;
                    TempData.Keep("provider");
                    TempData["response"] = response;
                    TempData.Keep("response");
                    return View("Create", ProviderModel);
                }
                catch
                {

                    TempData["provider"] = ProviderModel;
                    TempData.Keep("provider");
                    return View("Create", ProviderModel);
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
                return RedirectToAction("edit", new { id=contact.RefKey});
   
            }
        }

        // GET: ManageClient/Create
        public async Task<ActionResult> Create()
        {
            var ProviderModel = new ServiceProviderModel()
            {
                Organization = new OrganizationModel(),

            };
            
                var processes = await CommonModel.GetProcesses();
                ProviderModel.ProcessList = new SelectList(processes, "Value", "Text");
                ProviderModel.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
                ProviderModel.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
                var statutory = await CommonModel.GetStatutoryType();
                ProviderModel.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
                var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
                ProviderModel.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
                ProviderModel.ServiceList = await CommonModel.GetServiceType();
                ProviderModel.DeliveryServiceList = await CommonModel.GetDeliveryServiceType();
                ProviderModel.action = 'I';
            
            return View(ProviderModel);
        }

        // POST: ManageClient/Create    
        [HttpPost]
        public async Task<ActionResult> AddorEditServiceProvider(ServiceProviderModel provider)
        {

          
            var cltns = TempData["provider"] as ServiceProviderModel;
            provider.Organization = new OrganizationModel();
            if (TempData["provider"] != null )
            {
                    provider = cltns;
             
                    string _servicetype = "";
                    foreach (var item in provider.ServiceList)
                    {
                        if (item.IsChecked)
                            _servicetype = _servicetype + "," + item.Value;

                    }
                    _servicetype = _servicetype.TrimEnd(',');
                    _servicetype = _servicetype.TrimStart(',');

                    string __deliveryType = "";
                    foreach (var item in provider.DeliveryServiceList)
                    {
                        if (item.IsChecked)
                            __deliveryType = __deliveryType + "," + item.Value;

                    }
                    __deliveryType = __deliveryType.TrimStart(',');
                    __deliveryType = __deliveryType.TrimEnd(',');
                    provider.ServiceTypes = _servicetype;
                    provider.ServiceDeliveryTypes = __deliveryType;
               
            }
            else
            {
              
                    string _servicetype = "";
                    foreach (var item in provider.ServiceList)
                    {
                        if (item.IsChecked)
                            _servicetype = _servicetype + "," + item.Value;

                    }
                    _servicetype = _servicetype.TrimEnd(',');
                    _servicetype = _servicetype.TrimStart(',');

                    string __deliveryType = "";
                    foreach (var item in provider.DeliveryServiceList)
                    {
                        if (item.IsChecked)
                            __deliveryType = __deliveryType + "," + item.Value;

                    }
                    __deliveryType = __deliveryType.TrimStart(',');
                    __deliveryType = __deliveryType.TrimEnd(',');
                    provider.ServiceTypes = _servicetype;
                    provider.ServiceDeliveryTypes = __deliveryType;
                
            }
            provider.ProcessList = new SelectList(await  CommonModel.GetProcesses(), "Value", "Text");
            provider.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            provider.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            provider.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            provider.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            try
            {
                    provider.Activetab = "tab-1";
                    provider.CreatedBy = Convert.ToInt32(Session["User_ID"]);              
                    var response = await _provider.AddUpdateDeleteProvider(provider);
                    _provider.Save();
                    provider.ProviderId = new Guid(response.result);
                TempData["response"] = response;
                TempData.Keep("response");
                if (provider.action == 'I')
                {
                    provider.Activetab = "tab-2";
                    TempData["provider"] = provider;
                    TempData.Keep("provider");
                    return View("Create",provider);
                }
               else
                    return RedirectToAction("Index");
                                                                                         
            }
            catch(Exception ex)
            {
                if(provider.action=='I')
                return View("Create", provider);
                else
                    return View("Edit", provider);
            }
        }


        [HttpPost]
        public async Task<ActionResult> AddorEditOrganization(ServiceProviderModel provider,OrganizationModel org)
        {
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
                if (System.IO.File.Exists(Server.MapPath("~/Uploaded Images/Providers/Gsts/" + provider.Organization.OrgGSTFileName)))
                    System.IO.File.Delete(Server.MapPath("~/Uploaded Images/Providers/Gsts/" + provider.Organization.OrgGSTFileName));
            }
            if (provider.Organization.OrgPanNumberFilePath != null && provider.Organization.OrgPanFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Uploaded Images/Providers/PANCards/" + provider.Organization.OrgPanFileName)))
                    System.IO.File.Delete(Server.MapPath("~/Uploaded Images/Providers/PANCards/" + provider.Organization.OrgPanFileName));
            }

            if (provider.Organization.OrgGSTNumberFilePath != null)
                provider.Organization.OrgGSTFileName = SaveImageFile(provider.Organization.OrgGSTNumberFilePath, "Gsts");
            if (provider.Organization.OrgPanNumberFilePath != null)
                provider.Organization.OrgPanFileName = SaveImageFile(provider.Organization.OrgPanNumberFilePath, "PANCards");
            var statutory = await CommonModel.GetStatutoryType();
            provider.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            provider.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            provider.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            try
            {

                provider.Activetab = "tab-2";
                provider.CreatedBy = Convert.ToInt32(Session["User_ID"]);
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
                    return View("Create",provider);

                }
                else
                    return RedirectToAction("Index");



            }
            catch (Exception ex)
            {
                if (provider.action == 'I')
                    return View("Create",provider);
                 else
                    return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<ActionResult> AddOrEditClientReg(ServiceProviderModel provider)
        {

            if (provider.IsUser && !string.IsNullOrEmpty(provider.Password))
                provider.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(provider.Password, true);

            var cltns = TempData["provider"] as ServiceProviderModel;
            provider.Organization = new OrganizationModel();
            if (TempData["provider"] != null)
            {
                cltns.Remarks = provider.Remarks;
                cltns.IsActive = provider.IsActive;
                cltns.IsUser = provider.IsUser;
                cltns.UserName = provider.UserName;
                cltns.Password = provider.Password;
                provider = cltns;               
            }                    
            try
            {

                provider.Activetab = "tab-5";
                provider.CreatedBy = Convert.ToInt32(Session["User_ID"]);
                var response = await _provider.AddUpdateDeleteProvider(provider);
                _provider.Save();
                provider.ProviderId = new Guid(response.result);
                TempData["response"] = response;
                TempData.Keep("response");              
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

        // GET: ManageClient/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            TempData["provider"] = null;
            var provider = await _provider.GetProviderById(id);
            if (provider.Organization == null)
                provider.Organization = new OrganizationModel();
            var _deviceCat = provider._deviceCategories.Split(',');
            var processes = await CommonModel.GetProcesses();
            provider.ProcessList = new SelectList(processes, "Value", "Text");
            provider.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            provider.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            provider.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            provider.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            provider.ServiceList = await TogoFogo.CommonModel.GetServiceType();
            provider.DeliveryServiceList = await TogoFogo.CommonModel.GetDeliveryServiceType();
            provider.action = 'U';
            List<int> List        =new  List<int>(); 
            for (int i = 0; i < _deviceCat.Length; i++)
            {
                List.Add(Convert.ToInt16(_deviceCat[i]));

            }
            var _DeliveryService = provider.ServiceDeliveryTypes.Split(',');
            for (int i = 0; i < _DeliveryService.Length; i++)
            {
                    var item = provider.DeliveryServiceList.Where(x => x.Value == Convert.ToInt32( _DeliveryService[i])).FirstOrDefault();
                    if(item !=null)                
                    item.IsChecked = true;

             }

            var _serviceType = provider.ServiceTypes.Split(',');
            for (int i = 0; i < _serviceType.Length; i++)
            {
                var item = provider.ServiceList.Where(x => x.Value == Convert.ToInt32(_serviceType[i])).FirstOrDefault();
                if (item != null)
                    item.IsChecked = true;
            }
            provider.DeviceCategories = List;
            provider.action = 'U';
            return View(provider);

        }

        // POST: ManageClient/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ServiceProviderModel provider, OrganizationModel org)
        {
            
            try
            {

                provider.Organization = new OrganizationModel();
                if (provider.Activetab.ToLower() == "tab-1")
                {
                   
                    string _servicetype = "";
                    foreach (var item in provider.ServiceList)
                    {
                        if (item.IsChecked)
                            _servicetype = _servicetype + "," + item.Value;

                    }
                    _servicetype = _servicetype.TrimEnd(',');
                    _servicetype = _servicetype.TrimStart(',');


                    string __deliveryType = "";
                    foreach (var item in provider.DeliveryServiceList)
                    {
                        if (item.IsChecked)
                            __deliveryType = __deliveryType + "," + item.Value;

                    }
                    __deliveryType = __deliveryType.TrimStart(',');
                    __deliveryType = __deliveryType.TrimEnd(',');
                    provider.ServiceTypes = _servicetype;
                    provider.ServiceDeliveryTypes = __deliveryType;
                }
                else if (provider.Activetab.ToLower() == "tab-2")
                    provider.Organization = org;
                
                    provider.CreatedBy = Convert.ToInt32(Session["User_ID"]);
                    var response = await _provider.AddUpdateDeleteProvider(provider);
                    _provider.Save();
                    // TODO: Add insert logic here
                    TempData["response"] = "Edited Successfully";
                    TempData.Keep("response");
                    // TODO: Add update logic here

                    return RedirectToAction("index");
                
               
            }
            catch
            {
                
                return View(provider);
            }
        }

       
      
    }
}
