using JF.Domain.SeedWork;
using System;

namespace JF.Identity.Domain.AggregatesModel.UserAggregate
{
    public class User: Entity
    {
        public User()
        { }

        public User(Guid id)
        {
            this.Id = id;
        }

        #region Base Identity
        public Guid Id { get; set; }
        public string PasswordHash { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTimeOffset BanEnd { get; set; }
        public DateTimeOffset LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenEnd { get; set; }
        public string SecurityStamp { get; set; }
        #endregion

        public string Token { get; set; }
        public DateTimeOffset TokenEnd { get; set; }

    }
}
