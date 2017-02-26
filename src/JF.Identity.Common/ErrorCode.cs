using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Common
{
    public static class ErrorCode
    {
        public const string UnkownError = nameof(UnkownError);
        public const string EmailAlreadyExist = nameof(EmailAlreadyExist);
        public const string UserNotFound = nameof(UserNotFound);
    }
}
