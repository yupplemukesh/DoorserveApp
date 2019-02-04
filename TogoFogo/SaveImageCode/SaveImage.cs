using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TogoFogo.SaveImageCode
{
    public class SaveImage
    {
        // File Save Code
        private string SaveImageFile(HttpPostedFileBase file)
        {
            try
            {

                string path = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Path.GetFileNameWithoutExtension(fileFullName);
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                return savedFileName;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}