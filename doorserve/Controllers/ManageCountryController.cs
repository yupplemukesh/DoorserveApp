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
using doorserve.Repository.Country;

namespace doorserve.Controllers
{
    public class ManageCountryController : Controller
    {
        private readonly ICountry _Country;       
        public ManageCountryController()
        {
            _Country = new Country();
          
        }

        // GET: ManageCountry
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Countries)]
        public async Task<ActionResult> Index()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel();           
            var Country = await _Country.GetAllCountry(filter);
            return View(Country);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Countries)]
        public async Task<ActionResult> Create()
        {
             
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Countries)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Create(ManageCountryModel contry)
        {
            var session = Session["User"] as SessionModel;
            contry.EventAction = 'I';
            contry.UserId = session.UserId;            
            var response = await _Country.AddUpdateCountry(contry);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Countries)]
        public async Task<ActionResult> Edit(long Cnty_Id)
        {
            var session = Session["User"] as SessionModel;
            var Pro = await _Country.GetCountryById(Cnty_Id);
            return View(Pro);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Countries)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Edit(ManageCountryModel contry)
        {
            var session = Session["User"] as SessionModel;
            contry.EventAction = 'U';
            contry.UserId = session.UserId;            
            var response = await _Country.AddUpdateCountry(contry);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }

    }
}