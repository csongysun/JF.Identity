using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JF.Identity.Grain.Auth.Model;
using JF.Identity.Grain.Commands;
using Microsoft.EntityFrameworkCore;

namespace JF.Identity.Grain
{
    public class SignInWorker: Orleans.Grain, ISignInWorker
    {
        private readonly IdentityContext _context;
        public SignInWorker(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<SignInResult> HandleAsync(SignInCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == command.Email);
            if (user == null) return SignInResult.Error("UserNotFound");
            var userGrain = GrainFactory.GetGrain<IUserGrain>(user.Id);
            throw new NotImplementedException();
        }
    }
}
