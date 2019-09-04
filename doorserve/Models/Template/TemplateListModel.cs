using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models.Template
{
    public class TemplateListModel
    {
        public TemplateListModel() {

            TemplateTrackerList = new List<TemplateTracker>();
        }
        public int? ActionTypeId { get; set; }
        public int? MessageTypeId { get; set; }
        public int? NonMessageTypeId { get; set; }
        public SelectList ActionTypeList { get; set; }
        public SelectList MessageTypeList { get; set; }
        public List<TemplateModel> NonActionTemplates { get; set; }
        public List<TemplateModel> Templates { get; set; }
        public List<TemplateTracker> TemplateTrackerList { get; set; }        
    }
}