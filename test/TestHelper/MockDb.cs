using System;
using JF.Identity.Grain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TestHelper
{
    public static class MockDb
    {
        private static IdentityContext _context;
        public static IdentityContext Sqlite
        {
            get
            {
                if (_context is null)
                {
                    var lf = new LoggerFactory();
                    lf.AddProvider(new SqlLoggerProvider());
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();
                    var options = new DbContextOptionsBuilder<IdentityContext>()
                                        .UseSqlite(connection)
                                        .UseLoggerFactory(lf)
                                        .Options;

                    var context = new IdentityContext(options);
                    context.Database.EnsureCreated();
                    _context = context;
                }

                return _context;
            }
        }
    }
}
