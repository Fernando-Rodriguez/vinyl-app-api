using System;
using MongoDB.Bson;

namespace VinylAppApi.DataAccess.Entities
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => DateTime.UtcNow;
    }
}
