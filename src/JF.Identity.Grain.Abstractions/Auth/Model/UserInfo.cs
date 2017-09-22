using System;
using System.Collections.Generic;
using System.Text;
using JF.Identity.Domain.AggregatesModel.UserAggregate;

namespace JF.Identity.Grain.Auth.Model
{
    public class UserInfo
    {
        public UserInfo(User user)
        {
            UserId = user.Id;
            Email = user.Email;
            Nickname = user.Nickname;
        }

        public long UserId { get; set; }
        public string Email { get; private set; }
        public string Nickname { get; private set; }
    }
}
