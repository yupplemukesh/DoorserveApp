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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Gst Category")]
        public ActionResult Gst()
        {
            ManageGSTViewModel Mgc = new ManageGSTViewModel();
            Mgc.Rights = (UserActionRights)HttpContext.Items["ActionsRights"];
            if (TempData["Message"] != null)
            {
                Mgc.Message = TempData["Message"].ToString();
                
            }
            return View(Mgc);
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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Gst Category")]
        public ActionResult GstTable()
        {
            GstCategoryModel objGstCategoryModel = new GstCategoryModel();
            using (var con = new SqlConnection(_connectionString))
            {
                objGstCategoryModel._GstCategoryModelList = con.Query<GstCategoryModel>("Select mst.Id,mst.GSTCATEGORYID,MST.GSTCATEGORY,MST.ISACTIVE,MST.COMMENTS,MST.CREATEDDATE,MST.MODIFYDATE,u.UserName CRBY, u1.Username MODBY from MstGstCategory mst join create_User_Master u on u.Id = mst.CreatedBy left outer join create_user_master u1 on mst.ModifyBy = u1.id", new { }, commandType: CommandType.Text).ToList();
                
            }
            objGstCategoryModel._UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];

            return View(objGstCategoryModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Gst Category")]
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