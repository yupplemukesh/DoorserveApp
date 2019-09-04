using System;
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
    public class SMSGatewayController : BaseController
    {
        private readonly IGateway  _gatewayRepo;
        public SMSGatewayController()

        {
            _gatewayRepo = new Gateway();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.SMS_Gateway_Settings)]
        public async Task<ActionResult> Index()
        {

            var getwaylist = await CommonModel.GetGatewayType();


            var getwayTypeId = getwaylist.Where(x => x.Text == "SMS Gateway").Select(x => x.Value).SingleOrDefault();
            
            var smsgateway = await _gatewayRepo.GetGatewayByType(new Filters.FilterModel {GatewayTypeId=getwayTypeId,CompId=CurrentUser.CompanyId });
            
            SMSGateWayMainModel model = new SMSGateWayMainModel();
            model.Gateway = new SMSGatewayModel();
            if (CurrentUser.UserTypeName.ToLower() == "super admin")
            {
                model.Gateway.IsAdmin = true;
                model.Gateway.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
            }




            model.mainModel = Mapper.Map<List<SMSGatewayModel>>(smsgateway);
            model.Gateway.GatewayTypeId = getwayTypeId;
            model.Gateway.GatewayList = new SelectList(smsgateway, "GatewayId", "GatewayName");
   
            return View(model);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.SMS_Gateway_Settings)]
        public async Task<ActionResult> Create()
        {
            var smsgatewaymodel = new SMSGatewayModel();
            return View(smsgatewaymodel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.SMS_Gateway_Settings)]
        [HttpPost]
        public async Task<ActionResult> Create(SMSGatewayModel smsgateway)
        {
            if (ModelState.IsValid)
            {
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
                    UserId = CurrentUser.UserId,
                    CompanyId=CurrentUser.CompanyId
                };
                if (CurrentUser.UserTypeName.ToLower() == "super admin")
                    gatewayModel.CompanyId = smsgateway.CompanyId;
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