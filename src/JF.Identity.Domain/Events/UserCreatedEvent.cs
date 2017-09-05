using System;
using System.Collections.Generic;
using System.Text;
using JF.Domain.Event;
using JF.Identity.Domain.AggregatesModel.UserAggregate;

namespace JF.Identity.Domain.Events
{
    public class UserCreatedEvent: IEvent
    {
        public UserCreatedEvent(User user)
        {
        }
        public Guid UserId { get; private set; }
        public string Nickname { get; private set; }
        public string Email { get; private set; }
        public bool EmailConfirmed { get; private set; } = false;
        public DateTimeOffset BanEnd { get; private set; }
        public string RefreshToken { get; private set; } = null;
        public DateTimeOffset RefreshTokenEnd { get; private set; }
        public string SecurityStamp { get; private set; }
    }
}
