using System;
using System.Collections.Generic;
using System.Text;

namespace JF.Identity.Interfaces
{
    public class CommandResult
    {
        public CommandResult()
        { }
        public CommandResult(string errorCode)
        {
            ErrorCode = errorCode;
            Succeed = false;
        }
        public bool Succeed { get; private set; } = true;
        public string ErrorCode { get; private set; }
    }
}
