using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VinylAppApi.Shared.Models.DbModels
{
    public class GroupModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("group_name")]
        [Required]
        public string GroupName { get; set; }

        [BsonElement("users")]
        [Required]
        public List<string> Users { get; set; }
    }
}