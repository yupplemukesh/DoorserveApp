﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class ServiceModel:RegistrationModel
    {
        public Guid? ServiceId { get; set; }
        [Required(ErrorMessage = "Please enter Category Name")]
        [DisplayName("Category Name")]
        public int CategoryId { get; set; }
        public string Category { get; set; }

        [Required(ErrorMessage = "Please enter Sub-Category Name")]
        [DisplayName("Sub-Category Name")]
        public int SubCategoryId { get; set; }
        public string SubCategory { get; set; }
        public int CountPin { get; set; }

        [Required(ErrorMessage = "Please select Service Type")]
        [DisplayName("Service Type")]
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }

        [Required(ErrorMessage = "Please select Service Delivery Type")]
        [DisplayName("Service Delivery Type")]
        public int DeliveryTypeId{ get; set; }
        public string DeliveryType { get; set; }

        [Required(ErrorMessage = "Please enter Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter Amount")]
        public decimal? ServiceCharges { get; set; }

        [Required(ErrorMessage = "Please enter Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter Amount")]
        public decimal? MApprovalCost { get; set; }

        [Required(ErrorMessage = "Please enter Warranty")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter number only")]
        public int? WarranyPeriod { get; set; }

        [Required(ErrorMessage = "Please enter TAT(Hours)")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter number only")]
        public int? TargetApprovalTime { get; set; }


        public Guid? RefKey { get; set; }
        public SelectList SupportedCategoryList { get; set; }
        public SelectList SupportedSubCategoryList { get; set; }      
        public SelectList ServiceList { get; set; }     
        public SelectList DeliveryServiceList { get; set; }
       
    }
}