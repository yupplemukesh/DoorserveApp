using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Permission;
using doorserve.Repository;
using doorserve.Repository.EmailSmsServices;

namespace doorserve.Controllers
{
    public class EmployeesController : BaseController
    {
        private readonly IEmployee _employee;
        private readonly DropdownBindController drop;
        private readonly string folderPath;
        private readonly doorserve.Repository.EmailSmsTemplate.ITemplate _templateRepo;
        private readonly IEmailSmsServices _emailSmsServices;
        public EmployeesController()
        {
            _employee = new Employee();
            drop = new DropdownBindController();
            folderPath = "/UploadedImages/employees/";
            _templateRepo = new doorserve.Repository.EmailSmsTemplate.Template();
            _emailSmsServices = new Repository.EmailsmsServices();
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
            var filter = new FilterModel();
            if (CurrentUser.UserTypeName.ToLower().Contains("provider"))
                filter.ProviderId = CurrentUser.RefKey;
            if (CurrentUser.UserTypeName.ToLower().Contains("center"))
                filter.RefKey = CurrentUser.RefKey;
            filter.CompId = CurrentUser.CompanyId;
            var employee = await _employee.GetAllEmployees(filter);
            return View(employee);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Engineers)]
        public async Task<ActionResult> Create()
        {
            var empModel = new EmployeeModel();
            empModel.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            empModel.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");         
            empModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            empModel.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            empModel.CityList = new SelectList(Enumerable.Empty<SelectList>());
            empModel.LocationList = new SelectList(Enumerable.Empty<SelectListItem>());
            empModel.StateList = new SelectList(Enumerable.Empty<SelectList>());
            empModel.CenterList = new SelectList(Enumerable.Empty<SelectList>());
            empModel.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"), "Value", "Text");
            empModel.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");
            if (CurrentUser.UserTypeName.ToLower().Contains("provider"))
            {
                empModel.IsProvider = true;
                if (CurrentUser.UserRole.Contains("Service Provider SC Admin"))
                    empModel.ProviderId = CurrentUser.RefKey;
                else
                    empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(CurrentUser.RefKey), "Name", "Text");
            }
            else if (CurrentUser.UserTypeName.ToLower().Contains("center"))
            {
                empModel.RefKey = CurrentUser.RefKey;              
                empModel.IsCenter = true;
                empModel.ProviderList = new SelectList(Enumerable.Empty<SelectList>());
            }
            else
                empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(CurrentUser.CompanyId), "Name", "Text");
            return View(empModel);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Engineers)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Create(EmployeeModel emp)
        {
            if (emp.EMPPhoto1 != null)
                emp.EMPPhoto = SaveImageFile(emp.EMPPhoto1, "DP");
            if (emp.ConAdhaarNumberFilePath != null)
                emp.ConAdhaarFileName = SaveImageFile(emp.ConAdhaarNumberFilePath, "ADHRS");
            if (emp.ConVoterIdFilePath != null)
                emp.ConVoterIdFileName = SaveImageFile(emp.ConVoterIdFilePath, "VoterIds");
            if (emp.ConPanNumberFilePath != null)
                emp.ConPanFileName = SaveImageFile(emp.ConPanNumberFilePath, "PANCards");
            emp.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            emp.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
            emp.ProviderList = new SelectList(await CommonModel.GetServiceProviders(CurrentUser.CompanyId), "Name", "Text");
            emp.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            emp.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            emp.CityList = new SelectList(Enumerable.Empty<SelectList>());
            emp.StateList = new SelectList(Enumerable.Empty<SelectList>());
            emp.CenterList = new SelectList(Enumerable.Empty<SelectList>());
            emp.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"),"Value","Text");
            emp.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");
            emp.EventAction = 'I';
            emp.UserId = CurrentUser.UserId;
            emp.CompanyId = CurrentUser.CompanyId;
            var pwd = CommonModel.RandomPassword(8);
            if (emp.IsUser)
                emp.Password = Encrypt_Decript_Code.encrypt_decrypt.Encrypt(pwd, true);
            if (CurrentUser.UserTypeName.ToLower().Contains("provider"))
            {
                if (!CurrentUser.UserRole.Contains("Service Provider SC Admin"))
                    emp.CenterList = new SelectList(await CommonModel.GetServiceCenters(CurrentUser.RefKey), "Name", "Text");

            }          
         
            var response = await _employee.AddUpdateDeleteEmployee(emp);
            if(response.IsSuccess)
            {
               
                    if (emp.IsUser)
                    {
                        var Templates = await _templateRepo.GetTemplateByActionName("User Registration", CurrentUser.CompanyId);
                    CurrentUser.Email = emp.ConEmailAddress;
                        var WildCards =  CommonModel.GetWildCards(CurrentUser.CompanyId);
                        var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                        U.Val = emp.ConFirstName;
                        U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                        U.Val = pwd;
                        U = WildCards.Where(x => x.Text.ToUpper() == "USER NAME").FirstOrDefault();
                        U.Val = emp.ConEmailAddress;
                    CurrentUser.Mobile = emp.ConMobileNumber;
                        var c = WildCards.Where(x => x.Val != string.Empty).ToList();
                        if (Templates != null)
                            await _emailSmsServices.Send(Templates, c, CurrentUser);
                    }
                

                }
            TempData["response"] = response;
            return RedirectToAction("Index");

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Engineers)]
        public async Task<ActionResult> Edit(Guid empId)
        {
            var empModel = await _employee.GetEmployeeById(empId);

            empModel.RefKey = empModel.CenterId;
            empModel.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            empModel.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
            empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(CurrentUser.CompanyId), "Name", "Text");
            empModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
           empModel.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
           //empModel.StateList = new SelectList(drop.BindState(empModel.CountryId), "Value", "Text");
           empModel.LocationList = new SelectList(drop.BindLocationforEmp(empModel.LocationId), "Value", "Text");
            empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(empModel.ProviderId), "Name", "Text");
            empModel.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"),"Value","Text");
            empModel.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");          
            empModel.CurrentEmail = empModel.ConEmailAddress;
            empModel.CurrentIsUser = empModel.IsUser;
            if (CurrentUser.UserTypeName.ToLower().Contains("provider"))
            {
                empModel.IsProvider = true;
                if (!CurrentUser.UserRole.Contains("Service Provider SC Admin"))
                    empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(CurrentUser.RefKey), "Name", "Text");
            }
            else if (CurrentUser.UserTypeName.ToLower().Contains("center"))
            {
                empModel.ProviderList = new SelectList(Enumerable.Empty<SelectList>());
                empModel.IsCenter = true;
            }
            else
                empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(CurrentUser.CompanyId), "Name", "Text");

            return View(empModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Engineers)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Edit(EmployeeModel empModel)
         {
            empModel.UserId = CurrentUser.UserId;
            empModel.CompanyId = CurrentUser.CompanyId;
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
            var pwd = "CA5680";
            if (empModel.IsUser)
                empModel.Password = Encrypt_Decript_Code.encrypt_decrypt.Encrypt(pwd, true);


            empModel.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            empModel.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
            empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(CurrentUser.CompanyId), "Name", "Text");
            empModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            //empModel.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            //empModel.StateList = new SelectList(drop.BindState(empModel.CountryId), "Value", "Text");
            //empModel.CityList = new SelectList(drop.BindLocation(empModel.StateId), "Value", "Text");
            empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(empModel.ProviderId), "Name", "Text");
            empModel.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"), "Value", "Text");
            empModel.EngineerTypeList = new SelectList(await CommonModel.GetLookup("Engineer Type"), "Value", "Text");
            empModel.EventAction = 'U';
            if (CurrentUser.UserTypeName.ToLower().Contains("provider"))
            {              
                if (!CurrentUser.UserRole.Contains("Service Provider SC Admin"))
                    empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(CurrentUser.RefKey), "Name", "Text");
            }
            else if (CurrentUser.UserTypeName.ToLower().Contains("center"))
                empModel.ProviderList = new SelectList(Enumerable.Empty<SelectList>());
            else
                empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(CurrentUser.CompanyId), "Name", "Text");
            var response = await _employee.AddUpdateDeleteEmployee(empModel);
            if (response.IsSuccess)
            {

                if (empModel.IsUser && empModel.CurrentIsUser)
                {
                    var Templates = await _templateRepo.GetTemplateByActionName("User Registration", CurrentUser.CompanyId);
                    CurrentUser.Email = empModel.ConEmailAddress;
                    var WildCards =  CommonModel.GetWildCards(CurrentUser.CompanyId);
                    var U = WildCards.Where(x => x.Text.ToUpper() == "NAME").FirstOrDefault();
                    U.Val = empModel.ConFirstName;
                    U = WildCards.Where(x => x.Text.ToUpper() == "PASSWORD").FirstOrDefault();
                    U.Val = pwd;
                    U = WildCards.Where(x => x.Text.ToUpper() == "USER NAME").FirstOrDefault();
                    U.Val = empModel.ConEmailAddress;
                    CurrentUser.Mobile = empModel.ConMobileNumber;
                    var c = WildCards.Where(x => x.Val != string.Empty).ToList();
                    if (Templates != null)
                        await _emailSmsServices.Send(Templates, c, CurrentUser);
                }

            }
            TempData["response"] = response;

            return RedirectToAction("Index");

        }

        public async Task<ActionResult> GetEmployeeLocationByPinCode(string pin)

        {

            var PinCodeDetails = await _employee.GetPinCode(pin);

            return Json(PinCodeDetails, JsonRequestBehavior.AllowGet);

        }
    }
}