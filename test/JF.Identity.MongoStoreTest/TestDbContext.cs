using JF.Identity.Store.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JF.Identity.MongoStore
{
    /**
     *  It is recommended to store a MongoClient instance in a global place, either as a static variable or in an IoC container with a singleton lifetime.
     */
    /// <summary>
    /// The database context of identity.
    /// </summary>
    public static class TestDbContext
    {
        public static IdentityDbContext DbContext = new IdentityDbContext("localhost;identity_test;");

    }
}
