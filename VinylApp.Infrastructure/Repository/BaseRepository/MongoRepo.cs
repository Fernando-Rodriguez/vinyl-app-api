﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using VinylApp.Infrastructure.Repository.MongoModels;

namespace VinylApp.Infrastructure.Repository.BaseRepository
{
    public class MongoRepo<TDocument> : IMongoRepo<TDocument> where TDocument : IDocument
    {
        protected readonly IMongoCollection<TDocument> _collection;
        protected readonly IConfiguration _config;
        protected readonly ILogger<MongoRepo<TDocument>> _logger;

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
            var collectionName = documentType
                .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault() as BsonCollectionAttribute;

            return collectionName.CollectionName;
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual async Task DeleteByIdAsync(string id)
        {
            var tempObId = new ObjectId(id);
            await _collection
                .FindOneAndDeleteAsync(
                    Builders<TDocument>.Filter.Eq(dbDoc => dbDoc.Id, tempObId)
                );
        }

        public virtual async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filter)
        {
            await _collection.DeleteManyAsync<TDocument>(filter);
        }

        public virtual async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filter)
        {
            await _collection.DeleteOneAsync<TDocument>(filter);
        }

        public virtual async Task<List<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filter)
        {
            var result = await _collection.FindAsync<TDocument>(filter);
            return result.ToList();
        }

        public virtual async Task<TDocument> FindByIdAsync(string id)
        {
            var tempObId = new ObjectId(id);
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

        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public virtual async Task InsertOneAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            await _collection
                .ReplaceOneAsync(
                    Builders<TDocument>.Filter.Eq(dbDoc => dbDoc.Id, document.Id), document
                );
        }
    }
}
