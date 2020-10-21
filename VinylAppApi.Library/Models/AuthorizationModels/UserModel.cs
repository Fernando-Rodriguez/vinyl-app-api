using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VinylAppApi.Library.Models.AuthorizationModels
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

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
