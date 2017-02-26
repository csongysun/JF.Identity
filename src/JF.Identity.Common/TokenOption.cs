using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Common
{
    public class TokenOption
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; }
    }
}
