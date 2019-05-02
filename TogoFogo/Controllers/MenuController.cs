using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Repository.Menues;

namespace TogoFogo.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public readonly IMenues _menues;
        private string path;
        public MenuController()
        {        
            _menues = new Menues();
             path= "~/UploadedImages/icon-img/";
        }
       public async  Task<ActionResult> Index()
        {
            var menuesModel = new MenuesModel();
            menuesModel.Menues =await _menues.GetMenues();
            menuesModel.menu = new MenuMasterModel {ServiceTypeList=await CommonModel.GetServiceType(null)};  
            return View(menuesModel);
        }

        public async Task<JsonResult> GetMenu(string menuCapId)
        {
          var result = await _menues.GetMenuById(menuCapId);
            result.PagePath = path;
          return Json(result, JsonRequestBehavior.AllowGet);

        }

        private string SaveImageFile(HttpPostedFileBase file)
        {
            try
            {
                path = Server.MapPath(path);     
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Guid.NewGuid();
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                return savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(MenuMasterModel menu)
        {

            if (menu.IconFileName != null && menu.IconFileNamePath != null)
            {
                if (System.IO.File.Exists(Server.MapPath(path) + menu.IconFileName))
                    System.IO.File.Delete(Server.MapPath(path) + menu.IconFileName);
            }
            if (menu.IconFileNamePath != null)
                menu.IconFileName = SaveImageFile(menu.IconFileNamePath);
            string services = "";
            foreach (var item in menu.ServiceTypeList)
            {
                if(item.IsChecked)
                services = services + ","+item.Value;
            }
            menu.ServiceTypeIds = services.Trim(',');
            ResponseModel res = null;
                if (menu.MenuCapId == 0)
                 res = await _menues.AddUpdateMenu(menu, 'I');
                else
                 res = await _menues.AddUpdateMenu(menu, 'U');
            TempData["response"] = res;   
            var menuesModel = new MenuesModel();
            menuesModel.Menues = await _menues.GetMenues();
            menuesModel.menu = new MenuMasterModel { ServiceTypeList = await CommonModel.GetServiceType(null) };
            return RedirectToAction ("index", menuesModel);


        }
    }
}