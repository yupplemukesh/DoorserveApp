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

namespace TogoFogo.Controllers
{
    public class SMTPGatewayController : Controller
    {
        private readonly IGateway _gatewayRepo;
        public SMTPGatewayController()

        {
            _gatewayRepo = new Gateway();
        }
        public async Task<ActionResult> Index()
        {
            var Gatewaylist = await CommonModel.GetGatewayType();
            var GatewayTypeId = Gatewaylist.Where(x => x.Text == "SMTP Gateway").Select(x => x.Value).SingleOrDefault();
            var GatewayModel = await _gatewayRepo.GetGatewayByType(GatewayTypeId);
            var SmtpGatewayModel = Mapper.Map<List<SMTPGatewayModel>>(GatewayModel);
            return View(SmtpGatewayModel);
        }
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
                var GatewayModel = new GatewayModel
                {
                    GatewayId = smtpgateway.GatewayId,
                    GatewayTypeId = smtpgateway.GatewayTypeId,
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