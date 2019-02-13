using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Extension;
using TogoFogo.Models;

namespace TogoFogo.Controllers
{
    public class ManagePinZIPCodeController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // GET: ManagePinZIPCode
      [CustomAuthorize]
        public ActionResult PinZIPCode()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<CourierPinZipCode>("GetDataFrm_MstCourierPINzip", null, commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }
        }
        public ActionResult Create()
        {
            CourierPinZipCode courierPinZipCode = new CourierPinZipCode();
            courierPinZipCode.CourierList = new SelectList(dropdown.BindCourier(), "Value", "Text");
            courierPinZipCode.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            courierPinZipCode.StateList = new SelectList(Enumerable.Empty<SelectListItem>());
            courierPinZipCode.CityList = new SelectList(Enumerable.Empty<SelectListItem>());
            return View(courierPinZipCode);
        }

        [HttpPost]
        public ActionResult Create(CourierPinZipCode model)
        {
            try
            {
              
                using (var con = new SqlConnection(_connectionString))
                {
                    if (ModelState.IsValid)
                    {
                        var result = con.Query<int>("Add_Edit_Delete_CourierPinZipCode",
                        new
                        {
                            model.Pin_ZIP_ID,
                            CountryID = model.Country,
                            CourierID = model.Courier,
                            Pin_CountryID = model.PIN_Country1,
                            Pin_State = model.PIN_State1,
                            Pin_City = model.PIN_City1,
                            model.Pin_Region,
                            model.Pin_Zone,
                            model.Pin_Code,
                            model.Pin_TAT,
                            model.Pin_Cod,
                            model.ShortCode,
                            model.ISExpress,
                            model.ReverseLogistics,
                            model.OrderPreference,
                            model.IsActive,
                            model.Comments,
                            model.CreatedDate,
                            model.ModifyBy,
                            model.ModifyDate,
                            model.DeleteBy,
                            model.DeleteDate,
                            User = "",
                            Action = "add"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 1)
                        {
                            TempData["Message"] = "Successfully Added";
                        }
                        else
                        {
                            TempData["Message"] = "Pin Region Already Exist";
                        }
                    }
                    else
                    {                      
                            return View(model);                       
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("PinZIPCode");
        }

        public ActionResult Edit(int pinZipId)
        {
            DropdownBindController dropdownBindController=new DropdownBindController();
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<CourierPinZipCode>("select * from MstCourierPINzip where pin_Zip_ID=@pin_Zip_ID",
                    new { @pin_Zip_ID = pinZipId }
                    , commandType: CommandType.Text).FirstOrDefault();
                if (result != null)
                {
                    result.Country = result.CountryID.ToString();
                    result.Courier = result.CourierID.ToString();
                    result.PIN_Country1 = result.Pin_CountryID.ToString();
                    result.CountryList= new SelectList(dropdown.BindCountry(), "Value", "Text");
                    result.CourierList= new SelectList(dropdown.BindCourier(), "Value", "Text");
                    result.StateList = new SelectList(dropdownBindController.BindState(result.Pin_CountryID),"Value","Text");
                    result.CityList = new SelectList(dropdownBindController.BindLocation(result.Pin_State), "Value", "Text");
                    result.PIN_State1 = result.Pin_State.ToString();
                    result.PIN_City1 = result.Pin_City.ToString();
                }
                return View(result);
            }
        }

        [HttpPost]
        public ActionResult Edit(CourierPinZipCode model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    if (ModelState.IsValid)
                    {
                        var result = con.Query<int>("Add_Edit_Delete_CourierPinZipCode",
                            new
                            {
                                model.Pin_ZIP_ID,
                                CountryID = model.Country,
                                CourierID = model.Courier,
                                Pin_CountryID = model.PIN_Country1,
                                Pin_State = model.PIN_State1,
                                Pin_City = model.PIN_City1,
                                model.Pin_Region,
                                model.Pin_Zone,
                                model.Pin_Code,
                                model.Pin_TAT,
                                model.Pin_Cod,
                                model.ShortCode,
                                model.ISExpress,
                                model.ReverseLogistics,
                                model.OrderPreference,
                                model.IsActive,
                                model.Comments,
                                model.CreatedDate,
                                model.ModifyBy,
                                model.ModifyDate,
                                model.DeleteBy,
                                model.DeleteDate,
                                User = "",
                                Action = "edit"
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 2)
                        {
                            TempData["Message"] = "Updated Successfully";
                        }
                        else
                        {
                            TempData["Message"] = "Something went wrong";
                        }
                    }
                    else
                    {
                        return View(model);
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("PinZIPCode");
        }
        [HttpGet]
        public ActionResult Mass_Courier_UPload()
        {
            ViewBag.CountryBulk = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ViewBag.CourierBulk = new SelectList(dropdown.BindCourier(), "Value", "Text");
            ViewBag.PIN_CountryBulk = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ViewBag.PIN_StateBulk = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PIN_CityBulk = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }
        [HttpPost]
        public ActionResult Mass_Courier_UPload(Mass_Upload_Model m)
        {
            string filePath = string.Empty;
            if (m.postedFile != null)
            {
                string path = Server.MapPath("~/UploadExcel/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(m.postedFile.FileName);
                string extension = Path.GetExtension(m.postedFile.FileName);
                m.postedFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            //cmdExcel.CommandText = "SELECT *, "+m.Country+ "as CountryID, "+m.Courier+ "as CourierID,"+m.PIN_Country1+ "as Pin_CountryID ,"+m.PIN_State1+ "as Pin_State,"+m.PIN_City1+ "as Pin_City From [" + sheetName + "]";
                            cmdExcel.CommandText = "SELECT *, '" + m.CountryBulk + "'as CountryID, '" + m.CourierBulk + "'as CourierID,'" + m.PIN_CountryBulk + "'as Pin_CountryID ,'" + m.PIN_StateBulk + "'as Pin_State,'" + m.PIN_CityBulk + "'as Pin_City From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }               
                using (var con = new SqlConnection(_connectionString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.MstCourierPINzip";

                        //[OPTIONAL]: Map the Excel columns with that of the database table
                        sqlBulkCopy.ColumnMappings.Add("Pin_ZIP_ID", "Pin_ZIP_ID");
                        sqlBulkCopy.ColumnMappings.Add("CountryID", "CountryID");
                        sqlBulkCopy.ColumnMappings.Add("CourierID", "CourierID");
                        sqlBulkCopy.ColumnMappings.Add("Pin_CountryID", "Pin_CountryID");
                        sqlBulkCopy.ColumnMappings.Add("Pin_State", "Pin_State");
                        sqlBulkCopy.ColumnMappings.Add("Pin_City", "Pin_City");
                        sqlBulkCopy.ColumnMappings.Add("Pin_Region", "Pin_Region");
                        sqlBulkCopy.ColumnMappings.Add("Pin_Zone", "Pin_Zone");
                        sqlBulkCopy.ColumnMappings.Add("Pin_Code", "Pin_Code");
                        sqlBulkCopy.ColumnMappings.Add("Pin_TAT", "Pin_TAT");
                        sqlBulkCopy.ColumnMappings.Add("Pin_Cod", "Pin_Cod");
                        sqlBulkCopy.ColumnMappings.Add("ShortCode", "ShortCode");
                        sqlBulkCopy.ColumnMappings.Add("ISExpress", "ISExpress");
                        sqlBulkCopy.ColumnMappings.Add("ReverseLogistics", "ReverseLogistics");
                        sqlBulkCopy.ColumnMappings.Add("OrderPreference", "OrderPreference");
                        sqlBulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                        sqlBulkCopy.ColumnMappings.Add("Comments", "Comments");
                        sqlBulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                        sqlBulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                        sqlBulkCopy.ColumnMappings.Add("ModifyBy", "ModifyBy");
                        sqlBulkCopy.ColumnMappings.Add("ModifyDate", "ModifyDate");
                        sqlBulkCopy.ColumnMappings.Add("DeleteBy", "DeleteBy");
                        sqlBulkCopy.ColumnMappings.Add("DeleteDate", "DeleteDate");
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }

            return RedirectToAction("PinZIPCode","ManagePinZIPCode");
        }
 
    }
}