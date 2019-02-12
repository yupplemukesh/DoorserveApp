using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;

namespace TogoFogo.Controllers
{
    public class ClientDataController : Controller
    {
        // GET: ClientData
        //public ActionResult Index()
        //{

        //    return View();
        //}
        public async Task<ActionResult> Create()
        {
            var clientDate = new ClientDataModel();
            clientDate.ClientList = new SelectList(await CommonModel.GetClientData(), "Name", "Text");
            clientDate.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(), "Value", "Text");
            return View(clientDate);
        }
        [HttpPost]
        public async Task<ActionResult> Upload(ClientDataModel clientDataModel)
        {
            return View(clientDataModel);
        }
    }
}