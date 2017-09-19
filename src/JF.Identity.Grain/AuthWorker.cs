using System;
using System.Threading.Tasks;
using JF.Identity.Domain.Command;
using JF.Identity.Grain.Commands;
using Microsoft.EntityFrameworkCore;
using Orleans;
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
        public async Task<CommandResult> HandleAsync(SignUpCommand cmd)
        {
            if(await _context.Users.AnyAsync(u=>u.Email == cmd.Email))
            {
                return new CommandResult("Email exists");
            }

            var user = new User();
            user.Email = cmd.Email;
            
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            var userGrain = GrainFactory.GetGrain<IUserGrain>(user.Id);
            userGrain.InvokeOneWay(_ => _.BeginSignUpAsync(cmd.Password));
            return CommandResult.Ok;
        }

        public new virtual IGrainFactory GrainFactory
        {
            get { return base.GrainFactory; }
        }
    }
}
