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
            templates.MessageTypeList = new SelectList(await CommonModel.GetLookup("Message"), "Value", "Text");
            return View(templates);
        }
        public async Task<ActionResult> Create()
        {
            var templatemodel = new TemplateModel();
            templatemodel.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            templatemodel.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            templatemodel.TemplateTypeList = new SelectList(await CommonModel.GetLookup("Template"), "Value", "Text");
            templatemodel.PriorityTypeList = new SelectList(await CommonModel.GetLookup("Priority"), "Value", "Text");
            templatemodel.EmailHeaderFooterList = new SelectList(await CommonModel.GetHeaderFooter(), "Value", "Text");
            //templatemodel.GatewayList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
            templatemodel.IsSystemDefined = true;
            return View(templatemodel);
        }
        [HttpPost]
        public async Task<ActionResult> Create(TemplateModel templateModel)
        {
            if (ModelState.IsValid)
            {
                templateModel.AddedBy = Convert.ToInt32(Session["User_ID"]);
                var response = await _templateRepo.AddUpdateDeleteTemplate(templateModel, 'I');
                _templateRepo.Save();
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else
            {
                templateModel.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
                templateModel.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
                templateModel.TemplateTypeList = new SelectList(await CommonModel.GetLookup("Template"), "Value", "Text");
                templateModel.PriorityTypeList = new SelectList(await CommonModel.GetLookup("Priority"), "Value", "Text");
                templateModel.EmailHeaderFooterList = new SelectList(await CommonModel.GetHeaderFooter(), "Value", "Text");
                //templateModel.GatewayList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
                return View(templateModel);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var template = await _templateRepo.GetTemplateById(id);
            var array = template.ActionTypeIds.Split(',');
            List<int> ActionTypes = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                ActionTypes.Add(Convert.ToInt32(array[i]));


            }
            template.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            template.actionTypes = ActionTypes;
            return View(template);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TemplateModel templatemodel)
        {
            if (ModelState.IsValid)
            {
                templatemodel.AddedBy = Convert.ToInt32(Session["User_ID"]);

                var response = await _templateRepo.AddUpdateDeleteTemplate(templatemodel, 'U');
                _templateRepo.Save();

                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else
                templatemodel.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
                templatemodel.MessageTypeList = new SelectList(await CommonModel.GetLookup("Gateway"), "Value", "Text");
                templatemodel.TemplateTypeList = new SelectList(await CommonModel.GetLookup("Template"), "Value", "Text");
                templatemodel.PriorityTypeList = new SelectList(await CommonModel.GetLookup("Priority"), "Value", "Text");
                templatemodel.EmailHeaderFooterList = new SelectList(await CommonModel.GetHeaderFooter(), "Value", "Text");
                return View(templatemodel);

        }
        public JsonResult BindGateway(Int64 GatewayTypeId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var gateway = con.Query<BindGateway>("select GatewayId,GatewayName from MSTGateway where GatewayTypeId="+ GatewayTypeId + "", commandType: CommandType.Text);
                List<ListItem> items = new List<ListItem>();
                //items.Add(new ListItem
                //{
                //    Value = "", //Value Field(ID)
                //    Text = "--Select--" //Text Field(Name)
                //});
                foreach (var val in gateway)
                {
                    items.Add(new ListItem
                    {
                        Value = val.Id.ToString(), //Value Field(ID)
                        Text = val.GatewayName //Text Field(Name)l
                    });
                }
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
   }
}