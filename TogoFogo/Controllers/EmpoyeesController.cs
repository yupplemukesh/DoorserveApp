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
        private SessionModel user;
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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Engineers)]
        public async Task<ActionResult> Index()
        {
            user = Session["User"] as SessionModel;
            var filter = new FilterModel();
            if (user.UserRole.ToLower().Contains("provider"))
                filter.ProviderId = user.RefKey;
            if (user.UserRole.ToLower().Contains("center"))
                filter.RefKey = user.RefKey;
            filter.CompId = user.CompanyId;
            var employee = await _employee.GetAllEmployees(filter);
            return View(employee);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Engineers)]
        public async Task<ActionResult> Create()
        {
            user = Session["User"] as SessionModel;
            var empModel = new EmployeeModel();
            empModel.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            empModel.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
         
            empModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            empModel.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            empModel.CityList = new SelectList(Enumerable.Empty<SelectList>());
            empModel.StateList = new SelectList(Enumerable.Empty<SelectList>());
            empModel.CenterList = new SelectList(Enumerable.Empty<SelectList>());
            empModel.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"), "Value", "Text");
            empModel.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");
            if (user.UserRole.ToLower().Contains("provider"))
            {
                var ProviderId = user.RefKey;
                empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(ProviderId));
                empModel.IsProvider = true;
            }
            else if (user.UserRole.ToLower().Contains("center"))
            {
                empModel.RefKey = user.RefKey;
                empModel.CenterList = new SelectList(Enumerable.Empty<SelectList>());
                empModel.IsCenter = true;
            }
          else
                empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(user.CompanyId), "Name", "Text");

            return View(empModel);

        }
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Create(EmployeeModel emp,ContactPersonModel contact)
        {
            user = Session["User"] as SessionModel;
            if (emp.EMPPhoto1 != null)
                emp.EMPPhoto = SaveImageFile(emp.EMPPhoto1, "DP");
            if (contact.ConAdhaarNumberFilePath != null)
                contact.ConAdhaarFileName = SaveImageFile(contact.ConAdhaarNumberFilePath, "ADHRS");
            if (contact.ConVoterIdFilePath != null)
                contact.ConVoterIdFileName = SaveImageFile(contact.ConVoterIdFilePath, "VoterIds");
            if (contact.ConPanNumberFilePath != null)
                contact.ConPanFileName = SaveImageFile(contact.ConPanNumberFilePath, "PANCards");
            emp.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            emp.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
            emp.ProviderList = new SelectList(await CommonModel.GetServiceProviders(user.CompanyId), "Name", "Text");
            emp.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            emp.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            emp.CityList = new SelectList(Enumerable.Empty<SelectList>());
            emp.StateList = new SelectList(Enumerable.Empty<SelectList>());
            emp.CenterList = new SelectList(Enumerable.Empty<SelectList>());
            emp.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"),"Value","Text");
            emp.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");
            emp.Action = 'I';
            emp.UserId = user.UserId;
            emp.CompanyId = user.CompanyId;
            if (user.UserRole.ToLower().Contains("provider"))
            {
                var ProviderId = user.RefKey;
                emp.CenterList = new SelectList(await CommonModel.GetServiceCenters(ProviderId));
                emp.IsProvider = true;
            }
            else if (user.UserRole.ToLower().Contains("center"))
            {
                emp.RefKey = user.RefKey;
                emp.CenterList = new SelectList(Enumerable.Empty<SelectList>());
                emp.IsCenter = true;
            }
            else
                emp.ProviderList = new SelectList(await CommonModel.GetServiceProviders(user.CompanyId), "Name", "Text");

            var response = await _employee.AddUpdateDeleteEmployee(emp);
            TempData["response"] = response;
            return RedirectToAction("Index");

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Engineers)]
        public async Task<ActionResult> Edit(Guid empId)
        {
            user = Session["User"] as SessionModel;
            var empModel = await _employee.GetEmployeeById(empId);          
            empModel.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            empModel.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
            empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(user.CompanyId), "Name", "Text");
            empModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            empModel.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            empModel.StateList = new SelectList(drop.BindState(empModel.CountryId), "Value", "Text");
            empModel.CityList = new SelectList(drop.BindLocation(empModel.StateId), "Value", "Text");
            empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(empModel.ProviderId), "Name", "Text");
            empModel.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"),"Value","Text");
            empModel.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");
            return View(empModel);
        }
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Edit(EmployeeModel empModel)
         {
            user = Session["User"] as SessionModel;
            empModel.UserId = user.UserId;
            empModel.CompanyId = user.CompanyId;
            if (empModel.EMPPhoto1 != null && empModel.EMPPhoto != null)
            {

                if (System.IO.File.Exists(Server.MapPath(folderPath + "DP/" + empModel.ConAdhaarFileName)))
                    System.IO.File.Delete(Server.MapPath(folderPath + "DP/" + empModel.ConAdhaarFileName));
            }
            if (empModel.ConAdhaarNumberFilePath != null && empModel.ConAdhaarFileName != null)
            {

                if (System.IO.File.Exists(Server.MapPath(folderPath+"ADHRS/" + empModel.ConAdhaarFileName)))
                    System.IO.File.Delete(Server.MapPath(folderPath+"ADHRS/" + empModel.ConAdhaarFileName));
            }
            if (empModel.ConVoterIdFileName != null && empModel.ConVoterIdFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(folderPath + "VoterIds/" + empModel.ConVoterIdFileName)))
                    System.IO.File.Delete(Server.MapPath(folderPath + "VoterIds/" + empModel.ConVoterIdFileName));
            }
            if (empModel.ConPanFileName != null && empModel.ConPanNumberFilePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(folderPath+"PANCards/" + empModel.ConPanFileName)))
                    System.IO.File.Delete(Server.MapPath(folderPath + "PANCards/" + empModel.ConPanFileName));
            }

            if (empModel.EMPPhoto1 != null)
                empModel.EMPPhoto = SaveImageFile(empModel.EMPPhoto1, "DP");
            if (empModel.ConAdhaarNumberFilePath != null)
                empModel.ConAdhaarFileName = SaveImageFile(empModel.ConAdhaarNumberFilePath, "ADHRS");
            if (empModel.ConVoterIdFilePath != null)
                empModel.ConVoterIdFileName = SaveImageFile(empModel.ConVoterIdFilePath, "VoterIds");
            if (empModel.ConPanNumberFilePath != null)
                empModel.ConPanFileName = SaveImageFile(empModel.ConPanNumberFilePath, "PANCards");
            if(empModel.IsUser)
                empModel.Password = Encrypt_Decript_Code.encrypt_decrypt.Encrypt("CA5680", true);


            empModel.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            empModel.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
            empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(user.CompanyId), "Name", "Text");
            empModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            empModel.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            empModel.StateList = new SelectList(drop.BindState(empModel.CountryId), "Value", "Text");
            empModel.CityList = new SelectList(drop.BindLocation(empModel.StateId), "Value", "Text");
            empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(empModel.ProviderId), "Name", "Text");
            empModel.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"), "Value", "Text");
            empModel.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");
            empModel.Action = 'U';        
            var response = await _employee.AddUpdateDeleteEmployee(empModel);
            TempData["response"] = response;

            return RedirectToAction("Index");

        }
    }
}