namespace VinylAppApi.DataAccess.Entities
{
    [BsonCollection("users")]
    public class User : Document
    {
        public string UserName { get; set; }
        public string UserSecret { get; set; }
        public string UserRole { get; set; }
    }
}
