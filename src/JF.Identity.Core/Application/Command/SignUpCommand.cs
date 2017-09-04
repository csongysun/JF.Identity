using System;
using System.Collections.Generic;
using System.Text;
using JF.Common;
using JF.Domain.Command;

namespace JF.Identity.Core.Application.Command
{
    public class SignUpCommand: ICommand
    {
        public SignUpCommand(
            string email,
            string phone,
            string password
            )
        {
            Email = email;
            Phone = phone;
            Password = password;
        }

        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Password { get; private set; }

    }
}
