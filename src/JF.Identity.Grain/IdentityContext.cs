using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace JF.Identity.Grain
{
    public class IdentityContext: DbContext
    {
        public IdentityContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b=>
            {
                b.HasKey(_ => _.Id);
                b.HasAlternateKey(_ => _.Email);
            });
        }
    }
}
