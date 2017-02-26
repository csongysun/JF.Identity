using CSYS.Identity;
using JF.Identity.Store.Model;
using System;
using System.Collections.Generic;
using System.Text;
using CSYS.Identity.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using JF.Identity.Store;

namespace JF.Identity.Manager
{
    public class RoleManager : RoleManager<Role>
    {
        public RoleManager(IRoleStore store, 
            ILogger<RoleManager> logger, 
            IHttpContextAccessor contextAccessor,
            IdentityErrorDescriber errorDescriber) : base(store, logger, contextAccessor, errorDescriber)
        {
        }
    }
}
