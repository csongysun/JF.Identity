using System;
using System.Threading.Tasks;
using JF.Identity.Grain.Abstractions;
using Orleans;

namespace JF.Identity.Grain
{
    public class UserGrain: Orleans.Grain, IUserGrain
    {
        public Task<string> SignUpAsync(string email)
        {
            return Task.FromResult("success");
        }
    }
}
