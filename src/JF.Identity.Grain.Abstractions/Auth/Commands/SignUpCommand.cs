using JF.Domain.Command;
using Orleans.Concurrency;

namespace JF.Identity.Grain.Commands
{
    [Immutable]
    public class SignUpCommand: ICommand
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
