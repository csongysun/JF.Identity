using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Common
{
    public class LoggingEvents
    {

        #region DB

        public const int USER_DB = 10000;
        public const int USER_DB_FAILED = 10001;

        public const int ROLE_DB = 10010;
        public const int ROLE_DB_FAILED = 10011;

        #endregion

        #region Manager

        public const int USER_CREATE = 10100;
        public const int USER_CREATE_FAILED = 10101;

        public const int USER_UPDATE = 10110;
        public const int USER_UPDATE_FAILED = 10111;


        public const int USER_ADD_TO_ROLE = 10130;
        public const int USER_ADD_TO_ROLE_FAILED = 10131;

        public const int USER_NOT_FOUND = 10141;
        public const int USER_LOCKED_OUT = 10142;
        public const int USER_PASSWORD_INCORRECT = 10143;
        public const int TOKEN_INVALID = 10144;
        public const int STAMP_INVALID = 10145;

        public const int ROLE_CREATE = 10140;
        public const int ROLE_CREATE_FAILED = 10141;

        public const int ROLE_UPDATE = 10150;
        public const int ROLE_UPDATE_FAILED = 10151;

        public const int ROLE_NOT_FOUND = 10161;

        #endregion
    }
}
