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
using TogoFogo.Repository.ManageRegion;

namespace TogoFogo.Controllers
{
    public class ManageRegionController : Controller
    {
        private readonly IRegion _Region;
        private readonly DropdownBindController _Dropdown;
        public ManageRegionController()
        {
            _Region = new Region();
            _Dropdown = new DropdownBindController();
        }
        // GET: ManageRegion
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Regions)]
        public async Task<ActionResult> Index()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel();
            filter.CompId = session.CompanyId;
            var Region = await _Region.GetAllRegion(filter);
            return View(Region);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Regions)]
        public async Task<ActionResult> Create()
        {
            ManageRegionModel MR = new ManageRegionModel();
            MR.StateList= new SelectList(_Dropdown.BindState(), "Value", "Text");
            return View(MR);
        }
        [HttpPost]
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Regions)]
        public async Task<ActionResult> Create(ManageRegionModel Region)
        {
            var session = Session["User"] as SessionModel;
            Region.EventAction = 'I';
            Region.UserId = session.UserId;
            Region.CompanyId = session.CompanyId;
            var response = await _Region.AddUpdateRegion(Region);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Regions)]
        public async Task<ActionResult> Edit(Guid RegionId)
        {
            var session = Session["User"] as SessionModel;
            var Region = await _Region.GetRegionById(RegionId);
            Region.StateList = new SelectList(_Dropdown.BindState(), "Value", "Text");
            return View(Region);
        }
        [HttpPost]
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Regions)]
        public async Task<ActionResult> Edit(ManageRegionModel Region)
        {
            var session = Session["User"] as SessionModel;
            Region.EventAction = 'U';
            Region.UserId = session.UserId;
            Region.CompanyId = session.CompanyId;
            var response = await _Region.AddUpdateRegion(Region);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
    }
}