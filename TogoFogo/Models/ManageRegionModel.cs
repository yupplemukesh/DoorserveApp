using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ManageRegionModel:RegistrationModel
    {
        public Guid RegionId { get; set; }
        public string RegionName { get; set; }
        public string StateXml { get; set; }
        public SelectList StateList { get; set; }
        public  List<int> SelectedStates { get; set; }


    }
}