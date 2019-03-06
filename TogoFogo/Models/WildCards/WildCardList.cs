using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.WildCards
{
    public class WildCardList
    {
        public List<WildCardModel> WildCards { get; set; }
        public UserActionRights Rights  { get; set; }
    }
}