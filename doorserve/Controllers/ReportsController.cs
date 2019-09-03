using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using doorserve.Models;

namespace doorserve.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly string _connectionString =
          ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        // GET: Reports
        public ActionResult RepairRequestReport()
        {
            using (var con = new SqlConnection(_connectionString))
            {

                ViewBag.TotalCount = con.Query<int>("select Count(*) from Repair_Request_Details", null, commandType: CommandType.Text).FirstOrDefault();

                return View();
            }
        }
        public ActionResult FilterOfRepairReport()
        {


            ViewBag.TrcName = new SelectList(dropdown.BindTrc(), "Value", "Text");
            ViewBag.ServiceProviderName = new SelectList(dropdown.BindServiceProvider(CurrentUser.CompanyId), "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult FilterOfRepairReport(ReportsModel m)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = new List<ReportsModelTable>();
                
                if (m.Fromdate!=null && m.ToDate != null && m.ProcessName != null && m.ServiceProviderName != null && m.TrcName != null)
                {
                    result= con.Query<ReportsModelTable>("Repair_Request_report_Download_By_AllFilters",
                        new { m.Fromdate, m.ToDate,m.ProcessName, ServiceProvider=m.ServiceProviderName, TrcId=m.TrcName }, commandType: CommandType.StoredProcedure).ToList();
                    foreach (var item in result)
                    {
                        var finalvalue = "";
                        var Problem = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",
                       new { item.CC_NO }, commandType: CommandType.StoredProcedure).ToList();

                        foreach (var item1 in Problem)
                        {
                            var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item1.ProblemId }, commandType: CommandType.Text).FirstOrDefault();

                            //finalValue = string.Join(",", result1);
                            finalvalue = finalvalue + "," + result1;

                        }
                        finalvalue = finalvalue.Trim().TrimStart(',');
                        item.ProblemReported = finalvalue;

                    }
                    TempData["Report"] = result;
                }
                else 
                {
                    result = con.Query<ReportsModelTable>("Repair_Request_report_Download_By_Date", new { m.Fromdate, m.ToDate }, commandType: CommandType.StoredProcedure).ToList();
                    foreach (var item in result)
                    {
                        var finalvalue = "";
                        var Problem = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",
                       new { item.CC_NO }, commandType: CommandType.StoredProcedure).ToList();

                        foreach (var item1 in Problem)
                        {
                            var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item1.ProblemId }, commandType: CommandType.Text).FirstOrDefault();

                            //finalValue = string.Join(",", result1);
                            finalvalue = finalvalue + "," + result1;

                        }
                        finalvalue = finalvalue.Trim().TrimStart(',');
                        item.ProblemReported = finalvalue;

                    }
                    TempData["Report"] = result;
                }
               

                ViewBag.TotalCount = con.Query<int>("select Count(CC_NO) from Repair_Request_Details", null, commandType: CommandType.Text).FirstOrDefault();             

                return PartialView("TableRepair",result);
            }
                
        }
        public ActionResult TableRepair()
        {
            return View(new List<ReportsModelTable>());
        }
        public ActionResult DownloadReport()
        {
            List<ReportsModelTable> ReportResult = new List<ReportsModelTable>();
            if (TempData.ContainsKey("Report"))
                ReportResult = TempData["Report"] as List<ReportsModelTable>;
            var gv = new GridView();
            gv.DataSource = ReportResult;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View();
        }

        public ActionResult LandingPage()
        {
            return View();
        }
        public ActionResult LandingPageSearch()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LandingPageSearch(LandingPageModel m)
        {
            
            using (var con = new SqlConnection(_connectionString))
            {
                string FromDate = m.FromDate.ToString("yyyy-MM-dd");
                string ToDate = m.ToDate.ToString("yyyy-MM-dd");
                var result = con.Query<LandingPageExcelModel>("GetDataOfLandingPage", new { FromDate, ToDate }, commandType: CommandType.StoredProcedure).ToList();
                var gv = new GridView();
                gv.DataSource = result;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=LandingPage.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
                return View();
            }               
        }
        public ActionResult WebsiteUser()
        {
            return View();
        }
        public ActionResult WebsiteUserStatus()
        {
            var result = new List<WebsiteUserModel>();
            using (var con = new SqlConnection(_connectionString))
            {
                 result = con.Query<WebsiteUserModel>("Select * from Repair_request_Details", new { }, commandType: CommandType.Text).ToList();
               
            }
            return View(result);
        }
        public JsonResult UpdateCustomerStatus(string Status,int Id)
        {
            string result = "";
            using (var con = new SqlConnection(_connectionString))
            {
                result = con.Query<string>("UpdateWebsiteUserStatus", new { Id,Status }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult DownloadReportUser()
        {

            using (var con = new SqlConnection(_connectionString))
            {
                
                var result = con.Query<WebsiteUserModelReport>("Select * from Repair_Request_Details", new {}, commandType: CommandType.Text).ToList();
                var gv = new GridView();
                gv.DataSource = result;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=LandingPage.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
                return View();
            }
        }
    }
}