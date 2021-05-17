using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VinylAppApi.Domain.Entities;

namespace VinylAppApi.Domain.Repository
{
    /// <summary>
    /// Investigation implementation of Mongo Repository Pattern.
    /// Currently working through generic model taken from
    /// https://medium.com/@marekzyla95/mongo-repository-pattern-700986454a0e
    /// TODO: use information learned to build new repo model closer to
    /// what is used with EF Core.
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public interface IMongoRepo<TDocument> where TDocument : IDocument
    {
        IQueryable<TDocument> AsQueryable();

        Task<List<TDocument>> FilterByAsync(
            Expression<Func<TDocument, bool>> filter);

        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filter);

        Task<TDocument> FindByIdAsync(string id);

        Task InsertOneAsync(TDocument document);

        Task InsertManyAsync(ICollection<TDocument> documents);

        Task ReplaceOneAsync(TDocument document);

        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filter);

        Task DeleteByIdAsync(string id);

        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filter);
    }
}