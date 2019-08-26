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
    public class ProcessController : Controller
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

            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Process)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Create(ProcessModel Process)
        {
            var session = Session["User"] as SessionModel;
            Process.Action = 'I';
            Process.UserId = session.UserId;
            Process.CompanyId = session.CompanyId;
            var response = await _Process.AddUpdateProcess(Process);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Process)]
        public async Task<ActionResult> Edit(int ProcessId)
        {
            var session = Session["User"] as SessionModel;
            var Pro = await _Process.GetProcessesById(ProcessId);
            return View(Pro);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Process)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Edit(ProcessModel Process)
        {
            var session = Session["User"] as SessionModel;
            Process.Action = 'U';
            Process.UserId = session.UserId;
            Process.CompanyId = session.CompanyId;
            var response = await _Process.AddUpdateProcess(Process);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        }
}