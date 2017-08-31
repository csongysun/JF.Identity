namespace JF.Identity.Interfaces
{
    public class SignUpCommand
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
