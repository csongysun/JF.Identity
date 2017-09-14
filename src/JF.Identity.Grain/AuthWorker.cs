using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JF.Identity.Grain.Abstractions;
using Orleans.Concurrency;

namespace JF.Identity.Grain
{
    [StatelessWorker]
    public class AuthWorker: Orleans.Grain, IAuthWorker
    {
        private readonly IdentityContext _context;
        public AuthWorker(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<CommandResult> SignUpAsync(SignUpCommand cmd)
        {
            var user = new User();
            user.Email = cmd.Email;
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            var userGrain = GrainFactory.GetGrain<IUserGrain>(user.Id);

            return await userGrain.SignUpAsync();
        }
    }
}
