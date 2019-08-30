﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using doorserve.Models;
using doorserve.Models.Gateway;
using doorserve.Repository;
using doorserve.Repository.SMSGateway;
using AutoMapper;
using doorserve.Permission;

namespace doorserve.Controllers
{
    public class NotificationGatewayController : Controller
    {

        private readonly IGateway _gatewayRepo;
          public NotificationGatewayController()

        {
             _gatewayRepo = new Gateway();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Notification_Gateway)]
        public async Task<ActionResult> Index()


        {
            var getwaylist = await CommonModel.GetGatewayType();

            var SessionModel = Session["User"] as SessionModel;
            var getwayTypeId = getwaylist.Where(x => x.Text == "Notification Gateway").Select(x => x.Value).SingleOrDefault();

            var notificationgateway = await _gatewayRepo.GetGatewayByType(new Filters.FilterModel {GatewayTypeId=getwayTypeId,CompId=SessionModel.CompanyId });

            NotificationGateWayMainModel model = new NotificationGateWayMainModel();
            model.Gateway = new NotificationGatewayModel();
            model.mainModel = Mapper.Map<List<NotificationGatewayModel>>(notificationgateway);
            model.Gateway.GatewayTypeId = getwayTypeId;
            model.Gateway.GatewayList = new SelectList(notificationgateway, "GatewayId", "GatewayName");

            return View(model);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Notification_Gateway)]
        public async Task<ActionResult> Create()
        {
            var notificationgatewaymodel = new NotificationGatewayModel();
            return View(notificationgatewaymodel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Notification_Gateway)]
        [HttpPost]
        public async Task<ActionResult> Create(NotificationGatewayModel notificationgateway)
        {
            var SessionModel = Session["User"] as SessionModel;
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
                    UserId = SessionModel.UserId,
                    CompanyId=SessionModel.CompanyId,
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Notification_Gateway)]
        public async Task<ActionResult> Edit(int id)
        {
            var notificationgateway = await _gatewayRepo.GetGatewayById(id);
            return Json(notificationgateway, JsonRequestBehavior.AllowGet);
        }

    }

}