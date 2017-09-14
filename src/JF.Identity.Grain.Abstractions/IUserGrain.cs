using System;
using System.Threading.Tasks;
using Orleans;

namespace JF.Identity.Grain.Abstractions
{
    public interface IUserGrain: IGrainWithIntegerKey
    {
        Task<CommandResult> SignUpAsync();
    }
}
