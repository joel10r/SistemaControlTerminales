using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace SCT.IdentityExtensions
{
    public static class IdentityExtensions
    {
        public static string GetNombreUsuario(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            if (ci != null)
            {
                return ci.FindFirst("NombreUsuario").ToString().Substring(("NombreUsuario: ").Length);
            }
            return null;

        }
    }
}