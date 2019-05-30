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
    public class SMSGatewayController : Controller
    {
        private readonly IGateway  _gatewayRepo;
        public SMSGatewayController()

        {
            _gatewayRepo = new Gateway();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.SMS_Gateway_Settings)]
        public async Task<ActionResult> Index()
        {
            var SessionModel = Session["User"] as SessionModel;
            var getwaylist = await CommonModel.GetGatewayType();


            var getwayTypeId = getwaylist.Where(x => x.Text == "SMS Gateway").Select(x => x.Value).SingleOrDefault();
            
            var smsgateway = await _gatewayRepo.GetGatewayByType(new Filters.FilterModel {GatewayTypeId=getwayTypeId,CompId=SessionModel.CompanyId });
            
            SMSGateWayMainModel model = new SMSGateWayMainModel();
            model.Gateway = new SMSGatewayModel();
            model.mainModel = Mapper.Map<List<SMSGatewayModel>>(smsgateway);
            model.Gateway.GatewayTypeId = getwayTypeId;
            model.Gateway.GatewayList = new SelectList(smsgateway, "GatewayId", "GatewayName");
            //model.Rights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(model);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.SMS_Gateway_Settings)]
        public async Task<ActionResult> Create()
        {
            var smsgatewaymodel = new SMSGatewayModel();
            return View(smsgatewaymodel);
        }
        [HttpPost]
        public async Task<ActionResult> Create(SMSGatewayModel smsgateway)
        {
            if (ModelState.IsValid)
            {
                var SessionModel = Session["User"] as SessionModel;
                var gatewayModel = new GatewayModel {
                    GatewayId = smsgateway.GatewayId,
                    GatewayTypeId = smsgateway.GatewayTypeId,
                    GatewayName = smsgateway.GatewayName,
                    IsActive = smsgateway.IsActive,
                    OTPApikey = smsgateway.OTPApikey,
                    TransApikey = smsgateway.TransApikey,
                    URL = smsgateway.URL,
                    OTPSender = smsgateway.OTPSender,
                    SuccessMessage = smsgateway.SuccessMessage,
                    UserId = SessionModel.UserId,
                    CompanyId=SessionModel.CompanyId
                };
                ResponseModel response= new ResponseModel();
                if (gatewayModel.GatewayId != 0)
                     response = await _gatewayRepo.AddUpdateDeleteGateway(gatewayModel, 'U');
                else
                    response = await _gatewayRepo.AddUpdateDeleteGateway(gatewayModel, 'I');
                _gatewayRepo.Save();
                TempData["response"] = response;
                return RedirectToAction("Index");
            }
            else
                return View(smsgateway);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.SMS_Gateway_Settings)]
        public async Task<ActionResult> Edit(int id)
        {
            var smsgateway = await _gatewayRepo.GetGatewayById(id);
            return Json(smsgateway,JsonRequestBehavior.AllowGet);
        }
      
    }
}