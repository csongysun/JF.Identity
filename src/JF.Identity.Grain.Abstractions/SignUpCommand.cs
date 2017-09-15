using System;
using System.Collections.Generic;
using System.Text;
using Orleans.Concurrency;

namespace JF.Identity.Grain
{
    [Immutable]
    public class SignUpCommand
    {
        public SignUpCommand(string email, string password, string nickname)
        {
            Email = email;
            Password = password;
            Nickname = nickname;
        }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Nickname { get; private set; }
    }
}
