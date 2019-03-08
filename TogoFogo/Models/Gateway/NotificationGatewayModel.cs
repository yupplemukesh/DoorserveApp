using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models.Gateway
{
    public class NotificationGatewayModel
    {
        public NotificationGatewayModel()
        {
            GatewayList = new SelectList(Enumerable.Empty<SelectListItem>());

        }
        public Int64 GatewayId { get; set; }
        [Required]
        [DisplayName("Gateway Name")]
        public string GatewayName { get; set; }
        [DisplayName("Gateway Type Id")]
        public Int64 GatewayTypeId { get; set; }
        [DisplayName("Is Active ?")]
        public bool IsActive { get; set; }
        [Required]
        [DisplayName("Sender ID")]
        public string SenderID { get; set; }
        [Required]
        [DisplayName("Google Api key")]
        public string GoogleApikey { get; set; }
        [Url]
        [DisplayName("Google Api Url")]
        public string GoogleApiUrl { get; set; }
        [Required]
        [DisplayName("Google Project ID")]
        public string GoogleProjectID { get; set; }
        [Required]
        [DisplayName("Google Project Name")]
        public string GoogleProjectName { get; set; }
        public SelectList GatewayList { get; set; }
    }

    public class NotificationGateWayMainModel
    {
        public List<NotificationGatewayModel> mainModel { get; set; }
        public NotificationGatewayModel Gateway { get; set; }
        public UserActionRights Rights { get; set; }


    }
}