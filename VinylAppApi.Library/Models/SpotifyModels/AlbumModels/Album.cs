namespace VinylAppApi.Library.Models.SpotifyModels.AlbumModels
{
    public class Artist
    {
        public ExternalUrls external_urls { get; set; } = new ExternalUrls();
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}
