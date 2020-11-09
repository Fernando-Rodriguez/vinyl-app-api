using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VinylAppApi.Shared.Models.SpotifyModels;

namespace VinylAppApi.SpotifyHandler.SpotifyApiManager
{
    public class TokenManager : ITokenManager
    {
        public string _pToken { get; set; }
        private readonly IConfiguration _config;
        private ILogger<TokenManager> _logger;

        public TokenManager(IConfiguration config, ILogger<TokenManager> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task TestContact()
        {
            HttpClient client = new HttpClient();

            string baseAddress = @"https://accounts.spotify.com/api/token";

            string grant_type = "client_credentials";
            string client_id = _config.GetSection("Credentials").GetSection("Client_id").ToString();
            string client_secret = _config.GetSection("Credentials").GetSection("Client_secret").ToString();

            var form = new Dictionary<string, string>
                {
                    {"grant_type", grant_type},
                    {"client_id", client_id},
                    {"client_secret", client_secret},
                };

            HttpResponseMessage tokenResponse = await client.PostAsync(baseAddress, new FormUrlEncodedContent(form));
            var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
            SpotifyToken tok = JsonConvert.DeserializeObject<SpotifyToken>(jsonContent);

            _pToken = tok.access_token;

            _logger.LogInformation("<---------------------Refreshed Token--------------------->");
        }
    }
}
