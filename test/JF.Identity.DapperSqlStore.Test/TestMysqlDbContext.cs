using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JF.Identity.DapperSqlStore.Test
{
    public class TestMysqlDbContext
    {

        public static IdentityDbContext Context = new IdentityDbContext("User ID=postgres;Password=123456;Host=csys.me;Port=5432;Database=jf_identity_test;");

        public static async Task CleanTable(string tableName)
        {
            using (var db = Context.Connection)
            {
                db.Open();
                var err = await db.ExecuteAsync($"TRUNCATE {tableName}");
                //var p = new DynamicParameters();
                //p.Add("@table", tableName);
                //var err = await db.ExecuteAsync(@"TRUNCATE @table", new { table = tableName });
                // 
            }
        }

    }
}
