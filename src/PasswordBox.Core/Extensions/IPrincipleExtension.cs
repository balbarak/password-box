using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace PasswordBox.Core.Extensions
{
    public static class IPrincipleExtension
    {
        public static string GetUserId(this IPrincipal principal)
        {
            string result = "";

            if (principal == null)
                return null;

            if (principal is ClaimsPrincipal identity)
            {
                var found = identity.FindFirst(a => a.Type == ClaimTypes.NameIdentifier);

                if (found != null && !String.IsNullOrEmpty(found.Value))
                {
                    result = found.Value;

                    return result;
                }

                return null;

            }
            else
                return null;
        }
    }
}
