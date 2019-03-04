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
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class GstCategoryController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // GET: GstCategory
        public ActionResult Gst()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                
            }
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Gst Category")]
        public ActionResult AddGst()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGst(GstCategoryModel model)
        {
            
            try
            {
                if (ModelState.IsValid)
                { 
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_GstCategory",
                        new
                        {
                            model.GstCategoryId,
                            model.GSTCategory,
                            model.IsActive,
                            model.Comments,
                            User=Convert.ToInt32(Session["User_ID"]),
                            /*model.CreatedBy,
                           model.CreatedDate,
                           model.ModifyBy,
                            model.ModifyDate,*/
                            ACTION = 'I'
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {
                        TempData["Message"] = "Gst Category Successfully Added " ;

                    }
                    else
                    {
                            TempData["Message"] = "Gst Category Already Exist";
                            
                        }
                        
                    }
                    //return View(model);
                    return RedirectToAction("Gst");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("Gst");
            
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Gst Category")]
        public ActionResult GstTable()
        {
            GstCategoryModel objGstCategoryModel = new GstCategoryModel();
            using (var con = new SqlConnection(_connectionString))
            {
                objGstCategoryModel._GstCategoryModelList = con.Query<GstCategoryModel>("Select mst.Id,mst.GSTCATEGORYID,MST.GSTCATEGORY,MST.ISACTIVE,MST.COMMENTS,MST.CREATEDDATE,MST.MODIFYDATE,u.UserName CRBY, u1.Username MODBY from MstGstCategory mst join create_User_Master u on u.Id = mst.CreatedBy left outer join create_user_master u1 on mst.ModifyBy = u1.id", new { }, commandType: CommandType.Text).ToList();
                
            }
            UserActionRights objUserActiobRight = new UserActionRights();
            objGstCategoryModel._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objGstCategoryModel._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objGstCategoryModel._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objGstCategoryModel._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objGstCategoryModel._UserActionRights.Delete = true;
                    }
                }
            }
            else
            {

                objGstCategoryModel._UserActionRights.Create = true;
                objGstCategoryModel._UserActionRights.Edit = true;
                objGstCategoryModel._UserActionRights.Delete = true;
                objGstCategoryModel._UserActionRights.View = true;
                objGstCategoryModel._UserActionRights.History = true;
                objGstCategoryModel._UserActionRights.ExcelExport = true;

            }
            return View(objGstCategoryModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Gst Category")]
        public ActionResult EditGst(int? GstCategoryId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                 var result = con.Query<GstCategoryModel>("Select * from MstGstCategory Where GstCategoryId=@GstCategoryId",
                 new { @GstCategoryId = GstCategoryId }, commandType: CommandType.Text).FirstOrDefault();                
                return View(result);
            }
        }
        [HttpPost]
        public ActionResult EditGst(GstCategoryModel model)
        {
           
                try
            {
                if (ModelState.IsValid)
                {
                    using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_GstCategory",
                        new
                        {
                            model.GstCategoryId,
                            model.GSTCategory,
                            model.IsActive,
                            model.Comments,
                            User = Convert.ToInt32(Session["User_ID"]),


                            ACTION = 'U'
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["Message"] = "Gst Category Updated Successfully";

                    }
                    else
                    {

                        TempData["Message"] = "Gst Category Not Updated";
                           
                    }
                }
                    //return View(model);
                    return RedirectToAction("Gst");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           

            return RedirectToAction("Gst");
       
    }
    }
}