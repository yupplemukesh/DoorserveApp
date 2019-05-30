using AutoMapper;
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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Template_Part)]
        public async Task<ActionResult> Index()
        {
            var SessionModel = Session["User"] as SessionModel;
            var templatePart =new TemplatePartMainModel();
            templatePart.mainModel = await _templatePartRepo.GetTemplatePart(new Filters.FilterModel {CompId=SessionModel.CompanyId });
            templatePart.TemplatePart = new TemplatePartModel();
            return View(templatePart);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Template_Part)]
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
                var SessionModel = Session["User"] as SessionModel;
                var templatepartModel = new TemplatePartModel
                {
                    TemplatePartId = templatepart.TemplatePartId,                    
                    TemplatePartName = templatepart.TemplatePartName,
                    IsActive= templatepart.IsActive,
                    HTMLPart = templatepart.HTMLPart,
                    PlainTextPart = templatepart.PlainTextPart,
                    UserId = SessionModel.UserId,
                    CompanyId=SessionModel.CompanyId
                };
                ResponseModel response = new ResponseModel();
                if (templatepartModel.TemplatePartId != 0)
                    response = await _templatePartRepo.AddUpdateDeleteTemplatePart(templatepartModel, 'U');
                else
                    response = await _templatePartRepo.AddUpdateDeleteTemplatePart(templatepartModel, 'I');
                _templatePartRepo.Save();
                TempData["response"] = response;
                return RedirectToAction("Index");
            }
            else
                return View(templatepart);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit}, (int)MenuCode.Template_Part)]
        public async Task<ActionResult> Edit(int id)
        {
            var templatepart = await _templatePartRepo.GetTemplatePartById(id);         
            return Json(templatepart, JsonRequestBehavior.AllowGet);
        }

    }

}