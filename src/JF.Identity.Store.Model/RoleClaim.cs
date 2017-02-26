using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace JF.Identity.Store.Model
{
    public class RoleClaim
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public Claim ToClaim() => new Claim(ClaimType, ClaimValue);

        public void InitializeFromClaim(Claim other)
        {
            ClaimType = other?.Type;
            ClaimValue = other?.Value;
        }
    }
}
