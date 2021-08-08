using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VinylApp.Infrastructure.Repository.MongoModels
{
    [BsonCollection("groups")]
    public class MongoGroup : Document
    {
        [BsonElement("name")]
        [Required]
        public string Name { get; set; }

        [BsonElement("users")]
        [Required]
        public List<ObjectId> Users { get; set; }
    }
}
