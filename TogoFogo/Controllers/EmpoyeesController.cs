using System;
using System.Collections.Generic;
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
        public EmployeesController()
        {
            _employee = new Employee();
            drop = new DropdownBindController();

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
            if (Session["RoleName"].ToString().ToLower() == "provider")
            {
               var ProviderId = await CommonModel.GetProviderIdByUser(Convert.ToInt32(Session["User_ID"]));
                empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(ProviderId));
            }
            else
                empModel.CenterList = new SelectList(Enumerable.Empty<SelectList>());
            empModel.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"),"Value","Text");
            return View(empModel);

        }
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Create(EmployeeModel emp,ContactPersonModel contact)
        {
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
            return View(empModel);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Edit(EmployeeModel empModel, ContactPersonModel contact)
        {
            empModel.Contact = contact;
            empModel.DeginationList = new SelectList(await CommonModel.GetDesignations(), "Value", "Text");
            empModel.DepartmentList = new SelectList(await CommonModel.GetDepartments(), "Value", "Text");
            empModel.ProviderList = new SelectList(await CommonModel.GetServiceProviders(), "Name", "Text");
            empModel.Contact.AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text");
            empModel.Contact.CountryList = new SelectList(drop.BindCountry(), "Value", "Text");
            empModel.Contact.StateList = new SelectList(drop.BindState(empModel.Contact.CountryId), "Value", "Text");
            empModel.Contact.CityList = new SelectList(drop.BindLocation(empModel.Contact.StateId), "Value", "Text");
            empModel.CenterList = new SelectList(await CommonModel.GetServiceCenters(empModel.ProviderId), "Name", "Text");
            empModel.Vehicle.VehicleTypeList = new SelectList(await CommonModel.GetLookup("Vehicle"));
            empModel.Action = 'U';        
            var resonse = await _employee.AddUpdateDeleteEmployee(empModel);
            TempData["resonse"] = resonse;
            TempData.Keep("resonse");
            return RedirectToAction("Index");

        }
    }
}