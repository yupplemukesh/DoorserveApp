using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TogoFogo.Models.Template;
using TogoFogo.Repository.EmailSmsTemplate;

namespace TogoFogo.Controllers
{
    public class TemplatesController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private readonly ITemplate _templateRepo;  

        public TemplatesController()
        {
            _templateRepo = new Template();
        }

        public async Task<ActionResult> Index()
        {

            var templates = await _templateRepo.GetTemplates();
            templates.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templates.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            return View(templates);
        }
        public async Task<ActionResult> Create()
        {
            var templatemodel = new TemplateModel();
            templatemodel.IsActive = true;
            templatemodel.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templatemodel.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            templatemodel.TemplateTypeList = new SelectList(await CommonModel.GetLookup("Template"), "Value", "Text");
            templatemodel.PriorityTypeList = new SelectList(await CommonModel.GetLookup("Priority"), "Value", "Text");
            templatemodel.EmailHeaderFooterList = new SelectList(await CommonModel.GetHeaderFooter(), "Value", "Text");
            
            templatemodel.IsSystemDefined = true;
            return View(templatemodel);
        }
        [HttpPost]
        public async Task<ActionResult> Create(TemplateModel templateModel)
        {
            
                templateModel.AddedBy = Convert.ToInt32(Session["User_ID"]);
                var response = await _templateRepo.AddUpdateDeleteTemplate(templateModel, 'I');
                _templateRepo.Save();
            if (response.ResponseCode == 0)
            {
                response.Response = "Successfully insert details";
            }
            else if (response.ResponseCode == 2)
            {
                response.Response = "Already exists details";
            }
            else
            {
                response.Response = "Someting went wrong,please try again";
            }
            TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
           
        }

        public async Task<ActionResult> Edit(int id)
        {
            var templatemodel = new TemplateModel();
           
            templatemodel = await _templateRepo.GetTemplateById(id);
            templatemodel.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templatemodel.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            templatemodel.TemplateTypeList = new SelectList(await CommonModel.GetLookup("Template"), "Value", "Text");
            templatemodel.PriorityTypeList = new SelectList(await CommonModel.GetLookup("Priority"), "Value", "Text");
            templatemodel.EmailHeaderFooterList = new SelectList(await CommonModel.GetHeaderFooter(), "Value", "Text");
            templatemodel.GatewayList=new SelectList(await CommonModel.GetMailerGatewayList(templatemodel.MessageTypeId), "GatewayId", "GatewayName"); 

            return View(templatemodel);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(TemplateModel templatemodel)
        {
            
                templatemodel.AddedBy = Convert.ToInt32(Session["User_ID"]);

                var response = await _templateRepo.AddUpdateDeleteTemplate(templatemodel, 'U');
                _templateRepo.Save();
            if(response.ResponseCode==0)
            {
                response.Response = "Successfully updated";
            }
            else if(response.ResponseCode == 2)
            {
                response.Response = "Already exists details";
            }
            else
            {
                response.Response = "Someting went wrong,please try again";
            }
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
        

        }
        public JsonResult BindGateway(Int64 GatewayTypeId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var gateway = con.Query<BindGateway>("select GatewayId,GatewayName from MSTGateway where GatewayTypeId="+ GatewayTypeId + "", commandType: CommandType.Text);
                List<ListItem> items = new List<ListItem>();
                
                foreach (var val in gateway)
                {
                    items.Add(new ListItem
                    {
                        Value = val.GatewayId.ToString(), //Value Field(ID)
                        Text = val.GatewayName //Text Field(Name)l
                    });
                }
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        //public async Task<JsonResult> GetTemplatefilterData(int MessageTypeId, int ActionTypeId, string MailerTemplateName)
        //{
        //    var templates = await _templateRepo.GetTemplatesByMessageTypeActionType(MessageTypeId, ActionTypeId, MailerTemplateName);

        //    return Json(templates, JsonRequestBehavior.AllowGet);
        //}
    }
}