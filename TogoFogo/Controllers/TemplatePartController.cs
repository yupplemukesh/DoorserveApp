﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Permission;
using TogoFogo.Repository.EmailHeaderFooters;


namespace TogoFogo.Controllers
{
    public class TemplatePartController : Controller
    {
        private readonly ITemplateParts _templatePartRepo;
        public TemplatePartController()

        {
            _templatePartRepo = new TemplateParts();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Template Part")]
        public async Task<ActionResult> Index()
       {
            var templatepart = await _templatePartRepo.GetTemplatePart();
            TemplatePartMainModel model = new TemplatePartMainModel();
            model.TemplatePart = new TemplatePartModel();
            model.mainModel = Mapper.Map<List<TemplatePartModel>>(templatepart);           
            model.Rights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(model);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Template Part")]
        public async Task<ActionResult> Create()
        {
            var templatepartmodel = new TemplatePartModel();
            return View(templatepartmodel);
        }
        [HttpPost]
        public async Task<ActionResult> Create(TemplatePartModel templatepart)
        {
            if (ModelState.IsValid)
            {
                var templatepartModel = new TemplatePartModel
                {
                    TemplatePartId = templatepart.TemplatePartId,                    
                    TemplatePartName = templatepart.TemplatePartName,
                    IsActive= templatepart.IsActive,
                    HTMLPart = templatepart.HTMLPart,
                    PlainTextPart = templatepart.PlainTextPart,
                    AddeddBy = Convert.ToInt32(Session["User_ID"])
                };
                ResponseModel response = new ResponseModel();
                if (templatepartModel.TemplatePartId != 0)
                    response = await _templatePartRepo.AddUpdateDeleteTemplatePart(templatepartModel, 'U');
                else
                    response = await _templatePartRepo.AddUpdateDeleteTemplatePart(templatepartModel, 'I');
                _templatePartRepo.Save();
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else
                return View(templatepart);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit}, "Template Part")]
        public async Task<ActionResult> Edit(int id)
        {
            var templatepart = await _templatePartRepo.GetTemplatePartById(id);         
            return Json(templatepart, JsonRequestBehavior.AllowGet);
        }

    }

}