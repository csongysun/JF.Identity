using CSYS.Identity.Store;
using Identity.Store.Model;
using System;
using System.Collections.Generic;
using System.Text;
using CSYS.Identity;
using CSYS.Identity.Store.EFCore;

namespace JF.Identity.Store.EFCore
{
    public class UserStore : UserStore<User, Role, ApplicationDbContext>, IUserStore
    {
        public UserStore(ApplicationDbContext context,
            IdentityErrorDescriber errorDescriber = null) : base(context, errorDescriber)
        {
        }
    }
}
