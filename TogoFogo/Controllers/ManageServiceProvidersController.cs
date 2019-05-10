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
using TogoFogo.Repository.ServiceProviders;
using TogoFogo.Filters;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using TogoFogo.Repository.ImportFiles;

namespace TogoFogo.Controllers
{

    public class ManageServiceProvidersController : Controller
    {
        private readonly IProvider _provider;
        private readonly IBank _bank;
        private readonly IContactPerson _contactPerson;
        private readonly string _path = "/UploadedImages/Providers/";
        private readonly DropdownBindController dropdown;
        private readonly IUploadFiles _RepoUploadFile;

        public ManageServiceProvidersController()
        {
            _provider = new Provider() ;
             dropdown= new DropdownBindController();
            _bank = new Bank();
            _contactPerson = new ContactPerson();
            _RepoUploadFile = new UploadFiles();
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Service_Provider)]
        public  async Task<ActionResult> Index()
        {


            var filter = new FilterModel { CompId = SessionModel.CompanyId };
            var Providers = await _provider.GetProviders(filter);          
            return View(Providers);
        }
        private string SaveImageFile(HttpPostedFileBase file,string folderName)
        {
            try
            {
                string path = Server.MapPath("~/UploadedImages/Providers/"+ folderName);
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
        [HttpPost]
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

            if (contact.IsUser)
                contact.Password = Encrypt_Decript_Code.encrypt_decrypt.Encrypt("CA5680", true);
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
                var mpc = new Email_send_code();
                Type type = mpc.GetType();
                var Status = (int)type.InvokeMember("sendmail_update",
                                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                                        BindingFlags.NonPublic, null, mpc,
                                        new object[] { contact.ConEmailAddress, contact.Password, contact.ConEmailAddress });
            }
         
            TempData["response"] = response;

            if (TempData["Provider"] != null)
            {
                var Provider = TempData["Provider"] as ServiceProviderModel;
                contact.ContactId = new Guid(response.result);
                var cityName = dropdown.BindLocation(contact.StateId).Where(x => x.Value == contact.CityId.ToString()).FirstOrDefault();
                contact.City = cityName.Text;
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

        // POST: ManageClient/Create    
        [HttpPost]
        public async Task<ActionResult> AddorEditServiceProvider(ServiceProviderModel provider)
        {
   
            provider.CompanyId = SessionModel.CompanyId;
            var statutory = await CommonModel.GetStatutoryType();
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            var cltns = TempData["provider"] as ServiceProviderModel;
            provider.Organization = new OrganizationModel();
            if (provider.ServiceList.Where(x => x.IsChecked == true).Count() < 1 || provider.DeliveryServiceList.Where(x => x.IsChecked == true).Count() < 1)
            {
                provider.ProcessList = new SelectList(await CommonModel.GetProcesses(), "Value", "Text");
                provider.SupportedCategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
                provider.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
                provider.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
                provider.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
                provider.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
                provider.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
                provider.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
                provider.Contact.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
                provider.Contact.CityList = new SelectList(await CommonModel.GetLookup("City"), "Value", "Text");

                if (provider.action == 'I')
                    return View("Create", provider);
                else
                    return View("Edit", provider);
            }
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
            provider.SupportedCategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            provider.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
            provider.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");          
            provider.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            provider.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
            provider.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            provider.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            provider.Contact.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
            provider.Contact.CityList = new SelectList(await CommonModel.GetLookup("City"), "Value", "Text");

            try
            {
                    provider.Activetab = "tab-1";
                    provider.CreatedBy = SessionModel.UserId;
        
                    var response = await _provider.AddUpdateDeleteProvider(provider);
                    _provider.Save();
                    provider.ProviderId = new Guid(response.result);
                TempData["response"] = response;
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
                provider.CreatedBy = SessionModel.UserId;
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


        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Service_Provider)]
        public async Task<ActionResult> Edit(Guid id)
        {
            TempData["provider"] = null;
            var Provider = await GetProvider(id);
            return View(Provider);

        }

        private async Task<ServiceProviderModel> GetProvider(Guid? ProviderId)
        {
            var Provider = await _provider.GetProviderById(ProviderId);

            Provider.Path = _path;
            Provider.CompanyId = SessionModel.CompanyId;
            var processes = await CommonModel.GetProcesses();
                Provider.ProcessList = new SelectList(processes, "Value", "Text");
            if (Provider.Organization == null)
                Provider.Organization = new OrganizationModel();
           
            Provider.SupportedCategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            Provider.Organization.GstCategoryList = new SelectList(dropdown.BindGst(null), "Value", "Text");
            var statutory = await CommonModel.GetStatutoryType();
            Provider.Organization.StatutoryList = new SelectList(statutory, "Value", "Text");
            var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
            Provider.Organization.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
            Provider.ServiceList = await TogoFogo.CommonModel.GetServiceType(SessionModel.CompanyId);
            Provider.DeliveryServiceList = await TogoFogo.CommonModel.GetDeliveryServiceType(SessionModel.CompanyId);        
                        
            Provider.Bank.BankList = new SelectList(await CommonModel.GetLookup("Bank"), "Value", "Text");
           
            Provider.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "value", "Text");
            Provider.Contact.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            Provider.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            Provider.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            if (ProviderId != null)
                Provider.action = 'U';
            else
                Provider.action = 'I';
            List<int> List = new List<int>();
            if (!string.IsNullOrEmpty(Provider._deviceCategories))
            {
                var _deviceCat = Provider._deviceCategories.Split(',');
                for (int i = 0; i < _deviceCat.Length; i++)
                {
                    List.Add(Convert.ToInt16(_deviceCat[i]));
                }
            }
            if (!string.IsNullOrEmpty(Provider.ServiceDeliveryTypes))
            {
                var _DeliveryService = Provider.ServiceDeliveryTypes.Split(',');
                for (int i = 0; i < _DeliveryService.Length; i++)
                {
                    var item = Provider.DeliveryServiceList.Where(x => x.Value == Convert.ToInt32(_DeliveryService[i])).FirstOrDefault();
                    if (item != null)
                        item.IsChecked = true;

                }
            }
            if (!string.IsNullOrEmpty(Provider.ServiceTypes))
            {
                var _serviceType = Provider.ServiceTypes.Split(',');
                for (int i = 0; i < _serviceType.Length; i++)
                {
                    var item = Provider.ServiceList.Where(x => x.Value == Convert.ToInt32(_serviceType[i])).FirstOrDefault();
                    if (item != null)
                        item.IsChecked = true;
                }
            }
            Provider.DeviceCategories = List;
            return Provider;
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
                return path + "\\" + savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        [HttpPost]
        public async Task<ActionResult> ImportProviders(ProviderFileModel provider)
        {
            provider.CompanyId = SessionModel.CompanyId;
            provider.UserId = SessionModel.UserId;
           
            if (provider.DataFile != null)
            {
                string excelPath = SaveFile(provider.DataFile, "ProviderData");
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
                    dtExcelData.Columns.AddRange(new DataColumn[29] {
                new DataColumn("Process Name", typeof(string)),
                new DataColumn("Service Provider Code", typeof(string)),
                new DataColumn("Service Provider Name", typeof(string)),
                new DataColumn("Service Delivery Type", typeof(string)),
                new DataColumn("Supported Device Category", typeof(string)),
                new DataColumn("Service Type", typeof(string)),
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
                     new DataColumn("Locality", typeof(string)),
                  new DataColumn("Near By Location", typeof(string)),
                   new DataColumn("Pin Code", typeof(string)),
                     new DataColumn("IsUser", typeof(string))
                });
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [Process Name], [Service Provider Name],[Service Delivery Type],[Supported Device Category]," +
                        "[Service Type],[Organization Name],[Organization Code],[Organization IEC Number],[Statutory Type]," +
                        "[Applicable Tax Type],[GST Category],[GST Number],[PAN Card Number],[IsServiceCenter] ," +
                        "[Contact Name],[Contact Email],[Contact PAN],[Contact Voter Id],[Contact Adhaar], " +
                        " [Address Type],[Country],[State],[City],[Address],[Locality],[Near By Location], " +
                        " [Pin Code], [IsUser] FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();

                }
                try
                {

                    var response = await _RepoUploadFile.UploadServiceProviders(provider, dtExcelData);
                    if (!response.IsSuccess)
                        System.IO.File.Delete(excelPath);
                    TempData["response"] = response;
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    if (System.IO.File.Exists(Server.MapPath(excelPath)))
                        System.IO.File.Delete(Server.MapPath(excelPath));
                    return RedirectToAction("index");

                }
            }
            return RedirectToAction("index");

        }      
    }
}
