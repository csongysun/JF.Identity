using System;
using System.Collections.Generic;
using System.Text;
using JF.Domain.SeedWork;

namespace JF.Identity.Domain.AggregatesModel.UserAggregate
{
    public class User: Entity<long>, IAggregateRoot
    {
        public string PasswordHash { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }

}
