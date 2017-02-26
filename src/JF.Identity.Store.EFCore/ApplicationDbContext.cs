using CSYS.Identity.Store.EFCore;
using JF.Identity.Store.Model;
using Microsoft.EntityFrameworkCore;

namespace JF.Identity.Store.EFCore
{
    public class ApplicationDbContext : IdentityDbContext<User, Role>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
