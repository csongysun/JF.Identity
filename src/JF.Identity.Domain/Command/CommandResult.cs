using System;
using System.Collections.Generic;
using System.Text;
using Orleans.Concurrency;

namespace JF.Identity.Domain.Command
{
    [Immutable]
    public class CommandResult
    {
        public static CommandResult Ok = new CommandResult();
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
