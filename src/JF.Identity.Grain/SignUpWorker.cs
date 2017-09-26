using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JF.Domain.Command;
using JF.Identity.Domain.AggregatesModel.UserAggregate;
using JF.Identity.Grain.Commands;
using JF.Identity.Service;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Concurrency;

namespace JF.Identity.Grain
{
    [StatelessWorker]
    public class SignUpWorker: Orleans.Grain, ISignUpWorker
    {
        private readonly IdentityContext _context;
        private readonly IPasswordHasher _passwordHasher;
        public SignUpWorker(
            IdentityContext context,
            IPasswordHasher passwordHasher
            )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }
        public async Task<CommandResult> HandleAsync(SignUpCommand cmd)
        {
            if(await _context.Users.AnyAsync(u=>u.Email == cmd.Email))
            {
                return CommandResult.Error("Email exists");
            }

            var user = new User();
            user.Email = cmd.Email;
            
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            this.InvokeOneWay(_=>BeginSignUpAsync(user.Id, cmd));

            return CommandResult.Ok;
        }

        public async Task BeginSignUpAsync(long UserId, SignUpCommand cmd)
        {
            var userGrain = GrainFactory.GetGrain<IUserGrain>(UserId);

            var taskList = new List<Task>();
            taskList.Add(userGrain.SignUpAsync());
            var newPassword = _passwordHasher.HashPassword(cmd.Password);
            taskList.Add(userGrain.UpdatePasswordAsync(newPassword));

            await Task.WhenAll(taskList);
            await userGrain.SaveChangesAsync();
        }

        public new virtual IGrainFactory GrainFactory
        {
            get { return base.GrainFactory; }
        }
    }
}
