using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VinylAppApi.Library.DbModels
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
    }
}