using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Common.Exceptions
{
    public class JFIdentityException: AppException
    {
        public JFIdentityException(int code, string msg):base(code, msg)
        { }

        public JFIdentityException(int code, string msg, Exception e) : base(code, msg)
        { }
    }
}
