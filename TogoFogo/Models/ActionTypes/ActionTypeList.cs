using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.ActionTypes
{
    public class ActionTypeList
    {
        public List<ActionTypeModel> ActionTypes { get; set; }
        public UserActionRights Rights { get; set; }
    }
}