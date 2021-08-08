using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinylApp.Infrastructure.ExternalServices.SpotifyRequests;
using VinylAppApi.Domain.Models.Models;

namespace VinylApp.Infrastructure.ExternalServices.SpotifyHelper
{
    public class SpotifyHelper
    {
        private readonly IAlbumRequest _spotifyRequest;
        private readonly ILogger<SpotifyHelper> _logger;

        public SpotifyHelper(ILogger<SpotifyHelper> logger, IAlbumRequest spotifyRequest)
        {
            _spotifyRequest = spotifyRequest;
            _logger = logger;
        }

        public async Task<string> RetrieveAlbumUrl(Album album)
        {
            var spotifyRes = await _spotifyRequest.Search(album);

            var spotifyFilter = spotifyRes
                 .albums
                 .items
                 .Where(a => a.artists.First().name.ToLower() == album.Artist.ToLower())
                 .ToList();

            var imageUrl = spotifyFilter
                .Select(a => a.images)
                .FirstOrDefault()
                .FirstOrDefault()
                .url;

            _logger.LogInformation($"Model: {album.Artist} - {album.Name} successfully uploaded.");

            album.SetImageUrl(imageUrl);

            return imageUrl;
        }
    }
}
