using System;
using System.Collections.Generic;
using System.Text;
using Orleans.Concurrency;

namespace JF.Identity.Grain
{
    [Immutable]
    public class SignUpCommand
    {
        public SignUpCommand(string email, string passwordHash, string nickname)
        {
            Email = email;
            PasswordHash = passwordHash;
            Nickname = nickname;
        }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Nickname { get; private set; }
    }
}
