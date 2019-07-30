using AutoMapper;
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
using TogoFogo.Repository.ManagePageContents;

namespace TogoFogo.Controllers
{
    public class ManagePageContentsController : Controller
    {
        private readonly IPageContent _PageContent;
        private readonly DropdownBindController dropdown;
        public ManagePageContentsController()
        {
            _PageContent = new PageContent();
            dropdown = new DropdownBindController();
        }
        // GET: ManagePageContent
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Page_Contents)]
        public async Task<ActionResult> Index()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId };           
           ManagePageContentsModel PageContentModel = new ManagePageContentsModel();            
            PageContentModel.MainContent = await _PageContent.GetAllPageContent(filter);
            PageContentModel.DynamicContent = new ManagePageContentsModel();
            PageContentModel.DynamicContent.PageNameList = new SelectList(await CommonModel.GetLookup("Page"), "Value", "Text");    
            PageContentModel.DynamicContent.SectionNameList= new SelectList(Enumerable.Empty<SelectList>());
            return View(PageContentModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Page_Contents)]
        public async Task<ActionResult> Create()
        {
           
            var PageContentModel = new ManagePageContentsModel();
            PageContentModel.PageNameList = new SelectList(await CommonModel.GetLookup("Page"), "Value", "Text");
            PageContentModel.SectionNameList = new SelectList(Enumerable.Empty<SelectList>());
            return View(PageContentModel);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Page_Contents)]
        [HttpPost]
        public async Task<ActionResult> Create(ManagePageContentsModel PageContent)
        {
            var SessionModel = Session["User"] as SessionModel;
            PageContent.UserId = SessionModel.UserId;
            PageContent.CompanyId = SessionModel.CompanyId;
            ResponseModel response = new ResponseModel();
            if (PageContent.ContentId==Guid.Empty )
            { 
            PageContent.EventAction = 'I';
            response = await _PageContent.AddUpdatePageContent(PageContent);
            }
            else
            {
                PageContent.EventAction = 'U';
                response = await _PageContent.AddUpdatePageContent(PageContent);
            }
            TempData["response"] = response;
            return RedirectToAction("Index");


        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit}, (int)MenuCode.Manage_Page_Contents)]
        public async Task<ActionResult> Edit(Guid Id)
        {          
            var PageContent = await _PageContent.GetPageContentById(Id);           
            return Json(PageContent,JsonRequestBehavior.AllowGet);
        }

        
    }
}
