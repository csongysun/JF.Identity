using System;
using System.Threading.Tasks;
using JF.Identity.Service;
using Orleans;

namespace JF.Identity.Grain
{
    public class UserGrain: Orleans.Grain, IUserGrain
    {
        private User _state;
        private readonly IdentityContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserGrain(
            IdentityContext context,
            IPasswordHasher passwordHasher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
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

        public async Task BeginSignUpAsync(string password)
        {
            _state.CreatedDate = DateTimeOffset.Now;
            _state.PasswordHash = _passwordHasher.HashPassword(password);
            await _context.SaveChangesAsync();
        }
    }
}
