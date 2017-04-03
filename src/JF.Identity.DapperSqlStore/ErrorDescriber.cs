using CSYS.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.DapperSqlStore
{
    public static class ErrorDescriber
    {
        public static Error DBInsertFailed(string code) => new Error
        {
            Code = nameof(DBInsertFailed),
            Description = string.Format(Resources.DBInsertFailed, code)
        };
        public static Error DBUpdateFailed(string code) => new Error
        {
            Code = nameof(DBUpdateFailed),
            Description = string.Format(Resources.DBUpdateFailed, code)
        };
    }
}
