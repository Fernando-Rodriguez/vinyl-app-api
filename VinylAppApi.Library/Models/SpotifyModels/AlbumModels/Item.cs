using System;
using System.Collections.Generic;

namespace VinylAppApi.Library.Models.SpotifyModels.AlbumModels
{
    public class Item
    {
        public string album_type { get; set; }
        public List<Artist> artists { get; set; } = new List<Artist>();
        public ExternalUrls external_urls { get; set; } = new ExternalUrls();
        public string href { get; set; }
        public string id { get; set; }
        public List<Image> images { get; set; } = new List<Image>();
        public string name { get; set; }
        public string release_date { get; set; }
        public string release_date_precision { get; set; }
        public int total_tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}
