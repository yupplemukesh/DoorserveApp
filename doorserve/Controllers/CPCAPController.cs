using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doorserve.Models;
using doorserve.Permission;

namespace doorserve.Controllers
{
    public class CPCAPController : Controller
    {
        // GET: CPCAP
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Confirm_Par_Com_Advance_Payment)]
        public ActionResult Index()
        {         
            return View();
        }
        public ActionResult FindCPCAP()
        {
            var alldata = new AllData();
            return PartialView(alldata);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Confirm_Par_Com_Advance_Payment)]
        public ActionResult CPCAPForm()
        {
            var rpcap = new RPCAPModel();
            return PartialView(rpcap);
        }
    }
}