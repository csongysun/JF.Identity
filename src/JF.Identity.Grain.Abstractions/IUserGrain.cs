using System;
using System.Threading.Tasks;
using Orleans;

namespace JF.Identity.Grain.Abstractions
{
    public interface IUserGrain: IGrainWithGuidKey
    {
        Task<string> SignUpAsync(string email);
    }
}
