using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace VinylApp.Infrastructure.Repository.MongoModels
{
    [BsonCollection("albums")]
    public class MongoAlbum : Document
    {
        [BsonElement("album")]
        [Required]
        public string AlbumName { get; set; }

        [BsonElement("artist")]
        [Required]
        public string Artist { get; set; }

        [BsonElement("image_url")]
        public string ImageUrl { get; set; }
    }
}
