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
using doorserve.Repository.Process;

namespace doorserve.Controllers
{
    public class ProcessController : BaseController
    {
        private readonly IProcesses _Process;
        public ProcessController()
        {
            _Process = new Process();
        }

        // GET: Process
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Process)]
        public async Task<ActionResult> Index()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel();
            filter.CompId = session.CompanyId;
            var Process = await _Process.GetAllProcesses(filter);
            return View(Process);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Process)]
        public async Task<ActionResult> Create()
        {
            var process = new ProcessModel();
            if (CurrentUser.UserTypeName.ToLower() == "super admin")
            {
                process.IsAdmin = true;
                process.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text"); 
                    }
            return View(process);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Process)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Create(ProcessModel Process)
        {   
            Process.EventAction = 'I';
            Process.UserId = CurrentUser.UserId;
            Process.CompanyId = CurrentUser.CompanyId;
            if (CurrentUser.UserTypeName.ToLower() == "super admin")
                Process.CompanyId = Process.CompanyId;
                var response = await _Process.AddUpdateProcess(Process);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Process)]
        public async Task<ActionResult> Edit(int ProcessId)
        {

            var Pro = await _Process.GetProcessesById(ProcessId);
            if (CurrentUser.UserTypeName.ToLower() == "super admin")
            {
                Pro.IsAdmin = true;
                Pro.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
            }
            return View(Pro);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Process)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Edit(ProcessModel Process)
        {
            Process.EventAction = 'U';
            Process.UserId = CurrentUser.UserId;           
            Process.CompanyId = CurrentUser.CompanyId;
            if (CurrentUser.UserTypeName.ToLower() == "super admin")
                Process.CompanyId = Process.CompanyId;
            var response = await _Process.AddUpdateProcess(Process);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        }
}