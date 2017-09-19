using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JF.Identity.Domain.Command;
using JF.Identity.Grain.Commands;
using Orleans;
using Orleans.Concurrency;

namespace JF.Identity.Grain
{
    public interface IAuthWorker: 
        ICommandHandler<SignUpCommand>
    {
    }
}
