using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;

namespace TogoFogo.Controllers
{
    public class ManageEngineersController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // File Save Code
        private string SaveImageFile(HttpPostedFileBase file)
        {
            try
            {
                string path = Server.MapPath("~/Uploaded Images");
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

                return ViewBag.Message = ex.Message;
            }
        }
        // GET: ManageEngineers
        public ActionResult Me()
        {
            ViewBag.Trc = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                
            }
            return View();
        }
        public ActionResult AddEngineer()
        {
            ViewBag.Trc = new SelectList(dropdown.BindTrc(), "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult AddEngineer(ManageEngineerModel model)
        {
            try
            {
                if (model.EngineerPhoto1 != null)
                {
                    model.EngineerPhoto = SaveImageFile(model.EngineerPhoto1);
                }
               
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_Engineers",
                        new
                        {
                            model.EngineerId,
                            model.EngineerCode,
                            EngineerPhotoFile=model.EngineerPhoto,
                            TrcId=model.Trc,
                            model.Department,
                            model.Designation,
                            model.EmployeeName,
                            model.EmpMobileNo,
                            model.EmpAltNo,
                            model.EmpEmailId,
                            model.EmpAddress,
                            model.EmpJoiningDate,
                            model.EmpPicUp,
                            model.BikeModel,
                            model.BikeNumber,
                            model.IsActive,
                            User="",
                            Action="add"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 0)
                    {
                        TempData["Message"] = "Employee Code Already Exist";

                    }
                    else
                    {
                        TempData["Message"] = "Successfully Added Engineer";
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("Me");
        }

        public ActionResult AddEngineertable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageEngineerModel>("select * from MstEngineer", null, commandType: CommandType.Text).ToList();
                return View(result);
            }
        }
        public ActionResult EditEngineer(int engineerId)
        {
            ViewBag.Trc = new SelectList(dropdown.BindTrc(), "Value", "Text");
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageEngineerModel>("select * from MstEngineer Where EngineerId=@EngineerId",
                    new
                    {
                        @EngineerId = engineerId
                    }, commandType: CommandType.Text).FirstOrDefault();
                if (result !=null)
                {
                    result.Trc = result.TrcId.ToString();
                    //result.EmpJoiningDate = result.EmpJoiningDate.
                }

                return View(result);
            }
        }
        [HttpPost]
        public ActionResult EditEngineer(ManageEngineerModel model)
        {
            try
            {
                if (model.EngineerPhoto1 != null)
                {
                    model.EngineerPhoto = SaveImageFile(model.EngineerPhoto1);
                }

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_Engineers",
                        new
                        {
                            model.EngineerId,
                            model.EngineerCode,
                            EngineerPhotoFile = model.EngineerPhoto,
                            TrcId = model.Trc,
                            model.Department,
                            model.Designation,
                            model.EmployeeName,
                            model.EmpMobileNo,
                            model.EmpAltNo,
                            model.EmpEmailId,
                            model.EmpAddress,
                            model.EmpJoiningDate,
                            model.EmpPicUp,
                            model.BikeModel,
                            model.BikeNumber,
                            model.IsActive,
                            User = "",
                            Action = "edit"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["Message"] = "Updated Successfully";

                    }
                    else
                    {
                        TempData["Message"] = "Something Went Wrong";
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("Me");
        }
    }
}