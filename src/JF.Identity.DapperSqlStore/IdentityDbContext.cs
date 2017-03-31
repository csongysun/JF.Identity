using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace JF.Identity.DapperSqlStore
{
    public class IdentityDbContext
    {
        private readonly string _connStr;
        public IdentityDbContext(string connStr)
        {
            _connStr = new MySqlConnectionStringBuilder(connStr)
            {
                AllowZeroDateTime = true,
                ConvertZeroDateTime = true,
                MaximumPoolSize = 1000,
                Pooling = true
            }.ConnectionString;
        }
        public IDbConnection Connection => new MySqlConnection(_connStr);

    }
}
