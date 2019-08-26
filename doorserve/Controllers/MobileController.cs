using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using doorserve.Models;
using System.Data;
using System.Web.UI.WebControls;

namespace doorserve.Controllers
{
    public class MobileController : Controller
    {
        private readonly string _connectionString =
           ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private readonly string _connectionString1 =
           ConfigurationManager.ConnectionStrings["RajConnection"].ConnectionString;
        public ActionResult GetCity()
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var city = con.Query<BindAPI>("select distinct LocationId,LocationName from MstLocation ", commandType: CommandType.Text);
                    List<ListItem> items = new List<ListItem>();
                    foreach (var val in city)
                    {
                        items.Add(new ListItem
                        {
                            Value = val.LocationId.ToString(), //Value Field(ID)
                            Text = val.LocationName //Text Field(Name)
                        });
                    }

                    return Json(items, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetBrand()
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var brand = con
                        .Query<BindAPI>("SELECT DISTINCT BrandId, BrandName FROM MstBrand  where BrandId IS NOT NULL ORDER BY BrandName", null, commandType: CommandType.Text).ToList();
                    List<ListItem> items = new List<ListItem>();

                    foreach (var val in brand)
                    {
                        items.Add(new ListItem
                        {
                            Value = val.BrandId, //Value Field(ID)
                            Text = val.BrandName //Text Field(Name)
                        });
                    }
                    return Json(items, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetModel(int? id)
        {
            if (id == null || id == 0)
            {
                return Json("Provide Brand id", JsonRequestBehavior.AllowGet);
            }
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var modl = con.Query<BindAPI>(
                             "select DISTINCT ProductId,ProductName  from MstProduct Where Brand_Id=@BrandId", new { @BrandId = id }, commandType: CommandType.Text).ToList();
                    List<ListItem> items = new List<ListItem>();
                    foreach (var val in modl)
                    {
                        items.Add(new ListItem
                        {
                            Value = val.ProductId.ToString(), //Value Field(ID)
                            Text = val.ProductName //Text Field(Name)
                        });
                    }
                    return Json(items, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetColor(int? id)
        {
            if (id == null || id == 0)
            {
                return Json("Provide Model id", JsonRequestBehavior.AllowGet);
            }
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var modl = con.Query<BindAPI>("GetModelColor", new { id = id }, commandType: CommandType.StoredProcedure);
                    List<ListItem> items = new List<ListItem>();
                    foreach (var val in modl)
                    {
                        items.Add(new ListItem
                        {
                            Value = val.ColorId.ToString(), //Value Field(ID)
                            Text = val.ColorName //Text Field(Name)
                        });
                    }
                    return Json(items, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetProblemReported()
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var problem = con.Query<BindAPI>("GetProblemObserved", commandType: CommandType.StoredProcedure);
                    List<ListItem> items = new List<ListItem>();
                    foreach (var val in problem)
                    {
                        items.Add(new ListItem
                        {
                            Value = val.ProblemId.ToString(), //Value Field(ID)
                            Text = val.Problem //Text Field(Name)
                        });
                    }
                    return Json(items, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SubmitData(string name, string mobile, string email, string address, string brand, string model, int? problem, DateTime? pickdate)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<string>("Insert_Repair_Request", new
                    {
                        Customer_Name = name,
                        Mobile_No = mobile,
                        Email_Id = email,
                        Pincode = "0",
                        Cust_State = "",
                        Cust_City = "",
                        Cust_Add = address,
                        Brand = brand,
                        Model = model,
                        Prob_Reported = problem,
                        Device_Type = "1",
                        PickupDate = pickdate

                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return Json(new { success = true, CCNo = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(new { success = false, message = "Please Check Data (" + ee.Message + ")" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetTableDataAFSLABD2()
        {
            try
            {
                using (var con = new SqlConnection(_connectionString1))
                {
                    var TableData = con.Query<RajModel>("select * from Mobile_App_Data ", commandType: CommandType.Text);
               

                    return Json(TableData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}