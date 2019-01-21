using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;

namespace TogoFogo.Controllers
{
    public class ManageCourierApiController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // GET: ManageCourierApi
        public ActionResult ManageCourierApi()
        {
            ViewBag.Country = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Courier = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }
        public ActionResult AddCourierApi()
        {
            ViewBag.Country = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ViewBag.Courier = new SelectList(dropdown.BindCourier(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult AddCourierApi(ManageCourierApiModel model)
        {
            try
            {
              
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_CourierApi",
                        new
                        {
                            model.API_ID,     
                            CourierID=model.Courier,  
                            CountryID=model.Country,  
                            model.Username ,   
                            model.Passwrd ,    
                            model.AppVersion  ,
                            model.AccountNo  , 
                            model.AccountPIN , 
                            model.AccountEntity ,
                            model.AccountCountryCode ,
                            model.IsLargePacket ,    
                            model.IsActive ,   
                            model.Comments ,   
                            model.CreatedDate ,
                            model.ModifyBy ,
                            model.ModifyDate, 
                            model.DeleteBy ,
                            model.DeleteDate ,
                            User="" ,
                            Action="add" ,
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 0)
                    {
                        TempData["Message"] = "Username Already Exist";

                    }
                    else
                    {
                        TempData["Message"] = "Successfully Added";
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("ManageCourierApi");
        }

        public ActionResult CourierAPITable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageCourierApiModel>("GetCourierAPIDetails", new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }

        }

        public ActionResult EditCourierApi(int apiId)
        {
            ViewBag.Country = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ViewBag.Courier = new SelectList(dropdown.BindCourier(), "Value", "Text");
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageCourierApiModel>("Select * from MstCourierAPI Where API_ID=@API_ID", new {@API_ID=apiId }
                    , commandType: CommandType.Text).FirstOrDefault();
                if (result != null)
                {
                    result.Country = result.CountryID.ToString();
                    result.Courier = result.CourierID.ToString();
                    ViewBag.CourierImage = "http://crm.togofogo.com/Uploaded Images/" + result.CourierImage;
                }

               
                return View(result);
            }
        }

        [HttpPost]
        public ActionResult EditCourierApi(ManageCourierApiModel model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_CourierApi",
                        new
                        {
                            model.API_ID,
                            CourierID = model.Courier,
                            CountryID = model.Country,
                            model.Username,
                            model.Passwrd,
                            model.AppVersion,
                            model.AccountNo,
                            model.AccountPIN,
                            model.AccountEntity,
                            model.AccountCountryCode,
                            model.IsLargePacket,
                            model.IsActive,
                            model.Comments,
                            model.CreatedDate,
                            model.ModifyBy,
                            model.ModifyDate,
                            model.DeleteBy,
                            model.DeleteDate,
                            User = "",
                            Action = "edit",
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["Message"] = "Updated Successfully";

                    }
                    else
                    {
                        TempData["Message"] = "Not Updated";
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("ManageCourierApi");
        }
    }
}