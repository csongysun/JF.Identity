using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace JF.Identity.Store.Model
{
    public class UserClaim
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }

        public virtual void InitializeFromClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }
}
