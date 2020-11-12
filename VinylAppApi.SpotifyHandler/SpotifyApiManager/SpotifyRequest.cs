using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VinylAppApi.Shared.Models.SpotifyModels;
using VinylAppApi.Shared.Models.SpotifyModels.AlbumModels;

namespace VinylAppApi.SpotifyHandler.SpotifyApiManager
{
    public class SpotifyRequest : ISpotifyRequest
    {
        private ITokenManager _tokenManager;
        public AlbumModel QueryModel { get; set; }

        public SpotifyRequest(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
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
            if (String.IsNullOrEmpty(_tokenManager._pToken))
            {
                var x = 0;

                while (x < 1)
                {
                    await _tokenManager.TestContact();
                    x++;
                }
            }

            var response = await Search(searchAlbum);

            if (response.IsSuccessStatusCode)
            {
                return QueryModel;
            }
            else
            {
                var x = 0;
                while (x < 1)
                {
                    await _tokenManager.TestContact();
                    await Search(searchAlbum);
                    x++;
                }

                return new SpotifyErrorModel()
                    .CreateAlbumError();
            }
        }
    }
}


