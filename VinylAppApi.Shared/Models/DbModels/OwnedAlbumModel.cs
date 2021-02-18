using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// This is the actual model that represents the db.
namespace VinylAppApi.Shared.Models.DbModels
{
    public class OwnedAlbumModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("user")]
        [Required]
        public string User { get; set; }

        [BsonElement("album")]
        [Required]
        public string Album { get; set; }

        [BsonElement("artist")]
        [Required]
        public string Artist { get; set; }
        
        [BsonElement("image_url")] 
        public string ImageUrl { get; set; }

        [BsonElement("rating")]
        public int Rating { get; set; }
    }
}