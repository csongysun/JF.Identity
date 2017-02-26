using JF.Identity.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Store
{
    public class DbException : AppException
    {
        public DbException(int code, string msg) : base(code, msg)
        {
        }

        public DbException(int code, string msg, Exception e) : base(code, msg, e)
        {
        }
    }
}
