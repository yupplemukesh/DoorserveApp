using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace TogoFogo.Extended
{
    public static class UserExtended
    {
        public static string GetId(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Email);
            return claim == null ? null : claim.Value;
        }
    }
}