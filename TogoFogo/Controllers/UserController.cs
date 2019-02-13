using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using Dapper;
using System.Data;
using System.Reflection;

namespace TogoFogo.Controllers
{
    public class UserController : Controller
    {
        private readonly string _connectionString =
     ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(User objUser)
        {
            objUser.UserLoginId = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));
            Random r = new Random();
            int randomNumber = r.Next(999, 10000);
            string Password = randomNumber.ToString();           
            var mpc = new Email_send_code();
            Type type = mpc.GetType();
            var Status = (int)type.InvokeMember("sendmail_update",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, mpc,
                                    new object[] { objUser._ContactPerson.ConEmailAddress, Password, objUser.UserName });
            if (Status == 1)
            {
                objUser.Password = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(Password, true);
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("UspInsertUser",
                        new
                        {
                            objUser.UserId,
                            objUser.UserName,
                            objUser.Password,
                            objUser._ContactPerson.ConFirstName,
                            objUser._ContactPerson.ConMobileNumber,
                            objUser._ContactPerson.ConEmailAddress,
                            objUser._AddressDetail.AddressTypeId,
                            objUser._AddressDetail.Address,
                            objUser._AddressDetail.CityId,
                            objUser._AddressDetail.StateId,
                            objUser._AddressDetail.PinNumber,
                            objUser.IsActive,
                            objUser.UserLoginId

                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 0)
                    {
                        TempData["Message"] = "Something went wrong";

                    }
                    else if (result == 1)
                    {
                        TempData["Message"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["Message"] = "Successfully Updated";
                    }
                }
            }
            return View();
        }

    }
}