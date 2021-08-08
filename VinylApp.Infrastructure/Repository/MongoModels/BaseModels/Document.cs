﻿using System;
using MongoDB.Bson;

namespace VinylApp.Infrastructure.Repository.MongoModels
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => DateTime.UtcNow;

        public string IdString
        {
            get
            {
                return Id.ToString();
            }
        }
    }
}
