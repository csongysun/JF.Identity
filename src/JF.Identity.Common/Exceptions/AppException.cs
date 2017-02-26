using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Common.Exceptions
{
   public class AppException: Exception
    {
        public AppException(int code, string msg)
            :base(msg)
        {
            this.HResult = code;
        }
        public AppException(int code, string msg, Exception e)
            : base(msg, e)
        {
            this.HResult = code;
        }
    }
}
