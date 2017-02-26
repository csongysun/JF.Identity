using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Common.Options
{
    public class IdentityOptions
    {
        public ClaimsIdentityOption ClaimsIdentity { get; set; } = new ClaimsIdentityOption();
        public SignInOptions SignIn { get; set; } = new SignInOptions();
        public LockoutOptions Lockout { get; set; } = new LockoutOptions();
        public SignUpOption SignUp { get; set; } = new SignUpOption();
    }
    
}
