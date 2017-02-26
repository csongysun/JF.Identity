using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace JF.Identity.Store.Model
{
    public class Role
    {
        public Role() { }

        public Role(string roleName, int OrderId) : this()
        {
            Name = roleName;
        }

        public ICollection<Claim> Claims { get; } = new List<Claim>();

        public int Id { get; set; }

        public string Name { get; set; }

    }
}
