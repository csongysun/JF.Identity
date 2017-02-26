using JF.Identity.Store.Model;
using System;
using System.Collections.Generic;
using System.Text;
using CSYS.Identity;
using CSYS.Identity.Store.EFCore;

namespace JF.Identity.Store.EFCore
{
    public class RoleStore : RoleStore<Role, ApplicationDbContext>, IRoleStore
    {
        public RoleStore(ApplicationDbContext context,
            IdentityErrorDescriber errorDescriber = null) : base(context, errorDescriber)
        {
        }
    }
}
