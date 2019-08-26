using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public static class Permissions
    {
        public static UserActionRights Rights;

        public static void AssignRight(UserActionRights rights)
        {
            Rights = rights;

        }
    }
}