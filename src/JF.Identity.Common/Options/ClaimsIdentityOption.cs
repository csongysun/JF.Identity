using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace JF.Identity.Common.Options
{
    public class ClaimsIdentityOption
    {
        public string AuthSchema { get; set; } = "Jwt.Accese.Token";
        public string RoleClaimType { get; set; } = ClaimTypes.Role;
        public string EmailClaimType { get; set; } = ClaimTypes.Name;
        public string UserIdClaimType { get; set; } = ClaimTypes.NameIdentifier;
        public string SecurityStampClaimType { get; set; } = "SecurityStamp";
    }
}
