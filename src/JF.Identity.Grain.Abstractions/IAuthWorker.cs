using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;

namespace JF.Identity.Grain
{
    public interface IAuthWorker: IGrainWithIntegerKey
    {
        Task<CommandResult> SignUpAsync(SignUpCommand cmd);
    }
}
