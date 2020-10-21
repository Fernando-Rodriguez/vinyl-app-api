using System.Text.Json.Serialization;

namespace VinylAppApi.Library.SpotifyModels
{
    public class SpotifyToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }
    }
}