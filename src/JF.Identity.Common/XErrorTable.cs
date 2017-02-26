using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace JF.Identity.Common
{
    public static class ErrorTable
    {
        public static XError UserNotFound = new XError(HttpStatusCode.NotFound, nameof(UserNotFound));

        #region Password Login

        public static XError UserLockedOut(DateTimeOffset endTime) =>
            new XError(HttpStatusCode.Forbidden, nameof(UserLockedOut), endTime.UtcTicks.ToString());

        public static XError PasswordIncorrect = new XError(HttpStatusCode.Forbidden, nameof(PasswordIncorrect));

        #endregion

        #region Refresh Login

        public static XError RefreshTokenInvalid = new XError(HttpStatusCode.Forbidden, nameof(RefreshTokenInvalid));

        public static XError SecurityStampInvalid = new XError(HttpStatusCode.Forbidden, nameof(SecurityStampInvalid));

        #endregion



    }
}
