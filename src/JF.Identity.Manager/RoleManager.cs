using CSYS.Identity;
using JF.Identity.Store;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace JF.Identity.Manager
{
    public class RoleManager : RoleManager<Role>
    {
        private readonly HttpContext _context;

        public RoleManager(IRoleStore store, 
            ILogger<RoleManager> logger, 
            IHttpContextAccessor contextAccessor,
            IdentityErrorDescriber errorDescriber) : base(store, logger, errorDescriber)
        {
            _context = contextAccessor.HttpContext;
            CancellationToken = _context?.RequestAborted ?? default(CancellationToken);
        }
    }
}
