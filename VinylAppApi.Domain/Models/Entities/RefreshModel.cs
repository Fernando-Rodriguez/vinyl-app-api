using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using VinylAppApi.Domain.Entities;

namespace VinylAppApi.Domain.Models.Entities
{
    [BsonCollection("refresh-tokens")]
    public class RefreshModel : Document
    {
        [BsonElement("refresh-token")]
        [Required]
        public string Refresh { get; set; }

        [BsonElement("user_id")]
        [Required]
        public string UserId { get; set; }
    }
}
