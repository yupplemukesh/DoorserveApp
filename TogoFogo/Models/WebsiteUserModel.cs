using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class WebsiteUserModel
    {
        public int Id { get; set; }
        [DisplayName("Customer Name")]
        public string Customer_Name { get; set; }
        [DisplayName("Email")]
        public string Email_Id { get; set; }
        [DisplayName("Mobile")]
        public string Mobile_No { get; set; }
        [DisplayName("Address Type")]
        public string AddressType { get; set; }
        [DisplayName("Address")]
        public string Cust_Add { get; set; }
        public string Pincode { get; set; }
        [DisplayName("State")]
        public string Cust_State { get; set; }
        [DisplayName("City")]
        public string Cust_City { get; set; }
        [DisplayName("Entry Date")]
        public string Entry_date { get; set; }
        [DisplayName("User Status")]
        public string WebsiteUserStatus { get; set; }
        public bool Status { get; set; }
    }
    public class WebsiteUserModelReport
    {
        
        [DisplayName("Customer Name")]
        public string Customer_Name { get; set; }
        [DisplayName("Email")]
        public string Email_Id { get; set; }
        [DisplayName("Mobile")]
        public string Mobile_No { get; set; }
        [DisplayName("Address Type")]
        public string AddressType { get; set; }
        [DisplayName("Address")]
        public string Cust_Add { get; set; }
        public string Pincode { get; set; }
        [DisplayName("State")]
        public string Cust_State { get; set; }
        [DisplayName("City")]
        public string Cust_City { get; set; }
        [DisplayName("Entry Date")]
        public string Entry_date { get; set; }
        [DisplayName("User Status")]
        public string WebsiteUserStatus { get; set; }

    }
}