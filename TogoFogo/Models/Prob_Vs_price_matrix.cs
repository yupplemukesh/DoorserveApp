using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class Prob_Vs_price_matrix
    {
        public Prob_Vs_price_matrix()
        {
            BrandList= new SelectList(Enumerable.Empty<SelectListItem>());
            ProblemList = new SelectList(Enumerable.Empty<SelectListItem>());
            ModelList=new SelectList(Enumerable.Empty<SelectListItem>());           
        }
        public int SerialNo { get; set; }
        [DisplayName("Brand")]
        [Required(ErrorMessage = "Select Brand Name")]
        public int BrandId { get; set; }
        [DisplayName("Brand")]        
        public string BrandName { get; set; }
        [DisplayName("Model")]
        [Required(ErrorMessage = "Select Model")]
        public string Model_Id { get; set; }
        [DisplayName("Product")]       
        public string ProductName { get; set; }
        [DisplayName("Problem")]
        public string Problem_Id { get; set; }
        [DisplayName("Problem")]
        [Required(ErrorMessage = "Select Problem")]
        public string Problem { get; set; }
        [DisplayName("Market Price")]
        [Required(ErrorMessage = "Enter Market Price")]
        public string Market_Price { get; set; }
        [DisplayName("Estimated Price")]
        [Required(ErrorMessage = "Enter Estimated Price")]
        public string estimated_Price { get; set; }
        [DisplayName("Min Price")]
        [Required(ErrorMessage = "Enter Min Price")]
        public string Min_Price { get; set; }
        [DisplayName("Max Price")]
        [Required(ErrorMessage = "Enter Max Price")]
        public string Max_Price { get; set; }
        public int UserId { get; set; }
        public long CreatedBy { get; set; }
        [DisplayName("Created By")]
        public string CBy { get; set; }
        public long ModifyBy { get; set; }
        [DisplayName("Modify By")]
        public string MBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyDate { get; set; }        
        public SelectList BrandList { get; set; }
        public SelectList ProblemList { get; set; }
        public SelectList ModelList { get; set; }



    }
}