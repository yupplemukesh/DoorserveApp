using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models.Customer_Support
{
    public class AllocateCallModel
    {
        public List<DeviceModel> SelectedDevices { get; set; }
        public SelectList ToAllocateList { get; set; }
        public Guid AllocateId  { get; set; }
        public string AllocateTo { get; set; }
        public int UserId { get; set; }
    }
    public class DeviceModel
    { 
    public Guid DataId { get; set; }
    public Guid DeviceId { get; set; }
    }
}