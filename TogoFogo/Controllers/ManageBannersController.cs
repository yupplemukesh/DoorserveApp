using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;

namespace TogoFogo.Controllers
{
    public class ManageBannersController : Controller
    {
        private readonly string _connectionString =
         ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // GET: ManageBanners
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }
        private string SaveImageFile(HttpPostedFileBase file)
        {
            try
            {
                System.IO.Stream stream = file.InputStream;
                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

                int height = image.Height;
                int width = image.Width;
                if (height == 620 && width == 1904)
                {
                    string path = Server.MapPath("~/Repairs_Crousel_Images");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //var fileFullName = file.FileName;
                    //var fileExtention = Path.GetExtension(fileFullName);
                    //var fileName = Path.GetFileNameWithoutExtension(fileFullName);
                    //var savedFileName = fileName + fileExtention;
                    //file.SaveAs(Path.Combine(path, savedFileName));
                    var renamedImage = "Webview1.jpg";
                    var fileExtention = Path.GetExtension(renamedImage);
                    var fileName = Path.GetFileNameWithoutExtension(renamedImage);
                    var savedFileName = renamedImage;
                    file.SaveAs(Path.Combine(path, savedFileName));
                    return savedFileName;
                }
                else {
                    return ViewBag.Message = "File size is not appropriate";
                }

             
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        private string SaveImageFile2(HttpPostedFileBase file)
        {
            try
            {
                System.IO.Stream stream = file.InputStream;
                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

                int height = image.Height;
                int width = image.Width;
                if (height == 620 && width == 1904)
                {
                    string path = Server.MapPath("~/Repairs_Crousel_Images");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //var fileFullName = file.FileName;
                    //var fileExtention = Path.GetExtension(fileFullName);
                    //var fileName = Path.GetFileNameWithoutExtension(fileFullName);
                    //var savedFileName = fileName + fileExtention;
                    //file.SaveAs(Path.Combine(path, savedFileName));
                    var renamedImage = "Webview2.jpg";
                    var fileExtention = Path.GetExtension(renamedImage);
                    var fileName = Path.GetFileNameWithoutExtension(renamedImage);
                    var savedFileName = renamedImage;
                    file.SaveAs(Path.Combine(path, savedFileName));
                    return savedFileName;
                }
                else
                {
                    return ViewBag.Message = "File size is not appropriate";
                }


            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        private string SaveImageFile3(HttpPostedFileBase file)
        {
            try
            {
                System.IO.Stream stream = file.InputStream;
                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

                int height = image.Height;
                int width = image.Width;
                if (height == 620 && width == 1904)
                {
                    string path = Server.MapPath("~/Repairs_Crousel_Images");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //var fileFullName = file.FileName;
                    //var fileExtention = Path.GetExtension(fileFullName);
                    //var fileName = Path.GetFileNameWithoutExtension(fileFullName);
                    //var savedFileName = fileName + fileExtention;
                    //file.SaveAs(Path.Combine(path, savedFileName));
                    var renamedImage = "Webview3.jpg";
                    var fileExtention = Path.GetExtension(renamedImage);
                    var fileName = Path.GetFileNameWithoutExtension(renamedImage);
                    var savedFileName = renamedImage;
                    file.SaveAs(Path.Combine(path, savedFileName));
                    return savedFileName;
                }
                else
                {
                    return ViewBag.Message = "File size is not appropriate";
                }


            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        private string SaveImageFile4(HttpPostedFileBase file)
        {
            try
            {
                System.IO.Stream stream = file.InputStream;
                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

                int height = image.Height;
                int width = image.Width;
                if (height == 300 && width == 411)
                {
                    string path = Server.MapPath("~/Repairs_Crousel_Images");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //var fileFullName = file.FileName;
                    //var fileExtention = Path.GetExtension(fileFullName);
                    //var fileName = Path.GetFileNameWithoutExtension(fileFullName);
                    //var savedFileName = fileName + fileExtention;
                    //file.SaveAs(Path.Combine(path, savedFileName));
                    var renamedImage = "Mobileview1.jpg";
                    var fileExtention = Path.GetExtension(renamedImage);
                    var fileName = Path.GetFileNameWithoutExtension(renamedImage);
                    var savedFileName = renamedImage;
                    file.SaveAs(Path.Combine(path, savedFileName));
                    return savedFileName;
                }
                else
                {
                    return ViewBag.Message = "File size is not appropriate";
                }


            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        private string SaveImageFile5(HttpPostedFileBase file)
        {
            try
            {
                System.IO.Stream stream = file.InputStream;
                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

                int height = image.Height;
                int width = image.Width;
                if (height == 300 && width == 411)
                {
                    string path = Server.MapPath("~/Repairs_Crousel_Images");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //var fileFullName = file.FileName;
                    //var fileExtention = Path.GetExtension(fileFullName);
                    //var fileName = Path.GetFileNameWithoutExtension(fileFullName);
                    //var savedFileName = fileName + fileExtention;
                    //file.SaveAs(Path.Combine(path, savedFileName));
                    var renamedImage = "Mobileview2.jpg";
                    var fileExtention = Path.GetExtension(renamedImage);
                    var fileName = Path.GetFileNameWithoutExtension(renamedImage);
                    var savedFileName = renamedImage;
                    file.SaveAs(Path.Combine(path, savedFileName));
                    return savedFileName;
                }
                else
                {
                    return ViewBag.Message = "File size is not appropriate";
                }


            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        private string SaveImageFile6(HttpPostedFileBase file)
        {
            try
            {
                System.IO.Stream stream = file.InputStream;
                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

                int height = image.Height;
                int width = image.Width;
                if (height == 300 && width == 411)
                {
                    string path = Server.MapPath("~/Repairs_Crousel_Images");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //var fileFullName = file.FileName;
                    //var fileExtention = Path.GetExtension(fileFullName);
                    //var fileName = Path.GetFileNameWithoutExtension(fileFullName);
                    //var savedFileName = fileName + fileExtention;
                    //file.SaveAs(Path.Combine(path, savedFileName));
                    var renamedImage = "Mobileview3.jpg";
                    var fileExtention = Path.GetExtension(renamedImage);
                    var fileName = Path.GetFileNameWithoutExtension(renamedImage);
                    var savedFileName = renamedImage;
                    file.SaveAs(Path.Combine(path, savedFileName));
                    return savedFileName;
                }
                else
                {
                    return ViewBag.Message = "File size is not appropriate";
                }


            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        public ActionResult DynamicCrousel()
        {
           
                return View();
         
        }
        [HttpPost]
        public ActionResult DynamicCrousel(DynamicCrouselModel m)
        {
            if (ModelState.IsValid)
            {                
               
                if (m.Image1 != null || m.Image2 != null || m.Image3 != null)
                {
                    if (m.Image1 != null)
                    {
                        
                        m.FirstImg = SaveImageFile(m.Image1);
                    }
                   
                    if (m.Image2 != null)
                    {
                        m.SecondImg = SaveImageFile2(m.Image2);
                    }
                   
                    if (m.Image3 != null)
                    {
                        m.ThirdImg = SaveImageFile3(m.Image3);
                    }
                   
                    if (m.FirstImg == "File size is not appropriate" || m.SecondImg == "File size is not appropriate"
                        || m.ThirdImg == "File size is not appropriate")
                    {
                        TempData["Message"] = "File size is not appropriate";
                    }
                    else
                    {
                            int result = 1;
                            if (result == 1)
                            {
                                TempData["Message"] = "Banners Updated Successfully";
                            }
                            else
                            {
                                TempData["Message"] = "Banners Not Updated";
                            }
                      
                    }
                }
                if (m.MobileImageUpload1 != null || m.MobileImageUpload2 != null || m.MobileImageUpload3 != null)
                {
                    if (m.MobileImageUpload1 != null)
                    {
                        m.MobileImage1 = SaveImageFile4(m.MobileImageUpload1);
                    }
                   
                    if (m.MobileImageUpload2 != null)
                    {
                        m.MobileImage2 = SaveImageFile5(m.MobileImageUpload2);
                    }
                    
                    if (m.MobileImageUpload3 != null)
                    {
                        m.MobileImage3 = SaveImageFile6(m.MobileImageUpload3);
                    }
                   
                    if (m.MobileImage1 == "File size is not appropriate" || m.MobileImage2 == "File size is not appropriate" || m.MobileImage3 == "File size is not appropriate")
                    {
                        TempData["Message"] = "File size is not appropriate";
                    }
                    else
                    {
                        int result = 1;
                            if (result == 1)
                            {
                                TempData["Message"] = "Banners Updated Successfully";
                            }
                            else
                            {
                                TempData["Message"] = "Banners Not Updated";
                            }
                       
                    }
                }
                
            }
                
            return RedirectToAction("Index","ManageBanners");

        }
    }
}