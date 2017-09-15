using System;
using System.Threading.Tasks;
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
        public async Task<CommandResult> SignUpAsync(SignUpCommand cmd)
        {
            if(await _context.Users.AnyAsync(u=>u.Email == cmd.Email))
            {
                return new CommandResult("Email exists");
            }

            var user = new User();
            user.Email = cmd.Email;
            
            await _context.AddAsync(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return new CommandResult("failed");
            }
            var userGrain = GrainFactory.GetGrain<IUserGrain>(user.Id);
            userGrain.InvokeOneWay(_ => _.BeginSignUpAsync(cmd.PasswordHash));
            return CommandResult.Ok;
        }

        public new virtual IGrainFactory GrainFactory
        {
            get { return base.GrainFactory; }
        }
    }
}
