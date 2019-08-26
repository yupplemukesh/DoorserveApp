using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using doorserve.Models;
using doorserve.Permission;
using doorserve.Filters;

namespace doorserve.Controllers
{
    public class ManageCourierApiController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        // GET: ManageCourierApi
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Courier_API)]
        public ActionResult ManageCourierApi()
        {          
            using (var con = new SqlConnection(_connectionString))
            {
                var SessionModel = Session["User"] as SessionModel;
                var result = con.Query<ManageCourierApiModel>("GetCourierAPIDetails", new { SessionModel.CompanyId, }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }           
           
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Courier_API)]
        public ActionResult Create()
        {
            var SessionModel = Session["User"] as SessionModel;
            ManageCourierApiModel courierApiModel = new ManageCourierApiModel
            {
                CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text"),
                CourierList = new SelectList(dropdown.BindCourier(SessionModel.CompanyId), "Value", "Text")
            };
            return View(courierApiModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Courier_API)]
        [HttpPost]
        [ValidateModel]
        public ActionResult Create(ManageCourierApiModel model)
        {
            var SessionModel = Session["User"] as SessionModel;
            using (var con = new SqlConnection(_connectionString))
                {
        
                // ViewBag.Country = new SelectList(dropdown.BindCountry(), "Value", "Text");
                // ViewBag.Courier = new SelectList(dropdown.BindCourier(), "Value", "Text");/
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
                            User = SessionModel.UserId,
                            SessionModel.CompanyId,
                            Action = "I",
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                            var response = new ResponseModel();
                            if (result == 1)
                        {
                                response.IsSuccess = true;
                                response.Response = " Successfully Added ";
                                TempData["response"] = response;                            
                        }
                        else
                        {       response.IsSuccess = true;
                                response.Response = " Username Already Exist ";
                                TempData["response"] = response;                               
                        }            
           }
                    return RedirectToAction("ManageCourierApi");
        }
               
     
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Courier_API)]
        public ActionResult Edit(int apiId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
    
                var result = con.Query<ManageCourierApiModel>("Select * from MstCourierAPI Where API_ID=@API_ID", new { @API_ID = apiId }
                    , commandType: CommandType.Text).FirstOrDefault();
                if (result != null)
                {
                    var SessionModel = Session["User"] as SessionModel;
                    result.Country = result.CountryID.ToString();
                    result.Courier = result.CourierID.ToString();
                    result.CountryList= new SelectList(dropdown.BindCountry(), "Value", "Text");
                    result.CourierList= new SelectList(dropdown.BindCourier(SessionModel.CompanyId), "Value", "Text");
                    ViewBag.CourierImage = "http://crm.doorserve.com/UploadedImages/" + result.CourierImage;
                }


                return View(result);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Courier_API)]
        [HttpPost]
        [ValidateModel]
        public ActionResult Edit(ManageCourierApiModel model)
        {
            var SessionModel = Session["User"] as SessionModel;
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
                            User = SessionModel.UserId,
                            SessionModel.CompanyId,
                            Action = "U",
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var response = new ResponseModel();
                        if (result == 2)
                        {                            
                            response.IsSuccess = true;
                            response.Response = "Updated Successfully ";
                            TempData["response"] = response;
                        }
                        else
                        {
                            response.IsSuccess = true;
                            response.Response = "Not Updated";
                            TempData["response"] = response;                           
                        }
              }
                    // return View(model); 
                return RedirectToAction("ManageCourierApi");
        }
    }
            
}
  