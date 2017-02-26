using CSYS.Identity.Store.EFCore;
using Identity.Store.Model;
using System;

namespace JF.Identity.Store.EFCore
{
    public class ApplicationDbContext: IdentityDbContext<User, Role>
    {
    }
}
