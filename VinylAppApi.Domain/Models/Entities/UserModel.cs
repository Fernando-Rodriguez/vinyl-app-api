namespace VinylAppApi.Domain.Entities
{
    [BsonCollection("users")]
    public class UserModel : Document
    {
        public string UserName { get; set; }
        public string UserSecret { get; set; }
        public string UserRole { get; set; } = "user";
    }
}
