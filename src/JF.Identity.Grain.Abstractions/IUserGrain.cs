using System;
using System.Threading.Tasks;
using Orleans;

namespace JF.Identity.Grain
{
    public interface IUserGrain: IGrainWithIntegerKey
    {
        Task BeginSignUpAsync(string password);
    }
}
