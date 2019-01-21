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
    public class AbhayController: Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ActionResult WebsiteCallLogData()
        {
            List<AllData> result = new List<AllData>();
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    result = con.Query<AllData>("GetTableDataForOOW", commandType: CommandType.StoredProcedure).ToList();
                }                    
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}