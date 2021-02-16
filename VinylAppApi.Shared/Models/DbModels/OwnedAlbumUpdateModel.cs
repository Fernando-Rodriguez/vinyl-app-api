namespace VinylAppApi.Shared.Models.DbModels
{
    public class OwnedAlbumUpdateModel
    {
        public string User { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public int Rating { get; set; }
    }
}
