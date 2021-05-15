using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using VinylAppApi.DataAccess.Entities;

namespace VinylAppApi.DataAccess.Repository
{
    public class MongoRepo<TDocument> : IMongoRepo<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;
        private readonly IConfiguration _config;
        private readonly ILogger<MongoRepo<TDocument>> _logger;

        public MongoRepo(ILogger<MongoRepo<TDocument>> logger, IConfiguration config)
        {
            _config = config;
            _logger = logger;

            var dbClient = new MongoClient(_config.GetConnectionString("MongoDb"));
            var db = dbClient.GetDatabase("vinyl-db");

            _collection = db.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public void DeleteById(string id)
        {
            var tempObId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(document => document.Id, tempObId);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            throw new NotImplementedException();
        }

        public TDocument FindById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TDocument> FindByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            throw new NotImplementedException();
        }

        public Task InsertManyAsync(ICollection<TDocument> documents)
        {
            throw new NotImplementedException();
        }

        public void InsertOne(TDocument document)
        {
            throw new NotImplementedException();
        }

        public Task InsertOneAsync(TDocument document)
        {
            throw new NotImplementedException();
        }

        public void ReplaceOne(TDocument document)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceOneAsync(TDocument document)
        {
            throw new NotImplementedException();
        }
    }
}
