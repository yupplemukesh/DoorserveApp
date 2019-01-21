using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;

namespace TogoFogo.Controllers
{
    public class Trc_PFRMAController : Controller
    {
        private readonly string _connectionString =
           ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // GET: Trc_PFRMA
        public ActionResult Index()
        {
            ViewBag.RecvdBrand = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.RecvdModel = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.EnggName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ReceivedDevice = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }
        public ActionResult FindPFRMA()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindPFRMA(string CcNO)
        {
            new AllData();
            var finalValue = "";
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetDataByCCNO", new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (result.ChildtableDataProblem == null)
                {
                    var Problem = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem", new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).ToList();
                    result.ChildtableDataProblem = Problem;

                    foreach (var item in result.ChildtableDataProblem)
                    {
                        var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item.ProblemId }, commandType: CommandType.Text).FirstOrDefault();

                        finalValue = finalValue + " , " + result1;
                    }
                    finalValue = finalValue.Trim().TrimStart(',');
                }

                result.Problem = finalValue;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PFRMAForm()
        {
            ViewBag.ReceivedDevice = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ViewBag.RecvdBrand = new SelectList(dropdown.BindBrand(), "Value", "Text");
            ViewBag.RecvdModel = new SelectList(dropdown.BindProduct(),"Value","Text");
            ViewBag.EnggName = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult PFRMAForm(ReceiveMaterials m)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("InsertDatainPFRMA",
                    new
                    {
                        m.CC_NO,
                        m.ApproverRemarks,
                        m.IsApproveforReceivingDevice
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 2)
                {
                    TempData["Message"] = "Successfully Updated";
                }
                
                else
                {
                    TempData["Message"] = "Something Went Wrong";
                }
                return RedirectToAction("Index");
            }
           
        }
        public ActionResult TablePFRMA()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForRM_Approve",
                   new{}, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
               
        }
    }
}