using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ServiceModel:RegistrationModel
    {
        public Guid? ServiceId { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public int CategoryId { get; set; }
        public string Category { get; set; }
        [Required]
        [DisplayName("Sub-Category Name")]
        public int SubCategoryId { get; set; }
        public string SubCategory { get; set; }
        [Required]
        [DisplayName("Service Type")]
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        [Required]
        [DisplayName("Service Delivery Type")]
        public int DeliveryTypeId{ get; set; }
        public string DeliveryType { get; set; }
        [Required]
        [DisplayName("Service Delivery Type")]
        public decimal? ServiceCharges { get; set; }
     
      
        public Guid? RefKey { get; set; }
        public SelectList SupportedCategoryList { get; set; }
        public SelectList SupportedSubCategoryList { get; set; }      
        public SelectList ServiceList { get; set; }     
        public SelectList DeliveryServiceList { get; set; }
       
    }
}