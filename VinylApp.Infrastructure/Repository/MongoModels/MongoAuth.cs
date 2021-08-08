using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VinylApp.Infrastructure.Repository.MongoModels
{
    public class MongoAuth
    {
        [BsonElement("auth_name")]
        [Required]
        public string UserName { get; set; }

        [BsonElement("auth_secret")]
        [Required]
        public string UserSecret { get; set; }

        [BsonElement("role")]
        [Required]
        public string Role { get; set; }
    }
}
