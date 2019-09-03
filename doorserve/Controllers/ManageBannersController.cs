﻿using System;
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
using doorserve.Repository.ManageBanners;
using Newtonsoft.Json;

namespace doorserve.Controllers
{
    public class ManageBannersController : BaseController
    {
        private readonly IBanner _Banner;     
        public ManageBannersController()
        {
            _Banner = new Banner();
        }
        // GET: ManageBanners
        public async Task<ActionResult> Index()
        {


            Guid? BannerId = null;
            var filter = new FilterModel { CompId = CurrentUser.CompanyId, RefKey = BannerId };         
            var Banner = await _Banner.GetBanner(filter);          
            return View(Banner);
        }
        
      


        private string SaveImageFile(HttpPostedFileBase file, string folderName)
        {
            try
            {
                string path = Server.MapPath("~/Files/" + folderName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Guid.NewGuid();
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                
                return path + "\\" + savedFileName;
              
               
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }

        public async Task<ActionResult> Create()
        {
            var filter = new FilterModel { CompId = CurrentUser.CompanyId };
            var Banner = new ManageBannersModel();
            Banner.ImgDetails = new List<ManageBannerUploadModel>();
            Banner.PageNameList = new SelectList(await CommonModel.GetLookup("Page"), "Value", "Text");          
            Banner.SectionNameList = new SelectList(Enumerable.Empty<SelectList>());       
            return View(Banner);

        }

       
        [HttpPost]
        [ValidateInput(false)]
        [ValidateModel]
        public async Task<ActionResult> Create(ManageBannersModel Banner)
        {
           
            var ImageDetail = Request.Params["ImgDetail"];
            Banner = JsonConvert.DeserializeObject<ManageBannersModel>(ImageDetail);

            int i = 0;
            string directory = "~/TempFiles/Banners/"+Banner.Name;
            string path = Server.MapPath(directory);           
            foreach (var ban in Banner.ImgDetails)
            {
                ban.BannerFile = Request.Files["SlideImg"+i];
                if (ban.BannerFile != null)
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    if (ban.BannerFile != null)                        
                    ban.BannerFileName = "SlideImg"+i + Path.GetExtension(Path.Combine(directory, ban.BannerFile.FileName));
                    if (System.IO.File.Exists(path + "/" + ban.BannerFileName))
                        System.IO.File.Delete(path + "/" + ban.BannerFileName);
                    ban.BannerFile.SaveAs(path + "/" + ban.BannerFileName);
                    ban.BannerFile = null;
                }
                i++;
            }

            Banner.UserId = CurrentUser.UserId;
            Banner.CompanyId = CurrentUser.CompanyId;
            ResponseModel response = new ResponseModel();
            if (Banner.BannerId == null)
            {
                Banner.EventAction = 'I';
                response = await _Banner.AddUpdateBanner(Banner);
            }
            else
            {
                Banner.EventAction = 'U';
                response = await _Banner.AddUpdateBanner(Banner);
            }
           
            return Json("Ok", JsonRequestBehavior.AllowGet);

            
            }



        public async Task<ActionResult> Edit(Guid Id)
        {
          
         var Banner = await _Banner.GetBannerById(Id);
          Banner.PageNameList = new SelectList(await CommonModel.GetLookup("Page"), "Value", "Text");
         Banner.SectionNameList = new SelectList(await CommonModel.GetSection(Banner.PageId), "Name", "Text");
         return View("Create",Banner);
        }


      
        [HttpPost]
        public JsonResult UploadFile()
        {
            string directory = "~/TempFiles/";
            HttpPostedFileBase file = Request.Files["file"];          
            var HeaderTitle = Request.Params["HeaderTitle"];

            if (System.IO.File.Exists(directory +  "/" + HeaderTitle + Path.GetExtension(Path.Combine(directory, file.FileName))))
                System.IO.File.Delete(directory +  "/" + HeaderTitle + Path.GetExtension(Path.Combine(directory, file.FileName)));
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                string path = Server.MapPath(directory);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                file.SaveAs(path + "/" + HeaderTitle + Path.GetExtension(Path.Combine(directory, file.FileName)));
            }
            return Json(HeaderTitle + Path.GetExtension(Path.Combine(directory, file.FileName)), JsonRequestBehavior.AllowGet);
        }

    }


}

