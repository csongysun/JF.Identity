using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace JF.Identity.Common
{
    public class XError
    {
        public static XError Ok = new XError(HttpStatusCode.OK);

        public XError(HttpStatusCode statusCode)
        {
            StatusCode =(int) statusCode;
        }

        public XError(HttpStatusCode statusCode, string errorCode)
        {
            StatusCode =(int) statusCode;
            ErrorCode = errorCode;
        }
        public XError(HttpStatusCode statusCode, string errorCode, params string[] args)
        {
            StatusCode = (int)statusCode;
            ErrorCode = string.Join(";", statusCode, args);
        }

        public int StatusCode { get; private set; }
        public string ErrorCode { get; private set; }

        public bool IsOk => StatusCode == 200;
    }
}
