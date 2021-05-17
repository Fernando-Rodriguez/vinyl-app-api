using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace VinylAppApi.Domain.Entities
{
    [BsonCollection("users")]
    public class UserModel : Document
    {
        [BsonElement("user_name")]
        [Required]
        public string UserName { get; set; }

        [BsonElement("user_secret")]
        [Required]
        public string UserSecret { get; set; }

        [BsonElement("user_role")]
        [Required]
        public string UserRole { get; set; }
    }
}
