﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models.Customer_Support
{
    public class AllocateCallModel
    {
        public List<Guid> SelectedDevices { get; set; }
        public SelectList ToAllocateList { get; set; }
        public int AllocateId  { get; set; }
    } 
}