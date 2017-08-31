using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JF.Identity.Interfaces
{
    public interface ISignUp : Orleans.IGrain
    {
        Task<CommandResult> SignUpAsync(SignUpCommand cmd);
    }
}
