using doorserve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Controllers
{
    public class BaseController : Controller
    {
        public SessionModel CurrentUser
        {
            get
            {
                if (Session["User"] != null)
                {
                    return (SessionModel)Session["User"];
                }
                return new SessionModel();

            }


        }
    }
}