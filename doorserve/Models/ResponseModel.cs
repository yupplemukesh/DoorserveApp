using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class ResponseModel
    {
        public int ResponseCode { get; set; }
        public string Response { get; set; }
        public string result { get; set; }
        public bool IsSuccess { get; set; }
    }
}