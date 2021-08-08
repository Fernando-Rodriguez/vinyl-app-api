using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VinylApp.Infrastructure.Repository.MongoModels
{
    [BsonCollection("users")]
    public class User : Document
    {
        [BsonElement("name")]
        [Required]
        public string Name { get; set; }

        [BsonElement("auth")]
        [Required]
        public Auth Auth { get; set; }

        [BsonElement("owned_albums")]
        [Required]
        public List<Album> OwnedAlbums { get; set; }

        [BsonElement("groups")]
        [Required]
        public List<Group> Groups { get; set; }
    }
}

