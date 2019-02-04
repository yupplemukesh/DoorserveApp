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

        // GET: ManageClient/Create
        public async Task<ActionResult> Create()
        {
            var clientModel = new ClientModel();

            //clientModel.StateList = new SelectList(Enumerable.Empty<SelectListItem>());

            //clientModel.CityList = new SelectList(Enumerable.Empty<SelectListItem>());
            //clientModel.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ////ViewBag.ProcessName = new SelectList(dropdown.BindProduct(), "Value", "Text");
            //clientModel.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            //clientModel.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            clientModel.ServiceList = TogoFogo.CommonModel.ServiceType();
            clientModel.DeliveryServiceList = TogoFogo.CommonModel.DeliveryServiceType();
            return View(clientModel);
        }

        // POST: ManageClient/Create
        [HttpPost]
        public async Task<ActionResult> Create(ClientModel client)
        {

  
  
            //client.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            client.SupportedCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            client.Organization.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            try
            {

                if (ModelState.IsValid)
                {

                    if (client.Organization.OrgGSTNumberFilePath != null)
                        client.Organization.OrgGSTFileName = SaveImageFile(client.Organization.OrgGSTNumberFilePath, "Clients/GSTS");
                    if (client.Organization.OrgPanNumberFilePath != null)
                        client.Organization.OrgPanFileName = SaveImageFile(client.Organization.OrgPanNumberFilePath, "Clients/PANS");
                    //if (client.ContactPersons.ConAdhaarNumberFilePath != null)
                    //    client.ConAdhaarFileName = SaveImageFile(client.ConAdhaarNumberFilePath, "Clients/ADHRS");
                    //if (client.ConPanNumberFilePath != null)
                    //    client.ConPanFileName = SaveImageFile(client.ConPanNumberFilePath, "Clients/PANS");
                    //if (client.ConVoterIdFilePath != null)
                    //    client.ConVoterIdFileName = SaveImageFile(client.ConVoterIdFilePath, "Clients/VOTERIDS");
                    //if (client.BankCancelledChequeFilePath != null)
                    //    client.BankCancelledChequeFileName = SaveImageFile(client.BankCancelledChequeFilePath, "Clients/Cheques");


                    //if (client.IsUser && client.Password != string.Empty)
                    //    client.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(client.Password, true);
                    client.CreatedBy = User.Identity.Name;
                    string _servicetype = "";
                    foreach(var item in client.ServiceList)
                    {
                        if(item.IsChecked)
                        _servicetype = _servicetype + "," + item.Text;

                    }
                    _servicetype = _servicetype.TrimEnd(',');
                    _servicetype = _servicetype.TrimStart(',');

                    string __deliveryType = "";
                    foreach (var item in client.DeliveryServiceList)
                    {
                        if (item.IsChecked)
                            __deliveryType = __deliveryType + "," + item.Text;

                    }
                    __deliveryType = __deliveryType.TrimStart(',');
                    __deliveryType = __deliveryType.TrimEnd(',');
                    client.ServiceTypes = _servicetype;
                    client.ServiceDeliveryTypes = __deliveryType;
                    var response = await _client.AddUpdateDeleteClient(client, 'I');
                    _client.Save();
                    // TODO: Add insert logic here
                    TempData["response"] = response;
                    TempData.Keep("response");
                    return RedirectToAction("Index");
                }
                else
                {
                   
                    //client.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
                    //client.CityList = new SelectList(dropdown.BindLocationByState(client.ConState.ToString()), "Value", "Text");
                    return View(client);
                }

            }
            catch(Exception ex)
            {

                //client.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
                //client.CityList = new SelectList(dropdown.BindLocationByState(client.ConState.ToString()), "Value", "Text");

                return View(client);
            }
        }

        // GET: ManageClient/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            var client = await _client.GetClientByClientId(id);
            var _deviceCat = client._deviceCategories.Split(',');
            client.ServiceList = TogoFogo.CommonModel.ServiceType();
            client.DeliveryServiceList = TogoFogo.CommonModel.DeliveryServiceType();
            List<int> List        =new  List<int>(); 
            for (int i = 0; i < _deviceCat.Length; i++)
            {
                List.Add(Convert.ToInt16(_deviceCat[i]));

            }

            var _DeliveryService = client.ServiceDeliveryTypes.Split(',');
            for (int i = 0; i < _DeliveryService.Length; i++)
            {
                if(_DeliveryService[i]== "Pic & Drop")
                {
                    var item = client.DeliveryServiceList.Where(x => x.Text == "Pic & Drop").FirstOrDefault();
                    item.IsChecked = true;

                }
                if (_DeliveryService[i] == "Carry-in")
                {
                    var item = client.DeliveryServiceList.Where(x => x.Text == "Carry-in").FirstOrDefault();
                    item.IsChecked = true;

                }
                if (_DeliveryService[i] == "Onsite")
                {
                    var item = client.DeliveryServiceList.Where(x => x.Text == "Onsite").FirstOrDefault();
                    item.IsChecked = true;

                }
            }

            var _serviceType = client.ServiceTypes.Split(',');
            for (int i = 0; i < _serviceType.Length; i++)
            {
                if (_serviceType[i] == "Installation")
                {
                    var item = client.ServiceList.Where(x => x.Text == "Installation").FirstOrDefault();
                    item.IsChecked = true;

                }
                if (_serviceType[i] == "Repair")
                {
                    var item = client.ServiceList.Where(x => x.Text == "Repair").FirstOrDefault();
                    item.IsChecked = true;

                }
            }
            client.DeviceCategories = List;
            //client.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text"); 
          
            //client.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
            ////client.CityList = new SelectList(dropdown.BindLocationByState(client.con.ConState.ToString()), "Value", "Text");
            //client.SupportedCategoryList = new SelectList(dropdown.BindCategorySelectpicker(), "Value", "Text");

            //client.GstCategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            return View(client);

        }

        // POST: ManageClient/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ClientModel client)
        {

            //client.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");         
            client.SupportedCategoryList = new SelectList(dropdown.BindCategorySelectpicker(), "Value", "Text");
            client.ServiceList = TogoFogo.CommonModel.ServiceType();
            client.DeliveryServiceList = TogoFogo.CommonModel.DeliveryServiceType();
            try
            {
                if (ModelState.IsValid)
                {

                    //if (client.OrgGSTNumberFilePath != null)
                    //    client.OrgGSTFileName = SaveImageFile(client.OrgGSTNumberFilePath, "Clients/GSTS");
                    //if (client.OrgPanNumberFilePath != null)
                    //    client.OrgPanFileName = SaveImageFile(client.OrgPanNumberFilePath, "Clients/PANS");
                    //if (client.ConAdhaarNumberFilePath != null)
                    //    client.ConAdhaarFileName = SaveImageFile(client.ConAdhaarNumberFilePath, "Clients/ADHRS");
                    //if (client.ConPanNumberFilePath != null)
                    //    client.ConPanFileName = SaveImageFile(client.ConPanNumberFilePath, "Clients/PANS");
                    //if (client.ConVoterIdFilePath != null)
                    //    client.ConVoterIdFileName = SaveImageFile(client.ConVoterIdFilePath, "Clients/VOTERIDS");
                    //if (client.BankCancelledChequeFilePath != null)
                    //    client.BankCancelledChequeFileName = SaveImageFile(client.BankCancelledChequeFilePath, "Clients/Cheques");
                    string _servicetype = "";
                    foreach (var item in client.ServiceList)
                    {
                        if (item.IsChecked)
                            _servicetype = _servicetype + "," + item.Text;

                    }
                    _servicetype = _servicetype.TrimEnd(',');
                    _servicetype = _servicetype.TrimStart(',');

                    string __deliveryType = "";
                    foreach (var item in client.DeliveryServiceList)
                    {
                        if (item.IsChecked)
                            __deliveryType = __deliveryType + "," + item.Text;

                    }
                    __deliveryType = __deliveryType.TrimStart(',');
                    __deliveryType = __deliveryType.TrimEnd(',');
                    client.ServiceTypes = _servicetype;
                    client.ServiceDeliveryTypes = __deliveryType;

                    //if (client.IsUser && client.Password != string.Empty)
                    //    client.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(client.Password, true);
                    ////client.CreatedBy=
                    client.CreatedBy = User.Identity.Name;
                    var response = await _client.AddUpdateDeleteClient(client, 'U');
                    _client.Save();
                    // TODO: Add insert logic here
                    TempData["response"] = response;
                    TempData.Keep("response");
                    // TODO: Add update logic here

                    return RedirectToAction("Index");
                }
                else
                {
                    //client.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
                    //client.CityList = new SelectList(dropdown.BindLocationByState(client.ConState.ToString()), "Value", "Text");

                    return View(client);
                }
            }
            catch
            {
                //client.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
                //client.CityList = new SelectList(dropdown.BindLocationByState(client.ConState.ToString()), "Value", "Text");
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
                var response = await _client.AddUpdateDeleteClient(client, 'D');
                _client.Save();
            }
            catch
            {
            }

            return RedirectToAction("Index");
        }


      
        
        
     

    }
}
