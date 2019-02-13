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
namespace TogoFogo.Controllers
{
    public class SMSGatewayController : Controller
    {
        private readonly IGateway  _smsGatewayModel;
        public SMSGatewayController()

        {
           _smsGatewayModel = new Gateway();
                   }
        public async Task<ActionResult> Index()


        {
            var getwaylist = await CommonModel.GetGatewayType();


            var getwayTypeId = getwaylist.Where(x => x.Text == "SMS Gateway").Select(x => x.Value).SingleOrDefault();
            
            var smsgateway = await _smsGatewayModel.GetGatewayByType(getwayTypeId);
            
            SMSGateWayMainModel model = new SMSGateWayMainModel();
            model.Gateway = new SMSGatewayModel();
            model.mainModel = Mapper.Map<List<SMSGatewayModel>>(smsgateway);
            model.Gateway.GatewayTypeId = getwayTypeId;
            model.Gateway.GatewayList = new SelectList(smsgateway, "GatewayId", "GatewayName");
            return View(model);
        }
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
                var gatewayModel = new GatewayModel {
                    GatewayId=smsgateway.GatewayId,
                    GatewayTypeId=smsgateway.GatewayTypeId,
                    GatewayName=smsgateway.GatewayName,
                    IsActive=smsgateway.IsActive,
                    OTPApikey=smsgateway.OTPApikey,
                    TransApikey=smsgateway.TransApikey,
                    URL=smsgateway.URL,
                    OTPSender=smsgateway.OTPSender,
                    SuccessMessage=smsgateway.SuccessMessage,
                    AddeddBy= Convert.ToInt32(Session["User_ID"])
                };
                ResponseModel response= new ResponseModel();
                if (gatewayModel.GatewayId != 0)
                     response = await _smsGatewayModel.AddUpdateDeleteGateway(gatewayModel, 'U');
                else
                    response = await _smsGatewayModel.AddUpdateDeleteGateway(gatewayModel, 'I');
                _smsGatewayModel.Save();
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else
                return View(smsgateway);

        }
        public async Task<ActionResult> Edit(int id)
        {
            var smsgateway = await _smsGatewayModel.GetGatewayById(id);
            return Json(smsgateway,JsonRequestBehavior.AllowGet);
        }
      
    }
}