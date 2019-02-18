﻿using System;
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
        public ActionResult AddUser(Int64 id)
        {
            User objUser = new User();
            Int64 UserId = id;
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<dynamic>("UspGetUserDetails", new { UserId },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

               
                   
                    objUser._AddressDetail = new AddressDetail();
                    objUser._ContactPerson = new ContactPersonModel();
                   //objUser.SerialNo = result.SerialNo;
                    objUser.UserId = result.UserId;
                    objUser.UserName = result.UserName;
                    objUser.IsActive = result.IsActive;
                    objUser.CreatedDate = result.CreatedDate;
                    objUser.ModifyDate = result.ModifyDate;
                    objUser._AddressDetail.PinNumber = result.PinNumber;
                    objUser._AddressDetail.Address = result.Address;
                    objUser._AddressDetail.AddressTypeId = result.AddressTypeId;
                    objUser._AddressDetail.CityId = result.CityId;
                    objUser._AddressDetail.StateId = result.StateId;
                    objUser._AddressDetail.City = result.City;
                    objUser._AddressDetail.State = result.State;
                    objUser._ContactPerson.ConFirstName = result.ConFirstName;
                    objUser._ContactPerson.ConMobileNumber = result.ConMobileNumber;
                    objUser._ContactPerson.ConEmailAddress = result.ConEmailAddress;

                    
                
               
            }
            return View(objUser);
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
        public ActionResult UserList()
        {
            int UserId = 0;
            List<User> objUserList = new List<User>();
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<dynamic>("UspGetUserDetails", new { UserId },
                    commandType: CommandType.StoredProcedure).ToList();

                foreach(var item in result)
                {
                    User objUser = new User();
                    objUser._AddressDetail = new AddressDetail();
                    objUser._ContactPerson = new ContactPersonModel();
                    objUser.SerialNo = item.SerialNo;
                    objUser.UserId = item.UserId;
                    objUser.UserName = item.UserName;
                    objUser.IsActive = item.IsActive;
                    objUser.CreatedDate = item.CreatedDate;
                    objUser.ModifyDate = item.ModifyDate;
                    objUser._AddressDetail.PinNumber = item.PinNumber;
                    objUser._AddressDetail.Address = item.Address;
                    objUser._AddressDetail.AddressTypeId = item.AddressTypeId;
                    objUser._AddressDetail.CityId = item.CityId;
                    objUser._AddressDetail.StateId = item.StateId;
                    objUser._AddressDetail.City = item.City;
                    objUser._AddressDetail.State = item.State;
                    objUser._ContactPerson.ConFirstName = item.ConFirstName;
                    objUser._ContactPerson.ConMobileNumber = item.ConMobileNumber;
                    objUser._ContactPerson.ConEmailAddress = item.ConEmailAddress;

                    objUserList.Add(objUser);
                }
                return View(objUserList);
            }
            
        }
    }
}