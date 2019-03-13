using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class CPCAPController : Controller
    {
        // GET: CPCAP
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Confirm Par/Com Advance Payment")]
        public ActionResult Index()
        {
            var _UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(_UserActionRights);
        }
        public ActionResult FindCPCAP()
        {
            var alldata = new AllData();
            return PartialView(alldata);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Confirm Par/Com Advance Payment")]
        public ActionResult CPCAPForm()
        {
            var rpcap = new RPCAPModel();
            return PartialView(rpcap);
        }
    }
}