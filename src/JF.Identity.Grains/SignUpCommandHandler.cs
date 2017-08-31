using JF.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JF.Identity.Grains
{
    public class SignUpCommandHandler : Orleans.Grain, ISignUpCommandHandler
    {
        public Task<CommandResult> SignUpAsync(SignUpCommand cmd)
        {
            return Task.FromResult(new CommandResult("error"));
        }
    }
}
