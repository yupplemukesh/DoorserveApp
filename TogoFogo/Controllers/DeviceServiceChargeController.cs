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
            ViewBag.CategoryId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SubCatId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.BrandId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ModalNameId = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }

        public ActionResult AddServiceCharge()
        {
            ViewBag.CategoryId = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ViewBag.SubCatId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.BrandId = new SelectList(dropdown.BindBrand(), "Value", "Text");
            ViewBag.ModalNameId = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
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
                                ModelName=model.ModalNameId,
                                HSN=model.HSNCode,
                                model.SAC,
                                model.TRUPC,
                                model.Form,
                                model.MRP,
                                model.MarketPrice,
                                model.ServiceCharge,
                                model.IsActive,
                                model.User,
                                Action="add" 
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
        public ActionResult EditServiceCharge(int? serviceChargeId)
        {
            ViewBag.CategoryId = new SelectList(dropdown.BindCategory(), "Value", "Text");
           
            ViewBag.BrandId = new SelectList(dropdown.BindBrand(), "Value", "Text");
            ViewBag.ModalNameId = new SelectList(Enumerable.Empty<SelectListItem>());
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ServiceChargeModel>("SELECT * from MstDeviceServiceCharge Where ServiceChargeId=@serviceChargeId", new { @serviceChargeId = serviceChargeId },
                    commandType: CommandType.Text).FirstOrDefault();
                ViewBag.SubCatId = new SelectList(dropdown.BindSubCategory(result.CategoryId), "Value", "Text");
                //
                    ViewBag.ModalNameId = new SelectList(dropdown.BindProduct(result.BrandId), "Value", "Text");
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
                                model.User,
                                Action = "edit"
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