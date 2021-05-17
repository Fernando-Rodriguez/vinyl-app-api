using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace VinylAppApi.Domain.Entities
{
    [BsonCollection("user-groups")]
    public class GroupModel : Document
    {
        [BsonElement("group_name")]
        [Required]
        public string GroupName { get; set; }

        [BsonElement("users")]
        [Required]
        public List<string> Users { get; set; }
    }
}
