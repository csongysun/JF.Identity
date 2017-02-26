using JF.Identity.Store.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Security.Claims;
using System.Linq;

namespace JF.Identity.MongoStore
{
    /**
     *  It is recommended to store a MongoClient instance in a global place, either as a static variable or in an IoC container with a singleton lifetime.
     */
    /// <summary>
    /// The database context of identity.
    /// </summary>
    public class IdentityDbContext
    {
        public IdentityDbContext(string connStr)
        {
            var args = connStr.Split(';');
            this.Client = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(args[0].Replace("mongodb://", "")),
                ConnectTimeout = TimeSpan.FromSeconds(6)
            });
            this.Identity = this.Client.GetDatabase(args[1]);

            MapInit();

        }

        /**
         * 
         */
        /// <summary>
        /// The database instance.
        /// </summary>
        public IMongoDatabase Identity { get; private set; }
        public MongoClient Client { get; private set; }

        public void MapInit()
        {

            if (BsonClassMap.IsClassMapRegistered(typeof(User)))
                return;

            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();

                cm.UnmapMember(u => u.Token);
                cm.UnmapMember(u => u.TokenEnd);
                cm.UnmapMember(u => u.RefreshToken);
                cm.UnmapMember(u => u.RefreshTokenEnd);

                cm.MapIdMember(c => c.Id).SetIdGenerator(AscendingGuidGenerator.Instance);

                cm.GetMemberMap(u => u.Email).SetIsRequired(true);
            });

            BsonClassMap.RegisterClassMap<Role>(cm =>
            {
                cm.AutoMap();
            });

            BsonClassMap.RegisterClassMap<Claim>(cm =>
            {
                cm.MapMember(c => c.Type);
                cm.MapMember(c => c.Value);
            });

            var userSet = Identity.GetCollection<User>("Users");
            if (!userSet.Indexes.List().ToEnumerable().Any(v => v.Elements.Any(e => e.Name == "name" && e.Value == "EmailIndex")))
            {
                userSet.Indexes.CreateOne(new IndexKeysDefinitionBuilder<User>().Ascending(u => u.Email), new CreateIndexOptions
                {
                    Name = "EmailIndex",
                    Unique = true,
                });
            }

        }
    }
}
