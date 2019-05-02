using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Models.Gateway;
using TogoFogo.Repository.SMSGateway;
using AutoMapper;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
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

         //   var smtpgateway = new SMTPGatewayList();
            var Gatewaylist = await CommonModel.GetGatewayType();
            var GatewayTypeId = Gatewaylist.Where(x => x.Text == "SMTP Gateway").Select(x => x.Value).SingleOrDefault();
            var GatewayModel = await _gatewayRepo.GetGatewayByType(GatewayTypeId);
            SMTPGateWayMainModel model = new SMTPGateWayMainModel();
            model.Gateway = new SMTPGatewayModel();
            
            model.Gateway.GatewayTypeId = GatewayTypeId;
            model.Gateway.GatewayList = new SelectList(GatewayModel, "GatewayId", "GatewayName");
            model.mainModel = Mapper.Map<List<SMTPGatewayModel>>(GatewayModel);
            // var SmtpGatewayModel = Mapper.Map<List<SMTPGatewayModel>>(GatewayModel);
            // smtpgateway.SMTPGateway= Mapper.Map<List<SMTPGatewayModel>>(GatewayModel);
            //smtpgateway.Rights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(model);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.EMail_Gateway_Settings)]
        public async Task<ActionResult> Create()
        {
            var smtpgatewaymodel = new SMTPGatewayModel();
            return View(smtpgatewaymodel);
        }
        [HttpPost]
        public async Task<ActionResult> Create(SMTPGatewayModel smtpgateway)
           {
            if (ModelState.IsValid)
            {
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
                    AddeddBy = Convert.ToInt32(Session["User_ID"])
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
        [HttpPost]
        public async Task<ActionResult> Edit(SMTPGatewayModel smtpgateway)
        {
            if (ModelState.IsValid)
            {
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
                    AddeddBy = Convert.ToInt32(Session["User_ID"])
                };
                   var  response = await _gatewayRepo.AddUpdateDeleteGateway(GatewayModel, 'U');
                _gatewayRepo.Save();
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else
                return View(smtpgateway);

        }
    }
}