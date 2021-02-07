namespace VinylAppApi.Shared.Models.DbModels
{
    public class OwnedAlbumModelDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string User { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public int Rating { get; set; }
    }
}
