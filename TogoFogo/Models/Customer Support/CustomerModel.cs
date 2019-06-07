using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class CustomerModel
    {

        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string AltContactNumber { get; set; }
        public string Email { get; set; }
        public int AddressTypeId { get; set; }
        public int CustomerTypeId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public string Locality { get; set; }
        public string NearLocation { get; set; }
        public string PinNumber { get; set; }
    }
}