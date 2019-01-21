using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class Menu
    {
        public int ID { get; set; }
        public int MenuCap_ID { get; set; }
        public string Menu_Name { get; set; }
        public string Cap_Name { get; set; }
        public string PagePath { get; set; }
        public int ParentMenuID { get; set; }
        public string OrderNo { get; set; }
        public string Create_By { get; set; }
        public string Create_Date { get; set; }
        public string Modify_By { get; set; }
        public DateTime? Modify_Date { get; set; }
        public string Dele_By { get; set; }
        public DateTime? Dele_Date { get; set; }
        public string Visibility { get; set; }
    }
}