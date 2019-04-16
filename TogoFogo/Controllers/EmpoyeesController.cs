using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Filters;
using TogoFogo.Models;
using TogoFogo.Permission;
using TogoFogo.Repository;

namespace TogoFogo.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployee _employee;
        private readonly DropdownBindController drop;
        private string folderPath;
        public EmployeesController()
        {
            _employee = new Employee();
            drop = new DropdownBindController();
            folderPath = "~/UploadedImages/employees/";
        }
        private string SaveImageFile(HttpPostedFileBase file, string folderName)
        {
            try
            {
                string path = Server.MapPath(folderPath + folderName);
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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Engineers")]
        public async Task<ActionResult> Index()
        {
            Guid? CenterId = null;
            Guid? ProviderId = null;
            if (Session["RoleName"].ToString().ToLower().Contains("provider"))
                ProviderId = await CommonModel.GetProviderIdByUser(Convert.ToInt32(Session["User_ID"]));
            if (Session["RoleName"].ToString().ToLower().Contains("center"))
                CenterId = await CommonModel.GetProviderIdByUser(Convert.ToInt32(Session["User_ID"]));
            var employee = await _employee.GetAllEmployees(CenterId, ProviderId);
            return View(employee);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Engineers")]
        public async Task<ActionResult> Create()
        {
            var empModel = new EmployeeModel();
            empModel.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            empModel.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
            empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(), "Name", "Text");
            empModel.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            empModel.Contact.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            empModel.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            empModel.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            empModel.CenterList = new SelectList(Enumerable.Empty<SelectList>());
            empModel.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"), "Value", "Text");
            empModel.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");
            if (Session["RoleName"].ToString().ToLower() == "provider")
            {
               var ProviderId = await CommonModel.GetProviderIdByUser(Convert.ToInt32(Session["User_ID"]));
                empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(ProviderId));
            }
            else
                empModel.CenterList = new SelectList(Enumerable.Empty<SelectList>());           
            return View(empModel);

        }
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Create(EmployeeModel emp,ContactPersonModel contact)
        {
            if (emp.EMPPhoto1 != null)
                emp.EMPPhoto = SaveImageFile(emp.EMPPhoto1, "DP");
            if (contact.ConAdhaarNumberFilePath != null)
                contact.ConAdhaarFileName = SaveImageFile(contact.ConAdhaarNumberFilePath, "ADHRS");
            if (contact.ConVoterIdFilePath != null)
                contact.ConVoterIdFileName = SaveImageFile(contact.ConVoterIdFilePath, "VoterIds");
            if (contact.ConPanNumberFilePath != null)
                contact.ConPanFileName = SaveImageFile(contact.ConPanNumberFilePath, "PANCards");
            emp.Contact = contact;
            emp.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            emp.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
            emp.ProviderList = new SelectList(await CommonModel.GetServiceProviders(), "Name", "Text");
            emp.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            emp.Contact.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            emp.Contact.CityList = new SelectList(Enumerable.Empty<SelectList>());
            emp.Contact.StateList = new SelectList(Enumerable.Empty<SelectList>());
            emp.CenterList = new SelectList(Enumerable.Empty<SelectList>());
            emp.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"),"Value","Text");
            emp.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");
            emp.Action = 'I';
            emp.UserID = Convert.ToInt32(Session["User_ID"]);
            if (Session["RoleName"].ToString().ToLower().Contains("center"))
                emp.CenterId = await CommonModel.GetCenterIdByUser(Convert.ToInt32(Session["User_ID"]));
            var resonse = await _employee.AddUpdateDeleteEmployee(emp);
            TempData["response"] = resonse;
            TempData.Keep("response");
            return RedirectToAction("Index");

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Engineers")]
        public async Task<ActionResult> Edit(Guid empId)
        {
            var empModel = await _employee.GetEmployeeById(empId);
            empModel.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            empModel.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
            empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(), "Name", "Text");
            empModel.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            empModel.Contact.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            empModel.Contact.StateList = new SelectList(drop.BindState(empModel.Contact.CountryId), "Value", "Text");
            empModel.Contact.CityList = new SelectList(drop.BindLocation(empModel.Contact.StateId), "Value", "Text");
            empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(empModel.ProviderId), "Name", "Text");
            empModel.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"),"Value","Text");
            empModel.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");
            return View(empModel);
        }
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Edit(EmployeeModel empModel, ContactPersonModel contact)
        {
            empModel.UserID = Convert.ToInt32(Session["User_ID"]);
            if (empModel.EMPPhoto1 != null && empModel.EMPPhoto != null)
            {

                if (System.IO.File.Exists(Server.MapPath(folderPath + "DP/" + contact.ConAdhaarFileName)))
                    System.IO.File.Delete(Server.MapPath(folderPath + "DP/" + contact.ConAdhaarFileName));
            }
            if (contact.ConAdhaarNumberFilePath != null && contact.ConAdhaarFileName != null)
            {

                if (System.IO.File.Exists(Server.MapPath(folderPath+"ADHRS/" + contact.ConAdhaarFileName)))
                    System.IO.File.Delete(Server.MapPath(folderPath+"ADHRS/" + contact.ConAdhaarFileName));
            }
            if (contact.ConVoterIdFileName != null && contact.ConVoterIdFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(folderPath + "VoterIds/" + contact.ConVoterIdFileName)))
                    System.IO.File.Delete(Server.MapPath(folderPath + "VoterIds/" + contact.ConVoterIdFileName));
            }
            if (contact.ConPanFileName != null && contact.ConPanNumberFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(folderPath+"PANCards/" + contact.ConPanFileName)))
                    System.IO.File.Delete(Server.MapPath(folderPath + "PANCards/" + contact.ConPanFileName));
            }

            if (empModel.EMPPhoto1 != null)
                empModel.EMPPhoto = SaveImageFile(empModel.EMPPhoto1, "DP");
            if (contact.ConAdhaarNumberFilePath != null)
                contact.ConAdhaarFileName = SaveImageFile(contact.ConAdhaarNumberFilePath, "ADHRS");
            if (contact.ConVoterIdFilePath != null)
                contact.ConVoterIdFileName = SaveImageFile(contact.ConVoterIdFilePath, "VoterIds");
            if (contact.ConPanNumberFilePath != null)
                contact.ConPanFileName = SaveImageFile(contact.ConPanNumberFilePath, "PANCards");
            empModel.Contact = contact;
            empModel.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            empModel.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
            empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(), "Name", "Text");
            empModel.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            empModel.Contact.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            empModel.Contact.StateList = new SelectList(drop.BindState(empModel.Contact.CountryId), "Value", "Text");
            empModel.Contact.CityList = new SelectList(drop.BindLocation(empModel.Contact.StateId), "Value", "Text");
            empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(empModel.ProviderId), "Name", "Text");
            empModel.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"), "Value", "Text");
            empModel.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");
            empModel.Action = 'U';        
            var resonse = await _employee.AddUpdateDeleteEmployee(empModel);
            TempData["resonse"] = resonse;
            TempData.Keep("resonse");
            return RedirectToAction("Index");

        }
    }
}