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
using doorserve.Repository.State;

namespace doorserve.Controllers
{
    public class ManageStateController : BaseController
    {
        private readonly IState _State;
        private readonly DropdownBindController dropdown;
        public ManageStateController()
        {
            _State = new State();
            dropdown = new DropdownBindController();
        }
        // GET: ManageState
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_States_UTs)]
        public async Task<ActionResult> Index()
        {
            var filter = new FilterModel();
            var State = await _State.GetAllState(filter);
            return View(State);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_States_UTs)]
        public async Task<ActionResult> Create()
        {
            ManageStateModel st = new ManageStateModel
            {
                _CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text")
            };
            return PartialView(st);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_States_UTs)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Create(ManageStateModel state)
        {

            state.EventAction = 'I';
            state.UserId = CurrentUser.UserId;
            var response = await _State.AddUpdateState(state);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit}, (int)MenuCode.Manage_States_UTs)]
        public async Task<ActionResult> Edit(long St_ID)
        {
            var Result = await _State.GetStateById(St_ID);
            Result._CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            return View(Result);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_States_UTs)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Edit(ManageStateModel state)
        {
            state.EventAction = 'U';
            state.UserId = CurrentUser.UserId;
            var response = await _State.AddUpdateState(state);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
    }
}