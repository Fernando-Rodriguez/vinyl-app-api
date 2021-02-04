using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VinylAppApi.Shared.Models.SpotifyModels;
using VinylAppApi.Shared.Models.SpotifyModels.AlbumModels;

namespace VinylAppApi.SpotifyHandler.SpotifyApiManager
{
    public class SpotifyRequest : ISpotifyRequest
    {
        private ITokenManager _tokenManager;
        private ILogger _logger;
        public AlbumModel QueryModel { get; set; }

        public SpotifyRequest(ITokenManager tokenManager, ILogger<SpotifyRequest> logger)
        {
            _tokenManager = tokenManager;
            _logger = logger;
        }

        private async Task<HttpResponseMessage> Search(string searchAlbum)
        {
            string searchType = "album";

            var _client = new HttpClient();
            var builder = new UriBuilder("https://api.spotify.com/v1/search");

            builder.Query = $"q={searchAlbum}&type={searchType}&limit=5";

            string baseUrl = builder.ToString();

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, baseUrl);

            requestMessage.Headers.Add("Authorization", $"Bearer {_tokenManager._pToken}");

            var response = await _client.SendAsync(requestMessage);

            var albumJson = await response.Content.ReadAsStringAsync();

            var albumDeJsoned = JsonConvert.DeserializeObject<AlbumModel>(albumJson);

            QueryModel = albumDeJsoned;

            return response;

        }

        public async Task<AlbumModel> SpotifySearchManager(string searchAlbum)
        {
            if (string.IsNullOrEmpty(_tokenManager._pToken))
            {
                try
                {
                    await _tokenManager.TestContact();
                }
                catch (Exception e)
                {
                    _logger.LogDebug($"Error {e} at Token Gen.");
                }

            }

            var response = await Search(searchAlbum);

            if (response.IsSuccessStatusCode)
            {
                return QueryModel;
            }
            else
            {
                _logger.LogDebug($"Error with the token");
                
                return new SpotifyErrorModel()
                    .CreateAlbumError();
            }
        }
    }
}


