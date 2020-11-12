using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            string baseAddress = @"https://accounts.spotify.com/api/token";

            string clientId = _config.GetSection("Client_Id").Value.ToString();
            string clientSecret = _config.GetSection("Client_Secret").Value.ToString();
            string credentials = $"{clientId}:{clientSecret}";

            using (var client = new HttpClient())
            {
                //set main request headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

                //set body content
                List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
                requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);

                //create request
                var request = await client.PostAsync(baseAddress, requestBody);
                var response = await request.Content.ReadAsStringAsync();

                //decode the response
                SpotifyToken tok = JsonConvert.DeserializeObject<SpotifyToken>(response);

                _pToken = tok.access_token;
            }

            _logger.LogDebug("<---------------------Refreshed Token--------------------->");

        }
    }
}
