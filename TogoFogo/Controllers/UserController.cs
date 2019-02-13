using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
namespace TogoFogo.Controllers
{
    public class UserController : Controller
    {
        
        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(User objUser)
        {
            return View();
        }

    }
}