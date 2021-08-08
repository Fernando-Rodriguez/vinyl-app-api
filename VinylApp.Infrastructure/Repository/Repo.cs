using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using VinylApp.Infrastructure.Repository.MongoModels;
using VinylApp.Domain.AggregatesModel.InventoryAggregate;
using VinylApp.Domain.AggregatesModel.UserAggregate;

namespace VinylApp.Infrastructure.Repository
{
    public class InventoryRepo
    {
        protected readonly IMongoCollection<MongoModels.Album> _albumCollection;
        protected readonly IMongoCollection<MongoModels.Group> _groupCollection;
        protected readonly IMongoCollection<MongoModels.User> _userCollection;

        protected readonly IConfiguration _config;
        protected readonly ILogger<InventoryRepo> _logger;

        public InventoryRepo(ILogger<InventoryRepo> logger, IConfiguration config)
        {
            _config = config;
            _logger = logger;

            var dbClient = new MongoClient(_config.GetConnectionString("MongoDb"));
            var db = dbClient.GetDatabase("vinyl-db");

            _albumCollection = db.GetCollection<MongoModels.Album>("albums");
            _groupCollection = db.GetCollection<MongoModels.Group>("groups");
            _userCollection = db.GetCollection<MongoModels.User>("users");
        }

        public virtual async Task<Inventory> FindByUserIdAsync()
        {
            var tempObId = new ObjectId();
            var result = await _collection
                .FindAsync<TDocument>(
                    Builders<TDocument>.Filter.Eq(dbDoc => dbDoc.Id, tempObId)
                );
            return result.FirstOrDefault();
        }

        public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter)
        {
            var result = await _collection.FindAsync<TDocument>(filter);
            return result.FirstOrDefault();
        }


    }
}
