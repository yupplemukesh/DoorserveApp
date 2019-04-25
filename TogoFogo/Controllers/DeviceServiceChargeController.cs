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
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class DeviceServiceChargeController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        private SessionModel user;

        // GET: DeviceServiceCharge
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Device Service Charge")]
        public ActionResult ServiceCharge()
        {
          
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create}, "Device Service Charge")]
        public ActionResult AddServiceCharge()
        {
            user = Session["User"] as SessionModel;
            ServiceChargeModel scm = new ServiceChargeModel();
            scm.DeviceCategoryList = new SelectList(dropdown.BindCategory(user.CompanyId), "Value", "Text");
            scm.DeviceSubCategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
            scm.BrandList = new SelectList(dropdown.BindBrand(user.CompanyId), "Value", "Text");
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
                        var response = new ResponseModel { IsSuccess = false };
                        if (result == 1)
                        {
                            response.IsSuccess = true;
                            response.Response = "Submitted Successfully";
                            TempData["response"] = response;

                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Response = "Something Went Wrong";
                            TempData["response"] = response;
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


        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Device Service Charge")]
        public ActionResult ServiceChargeTable()
        {
            List<ServiceChargeModel> objServiceChargeModel = new List<ServiceChargeModel>();
            using (var con = new SqlConnection(_connectionString))
            {
                objServiceChargeModel = con.Query<ServiceChargeModel>("Get_ServiceCharge", new { }, commandType: CommandType.StoredProcedure).ToList();
                
            }
            return View(objServiceChargeModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Device Service Charge")]
        public ActionResult EditServiceCharge(int? ServiceChargeId)
        {
           
            using (var con = new SqlConnection(_connectionString))
            {
                user = Session["User"] as SessionModel;
                var result = con.Query<ServiceChargeModel>("SELECT * from MstDeviceServiceCharge Where ServiceChargeId=@serviceChargeId", new { @serviceChargeId = ServiceChargeId },
                commandType: CommandType.Text).FirstOrDefault();               
                result.DeviceCategoryList = new SelectList(dropdown.BindCategory(user.CompanyId), "Value", "Text");
                result.DeviceSubCategoryList = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
                result.BrandList = new SelectList(dropdown.BindBrand(user.CompanyId), "Value", "Text");
                result.ModelNameList = new SelectList(dropdown.BindProduct(user.CompanyId), "Value", "Text");
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
                        var response = new ResponseModel { IsSuccess = false };
                        if (result == 2)
                        {
                            response.IsSuccess = true;
                            response.Response = "Submitted Successfully";
                            TempData["response"] = response;

                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Response = "Something Went Wrong";
                            TempData["response"] = response;
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
 
 
 