using System;
using System.Data;
using System.Linq;
using Npgsql;

namespace JF.Identity.DapperSqlStore
{
    public class IdentityDbContext
    {
        private readonly string _connStr;
        public IdentityDbContext(string connStr)
        {
            _connStr = new NpgsqlConnectionStringBuilder(connStr)
            {
                //AllowZeroDateTime = true,
                //ConvertZeroDateTime = true,
                MinPoolSize = 0,
                MaxPoolSize = 100,
                Pooling = true,
                //ConnectionTimeout = 10,
                //DefaultCommandTimeout = 10,
            }.ConnectionString;
        }
        public IDbConnection Connection => new NpgsqlConnection(_connStr);

    }
}
