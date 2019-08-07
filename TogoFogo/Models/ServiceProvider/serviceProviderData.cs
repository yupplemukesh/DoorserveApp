using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class serviceProviderData
    {
        public string ProcessName { get; set; }
        public string ServiceProviderCode { get; set; }
        public string ServiceProviderName { get; set; }
        public string ServiceDeliveryType { get; set; }
        public string SupportedDeviceCategory { get; set; }
        public string ServiceType { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationIECNumber { get; set; }
        public string StatutoryType { get; set; }
        public string ApplicableTaxType{ get; set; }
        public string GSTCategory{ get; set; }
        public string GSTNumber{ get; set; }
        public string PANCardNumber{ get; set; }
        public string IsServiceCenter { get; set; }
        public string ContactName { get; set; }
        public string ContactMobile{ get; set; }
        public string ContactEmail{ get; set; }
        public string ContactPAN{ get; set; }
        public string ContactVoterId{ get; set; }
        public string ContactAdhaar{ get; set; }
        public string AddressType{ get; set; }
        public string Country { get; set; }
        public string State  { get; set; }
        public string City        { get; set; }
        public string Address { get; set; }
        public string Locality        { get; set; }
        public string NearByLocation        { get; set; }
        public string PinCode { get; set; }
        public string IsUser { get; set; }
        public string ServiceableAreaPinCode { get; set; }
    }
}