using System;
using System.Threading.Tasks;
using JF.Identity.Grain.Abstractions;
using Orleans;

namespace JF.Identity.Grain
{
    public class UserGrain: Orleans.Grain, IUserGrain
    {
        private User _state;
        private readonly IdentityContext _context;

        public UserGrain(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public override async Task OnActivateAsync()
        {
            _state = await _context.Users.FindAsync(this.GetPrimaryKeyLong())
                ?? throw new EntityNotFoundException();
            
            await base.OnActivateAsync();
        }

        public override async Task OnDeactivateAsync()
        {
            await _context.SaveChangesAsync();
            await base.OnDeactivateAsync();
        }

        public async Task<CommandResult> SignUpAsync()
        {
            _state.CreatedDate = DateTimeOffset.Now;
            await _context.SaveChangesAsync();
            return CommandResult.Ok;
        }
    }
}
