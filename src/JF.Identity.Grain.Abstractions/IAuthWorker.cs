using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;

namespace JF.Identity.Grain
{
    public interface IAuthWorker: IGrain
    {
        Task<CommandResult> SignUpAsync(string email);
    }
}
