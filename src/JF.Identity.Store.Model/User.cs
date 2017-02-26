using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace JF.Identity.Store.Model
{
    public class User
    {

        public User()
        {
            //this.Id = Guid.NewGuid();
        }

        #region Base Identity
        public Guid Id { get; set; }

        public string PasswordHash { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public ICollection<Role> Roles { get; } = new List<Role>();
        public ICollection<Claim> Claims { get; } = new List<Claim>();
        public DateTimeOffset? BanEnd { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenEnd { get; set; }
        public string SecurityStamp { get; set; }
        #endregion

        public string Token { get; set; }
        public DateTimeOffset TokenEnd { get; set; }

    }
}
