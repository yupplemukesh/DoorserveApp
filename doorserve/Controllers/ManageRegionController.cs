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
using doorserve.Repository.ManageRegion;

namespace doorserve.Controllers
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
        private T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Regions)]
        public async Task<ActionResult> Create()
        {
            var session = Session["User"] as SessionModel;
            ManageRegionModel MR = new ManageRegionModel();
            MR.StateList= new SelectList(_Dropdown.BindState(), "Value", "Text");
            if (session.UserTypeName.ToLower() == "super admin")
            {
                MR.IsAdmin = true;
                MR.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
            }
            return View(MR);
        }
        [HttpPost]
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Regions)]
        [ValidateModel]
        public async Task<ActionResult> Create(ManageRegionModel Region)
        {
            var session = Session["User"] as SessionModel;
            Region.EventAction = 'I';
            Region.UserId = session.UserId;
            if (session.UserTypeName.ToLower() == "company")
                Region.CompanyId = session.CompanyId;
            var response = await _Region.AddUpdateRegion(Region);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Regions)]
        public async Task<ActionResult> Edit(Guid RegionId)
        {
            var Region = await _Region.GetRegionById(RegionId);
            var selectedItems = new List<int>();
            if (Region.StateXml != null)
            {
                var states = Deserialize<List<StateModel>>(Region.StateXml);
       
                foreach (var item in states)
                {
                    selectedItems.Add(Convert.ToInt32(item.St_ID));
                }
            }
            Region.StateList = new SelectList(_Dropdown.BindState(), "Value", "Text");
            Region.SelectedStates = selectedItems;
            return View(Region);
        }
        [HttpPost]
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Regions)]
        [ValidateModel]
        public async Task<ActionResult> Edit(ManageRegionModel Region)
        {
            var session = Session["User"] as SessionModel;
            Region.EventAction = 'U';
            Region.UserId = session.UserId;         
            var response = await _Region.AddUpdateRegion(Region);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
    }
}