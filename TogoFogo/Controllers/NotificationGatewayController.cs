using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Models.Gateway;
using TogoFogo.Repository;
using TogoFogo.Repository.SMSGateway;
using AutoMapper;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class NotificationGatewayController : Controller
    {

        private readonly IGateway _gatewayRepo;
          public NotificationGatewayController()

        {
             _gatewayRepo = new Gateway();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Notification Gateway")]
        public async Task<ActionResult> Index()


        {
            var getwaylist = await CommonModel.GetGatewayType();


            var getwayTypeId = getwaylist.Where(x => x.Text == "Notification Gateway").Select(x => x.Value).SingleOrDefault();

            var notificationgateway = await _gatewayRepo.GetGatewayByType(getwayTypeId);

            NotificationGateWayMainModel model = new NotificationGateWayMainModel();
            model.Gateway = new NotificationGatewayModel();
            model.mainModel = Mapper.Map<List<NotificationGatewayModel>>(notificationgateway);
            model.Gateway.GatewayTypeId = getwayTypeId;
            model.Gateway.GatewayList = new SelectList(notificationgateway, "GatewayId", "GatewayName");
           // model.Rights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(model);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Notification Gateway")]
        public async Task<ActionResult> Create()
        {
            var notificationgatewaymodel = new NotificationGatewayModel();
            return View(notificationgatewaymodel);
        }
        [HttpPost]
        public async Task<ActionResult> Create(NotificationGatewayModel notificationgateway)
        {
            if (ModelState.IsValid)
            {
                var gatewayModel = new GatewayModel
                {
                    GatewayId = notificationgateway.GatewayId,
                    GatewayTypeId = notificationgateway.GatewayTypeId,
                    GatewayName = notificationgateway.GatewayName,
                    IsActive = notificationgateway.IsActive,
                    SenderID = notificationgateway.SenderID,
                    GoogleApikey = notificationgateway.GoogleApikey,
                    GoogleApiURL = notificationgateway.GoogleApiUrl,
                    GoogleProjectID = notificationgateway.GoogleProjectID,
                    GoogleProjectName = notificationgateway.GoogleProjectName,
                    AddeddBy = Convert.ToInt32(Session["User_ID"])
                };
                ResponseModel response = new ResponseModel();
                if (gatewayModel.GatewayId != 0)
                    response = await _gatewayRepo.AddUpdateDeleteGateway(gatewayModel, 'U');
                else
                    response = await _gatewayRepo.AddUpdateDeleteGateway(gatewayModel, 'I');
                _gatewayRepo.Save();
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else
                return View(notificationgateway);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Notification Gateway")]
        public async Task<ActionResult> Edit(int id)
        {
            var notificationgateway = await _gatewayRepo.GetGatewayById(id);
            return Json(notificationgateway, JsonRequestBehavior.AllowGet);
        }

    }

}