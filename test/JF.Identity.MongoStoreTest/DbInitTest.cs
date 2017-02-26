using JF.Identity.MongoStore;
using JF.Identity.Store.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JF.Identity.MongoStoreTest
{
    public class DbInitTest
    {

        [Fact]
        public async Task InitUserEmailIndex_Test()
        {
            var db = TestDbContext.DbContext.Identity;
            var userSet = db.GetCollection<User>("Users");
            //await userSet.Indexes.DropAllAsync();

            var emailIndex = (await userSet.Indexes.ListAsync()).ToEnumerable().FirstOrDefault(v => v.Elements.Any(e => e.Name == "name" && e.Value == "EmailIndex"));
            Assert.NotNull(emailIndex);
        }


    }
}
