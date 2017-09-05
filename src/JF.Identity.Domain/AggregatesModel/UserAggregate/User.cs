using JF.Domain.SeedWork;
using System;
using JF.Identity.Domain.Events;

namespace JF.Identity.Domain.AggregatesModel.UserAggregate
{
    public class User: Entity
    {
        #region properties

        public Guid Id { get; private set; }
        public string PasswordHash { get; private set; }
        public string Nickname { get; private set; }
        public string Email { get; private set; }
        public bool EmailConfirmed { get; private set; } = false;
        public DateTimeOffset BanEnd { get; private set; }
        public DateTimeOffset LockoutEnd { get; private set; }
        public int AccessFailedCount { get; private set; } = 0;
        public string RefreshToken { get; private set; } = null;
        public DateTimeOffset RefreshTokenEnd { get; private set; }
        public string SecurityStamp { get; private set; }

        #endregion


        public static User Create(string email, string passworldHash, string nickname)
        {
            var user = new User();
            user.Id = Guid.NewGuid();
            user.Email = email;
            user.PasswordHash = passworldHash;
            user.Nickname = user.Nickname;

            user.SecurityStamp = Guid.NewGuid().ToString();
            user.AddDomainEvent(new UserCreatedEvent(user));

            return user;
        }
    }
}
