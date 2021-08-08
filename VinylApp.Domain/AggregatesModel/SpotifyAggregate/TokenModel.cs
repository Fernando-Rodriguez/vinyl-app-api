using System;
using Newtonsoft.Json;

namespace VinylApp.Domain.AggregatesModel.SpotifyAggregate
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TokenModel
    {
        public TokenModel()
        {
            DateRequested = DateTime.UtcNow;
        }

        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        public DateTime DateRequested { get; }
        public DateTime ExpirationDate => DateRequested.AddSeconds(ExpiresIn);
    }
}