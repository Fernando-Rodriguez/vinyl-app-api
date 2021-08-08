using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VinylApp.Domain.AggregatesModel.SpotifyAggregate;

namespace VinylApp.Infrastructure.ExternalServices.SpotifyRequests
{
    public class TokenRequest : ITokenRequest
    {
        private readonly IConfiguration _config;
        private readonly ILogger<TokenRequest> _logger;
        private readonly HttpClient _client;
        private FormUrlEncodedContent _encodedBody;

        public TokenRequest(IConfiguration config, ILogger<TokenRequest> logger)
        {
            _config = config;
            _logger = logger;
            _client = new HttpClient();
        }

        private string ClientId => _config.GetSection("Client_Id").Value.ToString();
        private string ClientSecret => _config.GetSection("Client_Secret").Value.ToString();
        private string Credentials => $"{ClientId}:{ClientSecret}";
        private string SpotifyTokenURL => _config.GetSection("SpotifyLoginURL").Value.ToString();

        public async Task<TokenModel> RequestToken()
        {
            using (_client)
            {
                SetHeaders();
                AssignBodyContent();

                var request = await _client.PostAsync(SpotifyTokenURL, _encodedBody);
                var response = await request.Content.ReadAsStringAsync();

                TokenModel token = JsonConvert.DeserializeObject<TokenModel>(response);
                return token;
            }
        }

        public void SetHeaders()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(Credentials)));
        }

        public void AssignBodyContent()
        {
            _encodedBody = null;
            List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };
            _encodedBody = new FormUrlEncodedContent(requestData);
        }
    }
}
