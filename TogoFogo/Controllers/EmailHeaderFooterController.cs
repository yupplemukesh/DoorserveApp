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
    public class EmailHeaderFooterController : Controller
    {
        private readonly IEmailHeaderFooters _emailHeaderFooterRepo;
        public EmailHeaderFooterController()

        {
            _emailHeaderFooterRepo = new EmailHeaderFooters();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "E-Mail Header and Footer Template")]
        public async Task<ActionResult> Index()
       {
            var emailheaderfooter = await _emailHeaderFooterRepo.GetEmailHeaderFooters();
            EmailHeaderFooterMainModel model = new EmailHeaderFooterMainModel();
            model.EmailHeaderFooter = new EmailHeaderFooterModel();
            model.mainModel = Mapper.Map<List<EmailHeaderFooterModel>>(emailheaderfooter);
            model.EmailHeaderFooter.ActionTypeList = new SelectList( await CommonModel.GetActionTypes(),"Value","Text");        
            return View(model);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "E-Mail Header and Footer Template")]
        public async Task<ActionResult> Create()
        {
            var emailheaderfootermodel = new EmailHeaderFooterModel();
            return View(emailheaderfootermodel);
        }
        [HttpPost]
        public async Task<ActionResult> Create(EmailHeaderFooterModel emailheaderfooter)
        {
            if (ModelState.IsValid)
            {
                var emailheaderfooterModel = new EmailHeaderFooterModel
                {
                    EmailHeaderFooterId = emailheaderfooter.EmailHeaderFooterId,
                    ActionTypeId = emailheaderfooter.ActionTypeId,
                    Name = emailheaderfooter.Name,
                    ISACTIVE=emailheaderfooter.ISACTIVE,
                    HeaderHTML = emailheaderfooter.HeaderHTML,
                    FooterHTML = emailheaderfooter.FooterHTML,
                    AddeddBy = Convert.ToInt32(Session["User_ID"])
                };
                ResponseModel response = new ResponseModel();
                if (emailheaderfooterModel.EmailHeaderFooterId != 0)
                    response = await _emailHeaderFooterRepo.AddUpdateDeleteEmailHeaderFooter(emailheaderfooterModel, 'U');
                else
                    response = await _emailHeaderFooterRepo.AddUpdateDeleteEmailHeaderFooter(emailheaderfooterModel, 'I');
                _emailHeaderFooterRepo.Save();
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else
                return View(emailheaderfooter);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit}, "E-Mail Header and Footer Template")]
        public async Task<ActionResult> Edit(int id)
        {
            var emailheaderfooter = await _emailHeaderFooterRepo.GetEmailHeaderFooterById(id);
            var seletedActions = emailheaderfooter.ActionTypeIds.Split(',').ToList();
            emailheaderfooter.ActionTypeId = seletedActions.Select(int.Parse).ToList();
            return Json(emailheaderfooter, JsonRequestBehavior.AllowGet);
        }

    }

}