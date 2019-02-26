using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
namespace TogoFogo.Controllers
{
    public class DeviceServiceChargeController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        // GET: DeviceServiceCharge
        public ActionResult ServiceCharge()
        {
           if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }

        public ActionResult AddServiceCharge()
        {
            ServiceChargeModel scm = new ServiceChargeModel();
            scm.DeviceCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            scm.DeviceSubCategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
            scm.BrandList = new SelectList(dropdown.BindBrand(), "Value", "Text");
            scm.ModelNameList = new SelectList(Enumerable.Empty<SelectListItem>());           
            return View(scm);
        }
        [HttpPost]
        public ActionResult AddServiceCharge(ServiceChargeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var con = new SqlConnection(_connectionString))
                    {
                        var result = con.Query<int>("Add_Edit_Delete_ServiceCharge",
                            new
                            {
                                model.ServiceChargeId,
                                model.CategoryId,
                                model.SubCatId,
                                model.BrandId,
                                ModelName = model.ModalNameId,
                                HSN = model.HSNCode,
                                model.SAC,
                                model.TRUPC,
                                model.Form,
                                model.MRP,
                                model.MarketPrice,
                                model.ServiceCharge,
                                model.IsActive,
                                User = Convert.ToInt32(Session["User_ID"]),
                                Action = "I"
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 1)
                        {
                            TempData["Message"] = "Submitted Successfully";

                        }
                        else
                        {
                            TempData["Message"] = "Something Went Wrong";
                        }
                    }

                    return RedirectToAction("ServiceCharge");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("ServiceCharge");
        }

      

        public ActionResult ServiceChargeTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ServiceChargeModel>("Get_ServiceCharge", new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
        }
        public ActionResult EditServiceCharge(int? ServiceChargeId)
        {
           
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ServiceChargeModel>("SELECT * from MstDeviceServiceCharge Where ServiceChargeId=@serviceChargeId", new { @serviceChargeId = ServiceChargeId },
                commandType: CommandType.Text).FirstOrDefault();               
                result.DeviceCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
                result.DeviceSubCategoryList = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
                result.BrandList = new SelectList(dropdown.BindBrand(), "Value", "Text");
                result.ModelNameList = new SelectList(dropdown.BindProduct(), "Value", "Text");
                return View(result);
            }

           
        }
        [HttpPost]
        public ActionResult EditServiceCharge(ServiceChargeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var con = new SqlConnection(_connectionString))
                    {
                        var result = con.Query<int>("Add_Edit_Delete_ServiceCharge",
                            new
                            {
                                model.ServiceChargeId,
                                model.CategoryId,
                                model.SubCatId,
                                model.BrandId,
                                ModelName = model.ModalNameId,
                                HSN = model.HSNCode,
                                model.SAC,
                                model.TRUPC,
                                model.Form,
                                model.MRP,
                                model.MarketPrice,
                                model.ServiceCharge,
                                model.IsActive,
                                User=Convert.ToInt32(Session["User_ID"]),
                                Action = "U"
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 2)
                        {
                            TempData["Message"] = "Submitted Successfully";

                        }
                        else
                        {
                            TempData["Message"] = "Something Went Wrong";
                        }
                    }

                    return RedirectToAction("ServiceCharge");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("ServiceCharge");
        }                   
                 
       
    }
}
 
 
 