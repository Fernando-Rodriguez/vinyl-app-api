using System;
using System.Collections.Generic;

namespace VinylAppApi.Library.Models.SpotifyModels.AlbumModels
{
    public class Albums
    {
        public string href { get; set; }
        public List<Item> items { get; set; } = new List<Item>();
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }
}
