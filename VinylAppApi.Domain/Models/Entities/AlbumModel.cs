using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VinylAppApi.Domain.Entities
{
    [BsonCollection("albumss")]
    public class AlbumModel : Document
    {
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
