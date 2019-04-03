using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Filters;
using TogoFogo.Models;
using TogoFogo.Permission;
using TogoFogo.Repository;
using TogoFogo.Repository.ServiceCenters;
using TogoFogo.Repository.ServiceProviders;

namespace TogoFogo.Controllers
{

    public class ManageServiceCentersController : Controller
    {
        private readonly ICenter _Center;
        private readonly IProvider _Provider;
        private readonly IBank _bank;
        private readonly IContactPerson _contactPerson;

        private readonly DropdownBindController dropdown;
        public ManageServiceCentersController()
        {
            _Center = new Center() ;
             dropdown= new DropdownBindController();
            _bank = new Bank();
            _contactPerson = new ContactPerson();
            _Provider = new Provider();
        }


        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Service Center/TRC")]
        public  async Task<ActionResult> Index()
        {
   
            Guid? ProviderId=null;
            if (Session["RoleName"].ToString().ToLower().Contains("provider"))       
                ProviderId = await CommonModel.GetProviderIdByUser(Convert.ToInt32(Session["User_ID"]));
            var Centers = await _Center.GetCenters(ProviderId);           
            return View(Centers);
        }
        private string SaveImageFile(HttpPostedFileBase file,string folderName)
        {
            try
            {
                string path = Server.MapPath("~/UploadedImages/Centers/"+ folderName);
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

            if (TempData["Center"] != null)
                TempData.Keep("Center");
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
           
            contactPerson.AddressTypelist = new SelectList(Addresses, "Value", "Text");
            contactPerson.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");

            if (contactPerson.ContactId != null)
            {
                contactPerson.Action = 'U';
                contactPerson.StateList = new SelectList(dropdown.BindState(contactPerson.CountryId), "Value", "Text");
                contactPerson.CityList = new SelectList(dropdown.BindLocation(contactPerson.StateId), "Value", "Text");

            }
            else
            {

                contactPerson.Action = 'I';
            
                contactPerson.CityList = new SelectList(Enumerable.Empty<SelectListItem>());
                contactPerson.StateList = new SelectList(Enumerable.Empty<SelectListItem>());

            }


            if (TempData["Center"] != null)
                TempData.Keep("Center");
                return PartialView("~/views/common/_AddEditContactPerson.cshtml", contactPerson);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrEditBank(BankDetailModel bank)
        {

            if (bank.BankCancelledChequeFilePath != null && bank.BankCancelledChequeFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/UploadedImages/Centers/Banks/" + bank.BankCancelledChequeFileName)))
                    System.IO.File.Delete(Server.MapPath("~/UploadedImages/Centers/Banks/" + bank.BankCancelledChequeFileName));
            }

            if (bank.BankCancelledChequeFilePath != null)
                bank.BankCancelledChequeFileName = SaveImageFile(bank.BankCancelledChequeFilePath, "Banks");

            bank.UserId = Convert.ToInt32(Session["User_ID"]);
            if (TempData["Center"] != null)
            {             
                var Center = TempData["Center"] as ServiceCenterModel;
                var Response = await _bank.AddUpdateBankDetails(bank);
                var CenterModel = await _Center.GetCenterById(bank.RefKey);
                CenterModel.ProcessList = Center.ProcessList;
                CenterModel.SupportedCategoryList = Center.SupportedCategoryList;
                CenterModel.DeviceCategories = Center.DeviceCategories;
                CenterModel.Organization.GstCategoryList = Center.Organization.GstCategoryList;
                CenterModel.Organization.StatutoryList = Center.Organization.StatutoryList;
                CenterModel.Organization.AplicationTaxTypeList = Center.Organization.AplicationTaxTypeList;
                CenterModel.ServiceList = Center.ServiceList;
                CenterModel.DeliveryServiceList = Center.DeliveryServiceList;
                CenterModel.action = Center.action;
                CenterModel.Activetab = "tab-5";                    
                    TempData["Center"] = CenterModel;
                    TempData.Keep("Center");
                    TempData["response"] = Response;
                    TempData.Keep("response");
                    return View("Create", CenterModel);
               
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


            if(contact.ConAdhaarNumberFilePath != null && contact.ConAdhaarFileName != null)
            {

                if (System.IO.File.Exists(Server.MapPath("~/UploadedImages/Centers/ADHRS/" + contact.ConAdhaarFileName)))
                    System.IO.File.Delete(Server.MapPath("~/UploadedImages/Centers/ADHRS/" + contact.ConAdhaarFileName));
            }
            if (contact.ConVoterIdFileName != null && contact.ConVoterIdFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/UploadedImages/Centers/VoterIds/" + contact.ConVoterIdFileName)))
                    System.IO.File.Delete(Server.MapPath("~/UploadedImages/Centers/VoterIds/" + contact.ConVoterIdFileName));
            }
            if (contact.ConPanFileName != null && contact.ConPanNumberFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/UploadedImages/Centers/PANCards/" + contact.ConPanFileName)))
                    System.IO.File.Delete(Server.MapPath("~/UploadedImages/Centers/PANCards/" + contact.ConPanFileName));
            }

            if (contact.ConAdhaarNumberFilePath != null)
                contact.ConAdhaarFileName = SaveImageFile(contact.ConAdhaarNumberFilePath, "ADHRS");
            if (contact.ConVoterIdFilePath != null)
                contact.ConVoterIdFileName = SaveImageFile(contact.ConVoterIdFilePath, "VoterIds");
            if (contact.ConPanNumberFilePath != null)
                contact.ConPanFileName = SaveImageFile(contact.ConPanNumberFilePath, "PANCards");
            if (TempData["Center"] != null)
            {
                var Center = TempData["Center"] as ServiceCenterModel;
                if (contact.ContactId != null)
                    contact.Action = 'U';
                else
                {
                    contact.Action = 'I';
                    contact.RefKey = Center.CenterId ?? Guid.Empty;
                }            
                var response = await _contactPerson.AddUpdateContactDetails(contact);
                var CenterModel = await _Center.GetCenterById(contact.RefKey);
                CenterModel.ProcessList = Center.ProcessList;
                CenterModel.DeviceCategories = Center.DeviceCategories;
                CenterModel.SupportedCategoryList = Center.SupportedCategoryList;
                CenterModel.Organization.GstCategoryList = Center.Organization.GstCategoryList;              
                CenterModel.Organization.StatutoryList = Center.Organization.StatutoryList;          
                CenterModel.Organization.AplicationTaxTypeList = Center.Organization.AplicationTaxTypeList;
                CenterModel.ServiceList = Center.ServiceList;
                CenterModel.DeliveryServiceList = Center.DeliveryServiceList;
                try
                {
                   
                    

                    CenterModel.action = 'I';
                    CenterModel.Activetab = "tab-4";
                    TempData["Center"] = CenterModel;
                    TempData.Keep("Center");
                    TempData["response"] = response;
                    TempData.Keep("response");
                    return View("Create", CenterModel);
                }
                catch
                {

                    TempData["Center"] = CenterModel;
                    TempData.Keep("Center");
                    return View("Create", CenterModel);
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


        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Service Center/TRC")]
        public async Task<ActionResult> Create()
        {
            var CenterModel = new ServiceCenterModel()
            {
                Organization = new OrganizationModel(),

            };
            var processes = await CommonModel.GetProcesses();
            if (Session["RoleName"].ToString().ToLower().Contains("provider"))
            {
                var providerId = await CommonModel.GetProviderIdByUser(Convert.ToInt32(Session["User_ID"]));
                var provider = await _Provider.GetProviderById(providerId);
                var seletedProcess = processes.Where(x => x.Value == provider.ProcessId);
                CenterModel.ProcessList = new SelectList(seletedProcess, "Value", "Text");
            }
           else
                CenterModel.ProcessList = new SelectList(processes, "Value", "Text");
                CenterModel.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
                CenterModel.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
                var statutory = await CommonModel.GetStatutoryType();
                CenterModel.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
                var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
                CenterModel.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
                CenterModel.ServiceList = await CommonModel.GetServiceType();
                CenterModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(), "Name", "Text");
                CenterModel.DeliveryServiceList = await CommonModel.GetDeliveryServiceType();
                CenterModel.action = 'I';
            
            return View(CenterModel);
        }

  
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> AddorEditServiceCenter(ServiceCenterModel Center)
        {
            var statutory = await CommonModel.GetStatutoryType();
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            var cltns = TempData["Center"] as ServiceCenterModel;     
            Center.Organization = new OrganizationModel();
            if (Center.ServiceList.Where(x=>x.IsChecked==true).Count()<1 || Center.DeliveryServiceList.Where(x => x.IsChecked == true).Count() < 1)
            {
                Center.ProcessList = new SelectList(await CommonModel.GetProcesses(), "Value", "Text");
                Center.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
                Center.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
                Center.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
                Center.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
                Center.ProviderList = new SelectList(await CommonModel.GetServiceProviders(), "Name", "Text");
                if (Center.action == 'I')
                    return View("Create", Center);
                else
                    return View("Edit", Center);
            }
            if (TempData["Center"] != null )
            {
                    Center = cltns;
             
                    string _servicetype = "";
                    foreach (var item in Center.ServiceList)
                    {
                        if (item.IsChecked)
                            _servicetype = _servicetype + "," + item.Value;

                    }
                    _servicetype = _servicetype.TrimEnd(',');
                    _servicetype = _servicetype.TrimStart(',');

                    string __deliveryType = "";
                    foreach (var item in Center.DeliveryServiceList)
                    {
                        if (item.IsChecked)
                            __deliveryType = __deliveryType + "," + item.Value;

                    }
                    __deliveryType = __deliveryType.TrimStart(',');
                    __deliveryType = __deliveryType.TrimEnd(',');
                    Center.ServiceTypes = _servicetype;
                    Center.ServiceDeliveryTypes = __deliveryType;
               
            }
            else
            {
              
                    string _servicetype = "";
                    foreach (var item in Center.ServiceList)
                    {
                        if (item.IsChecked)
                            _servicetype = _servicetype + "," + item.Value;

                    }
                    _servicetype = _servicetype.TrimEnd(',');
                    _servicetype = _servicetype.TrimStart(',');

                    string __deliveryType = "";
                    foreach (var item in Center.DeliveryServiceList)
                    {
                        if (item.IsChecked)
                            __deliveryType = __deliveryType + "," + item.Value;

                    }
                    __deliveryType = __deliveryType.TrimStart(',');
                    __deliveryType = __deliveryType.TrimEnd(',');
                    Center.ServiceTypes = _servicetype;
                    Center.ServiceDeliveryTypes = __deliveryType;
                
            }

            Center.ProcessList = new SelectList(await  CommonModel.GetProcesses(), "Value", "Text");
            Center.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            Center.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            Center.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            Center.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            Center.ProviderList = new SelectList(await CommonModel.GetServiceProviders(), "Name", "Text");
    
            try
            {
                    Center.Activetab = "tab-1";
                    Center.CreatedBy = Convert.ToInt32(Session["User_ID"]);
                if (Session["RoleName"].ToString().ToLower().Contains("provider"))
                {
                    Guid? ProviderId = await CommonModel.GetProviderIdByUser(Center.CreatedBy);
                    if (ProviderId!=null)
                     Center.ProviderId =  ProviderId;
                }
                    var response = await _Center.AddUpdateDeleteCenter(Center);
                    _Center.Save();
                    Center.CenterId = new Guid(response.result);
                TempData["response"] = response;
                TempData.Keep("response");
                if (Center.action == 'I')
                {
                    Center.Activetab = "tab-2";
                    TempData["Center"] = Center;
                    TempData.Keep("Center");
                    return View("Create",Center);
                }
               else
                    return RedirectToAction("Index");
                                                                                         
            }
            catch(Exception ex)
            {
                if(Center.action=='I')
                return View("Create", Center);
                else
                    return View("Edit", Center);
            }
        }


        [HttpPost]
        public async Task<ActionResult> AddorEditOrganization(ServiceCenterModel Center,OrganizationModel org)
        {
            var cltns = TempData["Center"] as ServiceCenterModel;
            if (TempData["Center"] != null)
            {
                Center = cltns;
                Center.Organization = org;                
            }
            else          
                Center.Organization = org;

            if (Center.Organization.OrgGSTNumberFilePath != null && Center.Organization.OrgGSTFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/UploadedImages/Centers/Gsts/" + Center.Organization.OrgGSTFileName)))
                    System.IO.File.Delete(Server.MapPath("~/UploadedImages/Centers/Gsts/" + Center.Organization.OrgGSTFileName));
            }
            if (Center.Organization.OrgPanNumberFilePath != null && Center.Organization.OrgPanFileName != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/UploadedImages/Clients/PANCards/" + Center.Organization.OrgPanFileName)))
                    System.IO.File.Delete(Server.MapPath("~/UploadedImages/Clients/PANCards/" + Center.Organization.OrgPanFileName));
            }

            if (Center.Organization.OrgGSTNumberFilePath != null)
                Center.Organization.OrgGSTFileName = SaveImageFile(Center.Organization.OrgGSTNumberFilePath, "Gsts");
            if (Center.Organization.OrgPanNumberFilePath != null)
                Center.Organization.OrgPanFileName = SaveImageFile(Center.Organization.OrgPanNumberFilePath, "PANCards");
            var statutory = await CommonModel.GetStatutoryType();
            Center.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            Center.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            Center.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            try
            {

                Center.Activetab = "tab-2";
                Center.CreatedBy = Convert.ToInt32(Session["User_ID"]);
                var response = await _Center.AddUpdateDeleteCenter(Center);
                _Center.Save();
                Center.CenterId = new Guid(response.result);
                TempData["response"] = response;
                TempData.Keep("response");
                if (Center.action == 'I')
                {
                    Center.Activetab = "tab-3";
                    TempData["Center"] = Center;
                    TempData.Keep("Center");
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


        [HttpPost]
        public async Task<ActionResult> AddOrEditClientReg(ServiceCenterModel Center)
        {

            if (Center.IsUser && !string.IsNullOrEmpty(Center.Password))
                Center.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(Center.Password, true);

            var cltns = TempData["Center"] as ServiceCenterModel;
            Center.Organization = new OrganizationModel();
            if (TempData["Center"] != null)
            {
                cltns.Remarks = Center.Remarks;
                cltns.IsActive = Center.IsActive;
                cltns.IsUser = Center.IsUser;
                cltns.UserName = Center.UserName;
                cltns.Password = Center.Password;
                Center = cltns;               
            }                    
            try
            {

                Center.Activetab = "tab-5";
                Center.CreatedBy = Convert.ToInt32(Session["User_ID"]);
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

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Service Center/TRC")]
        public async Task<ActionResult> Edit(Guid id)
        {
            TempData["Center"] = null;
            var Center = await _Center.GetCenterById(id);
            if (Center.Organization == null)
                Center.Organization = new OrganizationModel();
            var _deviceCat = Center._deviceCategories.Split(',');
            var processes = await CommonModel.GetProcesses();
            Center.ProcessList = new SelectList(processes, "Value", "Text");
            Center.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            Center.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            Center.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            Center.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            Center.ServiceList = await TogoFogo.CommonModel.GetServiceType();
            Center.DeliveryServiceList = await TogoFogo.CommonModel.GetDeliveryServiceType();
            Center.ProviderList = new SelectList(await CommonModel.GetServiceProviders(), "Name", "Text");
            Center.action = 'U';
            List<int> List        =new  List<int>(); 
            for (int i = 0; i < _deviceCat.Length; i++)
            {
                List.Add(Convert.ToInt16(_deviceCat[i]));

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
            Center.action = 'U';
            return View(Center);

        }


        [HttpPost]
        public async Task<ActionResult> Edit(ServiceCenterModel Center, OrganizationModel org)
        {
            
            try
            {

                Center.Organization = new OrganizationModel();
                if (Center.Activetab.ToLower() == "tab-1")
                {
                   
                    string _servicetype = "";
                    foreach (var item in Center.ServiceList)
                    {
                        if (item.IsChecked)
                            _servicetype = _servicetype + "," + item.Value;

                    }
                    _servicetype = _servicetype.TrimEnd(',');
                    _servicetype = _servicetype.TrimStart(',');


                    string __deliveryType = "";
                    foreach (var item in Center.DeliveryServiceList)
                    {
                        if (item.IsChecked)
                            __deliveryType = __deliveryType + "," + item.Value;

                    }
                    __deliveryType = __deliveryType.TrimStart(',');
                    __deliveryType = __deliveryType.TrimEnd(',');
                    Center.ServiceTypes = _servicetype;
                    Center.ServiceDeliveryTypes = __deliveryType;
                }
                else if (Center.Activetab.ToLower() == "tab-2")
                    Center.Organization = org;
                
                    Center.CreatedBy = Convert.ToInt32(Session["User_ID"]);
                    var response = await _Center.AddUpdateDeleteCenter(Center);
                    _Center.Save();
                    // TODO: Add insert logic here
                    TempData["response"] = "Edited Successfully";
                    TempData.Keep("response");
                    // TODO: Add update logic here

                    return RedirectToAction("index");
                
               
            }
            catch
            {
                
                return View(Center);
            }
        }

       
      
    }
}
