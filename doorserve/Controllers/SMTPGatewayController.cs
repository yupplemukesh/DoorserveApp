using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using doorserve.Models;
using doorserve.Models.Gateway;
using doorserve.Repository.SMSGateway;
using AutoMapper;
using doorserve.Permission;

namespace doorserve.Controllers
{
    public class SMTPGatewayController : Controller
    {
        private readonly IGateway _gatewayRepo;
        public SMTPGatewayController()

        {      
            _gatewayRepo = new Gateway();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.EMail_Gateway_Settings)]
        public async Task<ActionResult> Index()
        {
            var SessionModel = Session["User"] as SessionModel;
            //   var smtpgateway = new SMTPGatewayList();
            var Gatewaylist = await CommonModel.GetGatewayType();
            var GatewayTypeId = Gatewaylist.Where(x => x.Text == "SMTP Gateway").Select(x => x.Value).SingleOrDefault();
            var GatewayModel = await _gatewayRepo.GetGatewayByType(new Filters.FilterModel {GatewayTypeId=GatewayTypeId,CompId=SessionModel.CompanyId });
            SMTPGateWayMainModel model = new SMTPGateWayMainModel();
            model.Gateway = new SMTPGatewayModel();
            
            model.Gateway.GatewayTypeId = GatewayTypeId;
            model.Gateway.GatewayList = new SelectList(GatewayModel, "GatewayId", "GatewayName");
            model.mainModel = Mapper.Map<List<SMTPGatewayModel>>(GatewayModel);
            return View(model);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.EMail_Gateway_Settings)]
        public async Task<ActionResult> Create()
        {
            var smtpgatewaymodel = new SMTPGatewayModel();
            return View(smtpgatewaymodel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.EMail_Gateway_Settings)]
        [HttpPost]
        public async Task<ActionResult> Create(SMTPGatewayModel smtpgateway)
           {
            if (ModelState.IsValid)
            {
                var SessionModel = Session["User"] as SessionModel;
                var Gatewaylist = await CommonModel.GetGatewayType();
                var GatewayTypeId = Gatewaylist.Where(x => x.Text == "SMTP Gateway").Select(x => x.Value).SingleOrDefault();
                var GatewayModel = new GatewayModel
                {
                    GatewayId = smtpgateway.GatewayId,
                    GatewayTypeId = GatewayTypeId,
                    GatewayName = smtpgateway.GatewayName,
                    IsActive = smtpgateway.IsActive,
                    IsDefault= smtpgateway.IsDefault,
                    IsProcessByAWS= smtpgateway.IsProcessByAWS,
                    Name =smtpgateway.Name,
                    Email=smtpgateway.Email,
                    SmtpServerName=smtpgateway.SmtpServerName,
                    SmtpUserName=smtpgateway.SmtpUserName,
                    SmtpPassword=smtpgateway.SmtpPassword,
                    PortNumber=smtpgateway.PortNumber,
                    SSLEnabled=smtpgateway.SSLEnabled,
                    UserId =SessionModel.UserId,
                    CompanyId=SessionModel.CompanyId
                };
               
              var   response = await _gatewayRepo.AddUpdateDeleteGateway(GatewayModel, 'I');
                _gatewayRepo.Save();
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else
                return View(smtpgateway);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.EMail_Gateway_Settings)]
        public async Task<ActionResult> Edit(int id)
        {
            var GatewayModel = await _gatewayRepo.GetGatewayById(id);
            var SmtpGatewayModel = Mapper.Map<SMTPGatewayModel>(GatewayModel);
            return View(SmtpGatewayModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit}, (int)MenuCode.EMail_Gateway_Settings)]
        [HttpPost]
        public async Task<ActionResult> Edit(SMTPGatewayModel smtpgateway)
        {
            if (ModelState.IsValid)
            {
                var SessionModel = Session["User"] as SessionModel;
                var GatewayModel = new GatewayModel
                {
                    GatewayId = smtpgateway.GatewayId,
                    GatewayTypeId = smtpgateway.GatewayTypeId,
                    GatewayName = smtpgateway.GatewayName,
                    IsActive = smtpgateway.IsActive,
                    IsDefault = smtpgateway.IsDefault,
                    IsProcessByAWS = smtpgateway.IsProcessByAWS,
                    Name = smtpgateway.Name,
                    Email = smtpgateway.Email,
                    SmtpServerName = smtpgateway.SmtpServerName,
                    SmtpUserName = smtpgateway.SmtpUserName,
                    SmtpPassword = smtpgateway.SmtpPassword,
                    PortNumber = smtpgateway.PortNumber,
                    SSLEnabled = smtpgateway.SSLEnabled,
                    UserId = SessionModel.UserId,
                    CompanyId=SessionModel.CompanyId
                };
                   var  response = await _gatewayRepo.AddUpdateDeleteGateway(GatewayModel, 'U');
                _gatewayRepo.Save();
                TempData["response"] = response;

                return RedirectToAction("Index");
            }
            else
                return View(smtpgateway);

        }
    }
}