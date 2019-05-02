using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Permission;
using TogoFogo.Repository;

namespace TogoFogo.Controllers
{
    public class ActionTypesController : Controller
    {
        private SessionModel user;
        private readonly IActionTypes _actionTypeModel;
        public ActionTypesController()
        {
            _actionTypeModel = new ActionTypes();

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Action_Types)]
        public async Task<ActionResult> Index()
        {
            var actiontypes = await _actionTypeModel.GetActiontypes();           
            return View(actiontypes);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Action_Types)]
        public async Task<ActionResult> Create()
        {
            var actiontypemodel = new ActionTypeModel();
            return View(actiontypemodel);
        }
        [HttpPost]
        public async Task<ActionResult> Create(ActionTypeModel actiontype)
        {
            if (ModelState.IsValid)
            {
                user = Session["User"] as SessionModel;
                actiontype.AddeddBy = user.UserId;
                var response = await _actionTypeModel.AddUpdateDeleteActionTypes(actiontype, 'I');
                _actionTypeModel.Save();
                TempData["response"] = response;
                return RedirectToAction("Index");
            }
            else
                return View(actiontype);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Action_Types)]
        public async Task<ActionResult> Edit(int id)
        {
            var actiontype = await _actionTypeModel.GetActionByActionId(id);
            return View(actiontype);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ActionTypeModel actiontype)
        {
            if (ModelState.IsValid)
            {
                
                    user = Session["User"] as SessionModel;
                    actiontype.AddeddBy = user.UserId;
                var response = await _actionTypeModel.AddUpdateDeleteActionTypes(actiontype, 'U');
                _actionTypeModel.Save();
                TempData["response"] = response;
                return RedirectToAction("Index");
            }
            else

                return View(actiontype);
        
        }

    }

}
