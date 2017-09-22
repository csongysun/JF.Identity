using JF.Domain.Command;
using JF.Identity.Grain.Auth.Model;

namespace JF.Identity.Grain.Commands
{
    public class SignInCommand: ICommand<SignInResult>
    {
        public SignInCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
