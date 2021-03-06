﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VinylAppApi.Domain.Entities
{
    /// <summary>
    /// Base description of a Bson document that all others will
    /// implement
    /// </summary>
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedAt { get; }

        public string IdString { get; }
    }
}
