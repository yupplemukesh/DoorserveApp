using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class ServiceViewModel:ServiceModel
    {

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter number only")]
        [Required]
        public override int? TargetApprovalTime { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter number only")]
        [Required]
        public override int? WarranyPeriod { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter Amount")]
        [Required]
        public override decimal? ServiceCharges { get; set; }


        [Range(0, double.MaxValue, ErrorMessage = "Please enter Amount")]
        [Required]
        public override decimal? MApprovalCost { get; set; }

    }
}