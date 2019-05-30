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
using System.Reflection;
using TogoFogo.Repository.ServiceProviders;

namespace TogoFogo.Controllers
{

    public class ManageServiceCentersController : Controller
    {
        private readonly ICenter _Center;
        private readonly IProvider _Provider;
        private readonly IBank _bank;
        private readonly IContactPerson _contactPerson;
        private readonly string _path = "/UploadedImages/Centers/";
        private readonly DropdownBindController dropdown;

        public ManageServiceCentersController()
        {
            _Center = new Center() ;
             dropdown= new DropdownBindController();
            _bank = new Bank();
            _contactPerson = new ContactPerson();
            _Provider = new Provider();
        }


        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Service_Center_TRC)]
        public  async Task<ActionResult> Index()
        {
            var SessionModel = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = SessionModel.CompanyId };
            if (SessionModel.UserRole.ToLower().Contains("provider"))
                filter.ProviderId = SessionModel.RefKey;
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Create,Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        [HttpPost]
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Create, Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        [HttpPost]
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
            if (contact.IsUser)
                contact.Password = Encrypt_Decript_Code.encrypt_decrypt.Encrypt("CA5680", true);
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
                var mpc = new Email_send_code();
                Type type = mpc.GetType();
                var Status = (int)type.InvokeMember("sendmail_update",
                                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                                        BindingFlags.NonPublic, null, mpc,
                                        new object[] { contact.ConEmailAddress, contact.Password, contact.ConEmailAddress });
            }
            TempData["response"] = response;
            if (TempData["center"] != null)
            {
                var Center = TempData["center"] as ServiceCenterModel;
                contact.ContactId = new Guid(response.result);
                var cityName = dropdown.BindLocation(contact.StateId).Where(x => x.Value == contact.CityId.ToString()).FirstOrDefault();
                contact.City = cityName.Text;
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
            var processes = await CommonModel.GetProcesses();
            if (SessionModel.UserRole.ToLower().Contains("provider"))
            {
                var providerId = SessionModel.RefKey;
                var provider = await _Provider.GetProviderById(providerId);
                var seletedProcess = processes.Where(x => x.Value == provider.ProcessId);
                Center.ProcessList = new SelectList(seletedProcess, "Value", "Text");
                Center.IsProvider = true;
            }
            else
                Center.ProcessList = new SelectList(processes, "Value", "Text");
            if (Center.Organization == null)
                Center.Organization = new OrganizationModel();
            Center.SupportedCategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            Center.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            Center.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            Center.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            Center.ServiceList = await TogoFogo.CommonModel.GetServiceType(SessionModel.CompanyId);
            Center.DeliveryServiceList = await TogoFogo.CommonModel.GetDeliveryServiceType(SessionModel.CompanyId);
            Center.ProviderList = new SelectList(await CommonModel.GetServiceProviders(SessionModel.CompanyId), "Name", "Text");
            Center.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            Center.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            Center.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            Center.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            Center.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Create, Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        [HttpPost]
        public async Task<ActionResult> AddorEditServiceCenter(ServiceCenterModel Center)
        {
            var SessionModel = Session["User"] as SessionModel;
            var statutory = await CommonModel.GetStatutoryType();
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            var cltns = TempData["center"] as ServiceCenterModel;     
            Center.Organization = new OrganizationModel();
            if (Center.ServiceList.Where(x=>x.IsChecked==true).Count()<1 || Center.DeliveryServiceList.Where(x => x.IsChecked == true).Count() < 1)
            {
                Center.ProcessList = new SelectList(await CommonModel.GetProcesses(), "Value", "Text");
                Center.SupportedCategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
                Center.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
                Center.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
                Center.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
                Center.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
                Center.ProviderList = new SelectList(await CommonModel.GetServiceProviders(SessionModel.CompanyId), "Name", "Text");
                Center.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
                Center.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
                Center.Contact.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
                Center.Contact.CityList = new SelectList(await CommonModel.GetLookup("City"), "Value", "Text");
                if (Center.action == 'I')
                    return View("Create", Center);
                else
                    return View("Edit", Center);
            }
            if (TempData["center"] != null )
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
            Center.SupportedCategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            Center.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
            Center.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            Center.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            Center.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            Center.ProviderList = new SelectList(await CommonModel.GetServiceProviders(SessionModel.CompanyId), "Name", "Text");
            Center.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            Center.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            Center.Contact.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
            Center.Contact.CityList = new SelectList(await CommonModel.GetLookup("City"), "Value", "Text");
            try
            {
                    Center.Activetab = "tab-1";
                    Center.CreatedBy = SessionModel.UserId;
                Center.CompanyId = SessionModel.CompanyId;
                if (SessionModel.UserRole.ToLower().Contains("provider"))
                {
                    Guid? ProviderId = SessionModel.RefKey;
                    if (ProviderId != null)
                    {
                        Center.ProviderId = ProviderId;
                        Center.IsProvider = true; 
                    }
                     
                }
                    var response = await _Center.AddUpdateDeleteCenter(Center);
                    _Center.Save();
                    Center.CenterId = new Guid(response.result);
                TempData["response"] = response;
                TempData.Keep("response");
                if (Center.action == 'I')
                {
                    Center.Activetab = "tab-2";
                    TempData["center"] = Center;
                    TempData.Keep("Center");
                    return View("Create", Center);
                }
                else                    
                    return View("Create", Center);
                

            }
            catch(Exception ex)
            {
                if(Center.action=='I')
                return View("Create", Center);
                else
                    return View("Edit", Center);
            }
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Create, Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        [HttpPost]
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

        [PermissionBasedAuthorize(new Actions[] { Actions.Create, Actions.Edit }, (int)MenuCode.Manage_Service_Center_TRC)]
        [HttpPost]
        public async Task<ActionResult> AddOrEditClientReg(ServiceCenterModel Center)
        {

            var SessionModel = Session["User"] as SessionModel;
            if (Center.IsUser && !string.IsNullOrEmpty(Center.Password))
                Center.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(Center.Password, true);

            var cltns = TempData["center"] as ServiceCenterModel;
            Center.Organization = new OrganizationModel();
            if (TempData["center"] != null)
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
    

       
      
    }
}
