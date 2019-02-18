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
    public class UserPermissionController : Controller
    {
        private readonly string _connectionString =
        ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ActionResult AddUserPermission()
        {
            UserPermission objUserPermission = new UserPermission();
            Int64 UserId = 0;
            Int64 RoleId = 0;
            using (var con = new SqlConnection(_connectionString))
            {
                objUserPermission.UserList = con.Query<User>("UspGetUserDetails", new { UserId },
                    commandType: CommandType.StoredProcedure).ToList();
            }
            using (var con = new SqlConnection(_connectionString))
            {
                objUserPermission.UserRoleList = con.Query<UserRole>("UspGetUserRoleDetail", new { RoleId },
                    commandType: CommandType.StoredProcedure).ToList();
            }

            return View(objUserPermission);
        }     
    }
}