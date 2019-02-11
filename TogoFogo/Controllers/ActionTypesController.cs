﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Repository;

namespace TogoFogo.Controllers
{
    public class ActionTypesController : Controller
    {
        private readonly IActionTypes _actionTypeModel;
        public ActionTypesController()
        {
            _actionTypeModel = new ActionTypes();

        }

        public async Task<ActionResult> Index()
        {
            var actiontypes = await _actionTypeModel.GetActiontypes();
            return View(actiontypes);
        }

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
                actiontype.AddeddBy = Convert.ToInt32(Session["User_ID"]);
                var response = await _actionTypeModel.AddUpdateDeleteActionTypes(actiontype, 'I');
                _actionTypeModel.Save();
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else
                return View(actiontype);

        }
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
                var response = await _actionTypeModel.AddUpdateDeleteActionTypes(actiontype, 'U');
                _actionTypeModel.Save();
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else

                return View(actiontype);
        
        }

    }

}
