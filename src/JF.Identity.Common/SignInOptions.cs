using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Common
{
    public class SignInOptions
    {
        public bool RequireConfirmedEmail { get; set; } = false;
        public TokenOption AccessToken { get; set; }
        public TokenOption RefreshToken { get; set; }
    }
}
