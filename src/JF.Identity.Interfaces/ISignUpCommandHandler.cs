using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JF.Identity.Interfaces
{
    public interface ISignUpCommandHandler : Orleans.IGrainWithGuidKey
    {
        Task<CommandResult> SignUpAsync(SignUpCommand cmd);
    }
}
