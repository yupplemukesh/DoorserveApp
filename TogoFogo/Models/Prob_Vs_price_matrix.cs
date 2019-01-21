using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class Prob_Vs_price_matrix
    {
        [DisplayName("Brand")]
        public string BrandName { get; set; }
        [DisplayName("Model")]
        public string Model_Id { get; set; }
        [DisplayName("Product")]
        public string ProductName { get; set; }
        [DisplayName("Problem")]
        public string Problem_Id { get; set; }
        [DisplayName("Problem")]
        public string Problem { get; set; }
        [DisplayName("Market Price")]
        public string Market_Price { get; set; }
        [DisplayName("Estimated Price")]
        public string estimated_Price { get; set; }
        [DisplayName("Min Price")]
        public string Min_Price { get; set; }
        [DisplayName("Max Price")]
        public string Max_Price { get; set; }
        
    }
}