namespace VinylApp.Domain.DTOs
{
    public class AlbumUpdateModelDTO
    {
        public string Id { get; set; }
        public string User { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public int Rating { get; set; }
    }
}
