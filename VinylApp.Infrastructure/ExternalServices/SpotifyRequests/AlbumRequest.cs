using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VinylApi.Domain.DTOs.SpotifyModels.AlbumModels;
using VinylApp.Domain.AggregatesModel.SpotifyAggregate;
using VinylAppApi.Domain.Models.Models;

namespace VinylApp.Infrastructure.ExternalServices.SpotifyRequests
{
    public class AlbumRequest : IAlbumRequest
    {
        private readonly ILogger _logger;
        private TokenModel _token;
        private readonly string _searchType = "album";
        private readonly HttpClient _client;
        private readonly ITokenRequest _tokenRequest;

        public AlbumRequest(TokenModel token, ILogger<AlbumRequest> logger, ITokenRequest tokenRequest)
        {
            _logger = logger;
            _token = token;
            _tokenRequest = tokenRequest;
            _client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.spotify.com/v1/search")
            };
        }

        public async Task<AlbumModel> Search(Album album)
        {
            try
            {
                if (VerifyTokenIsValid(_token)) SetHeaders();
                else
                {
                    _token = await _tokenRequest.RequestToken();
                    SetHeaders();
                }
                var res = await _client.GetAsync(UrlBuilder(album.Name));
                var albumJson = await res.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<AlbumModel>(albumJson);
            }
            catch (HttpRequestException err)
            {
                _logger.LogWarning("Issue retriving album!");
                _logger.LogWarning(err.ToString());
                throw;
            }
            catch (Exception err)
            {
                _logger.LogError("Error making request.");
                _logger.LogError(err.ToString());
                throw;
            }
        }

        private bool VerifyTokenIsValid(TokenModel tokenToTest)
        {
            if (tokenToTest.ExpirationDate < DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }

        private string UrlBuilder(string album)
        {
            var builder = new UriBuilder("https://api.spotify.com/v1/search")
            {
                Query = $"q={album}&type={_searchType}&limit=5"
            };

            return builder.ToString();
        }

        private void SetHeaders()
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token.Token}");
        }
    }
}


