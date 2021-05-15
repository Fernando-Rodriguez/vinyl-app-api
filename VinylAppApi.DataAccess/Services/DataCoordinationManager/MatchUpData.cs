using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinylAppApi.Shared.Models.DbModels;
using VinylAppApi.Shared.Models.UserInterfacingModels;
using VinylAppApi.SpotifyHandler.SpotifyApiManager;

namespace VinylAppApi.DataAccess.DataCoordinationManager
{
    public class MatchUpData : IMatchUpData
    {
        private ISpotifyRequest _spotifyRequest;
        private ILogger<MatchUpData> _logger; 

        public MatchUpData(ILogger<MatchUpData> logger, ISpotifyRequest spotifyRequest)
        {
            _spotifyRequest = spotifyRequest;
            _logger = logger;
        }

        public async Task<OwnedAlbumModel> DataMatcher(AlbumUpdateModelDTO albumModelDTO)
        {
            string searchAlbum = albumModelDTO.Album;

            var spotifyRes = await _spotifyRequest.SpotifySearchManager(searchAlbum);

            var mainOutModel = new OwnedAlbumModel();

            var spotifyFilter = spotifyRes
                 .albums
                 .items
                 .Where(a => a.artists.First().name.ToLower() == albumModelDTO.Artist.ToLower())
                 .ToList();

            var imageUrl = spotifyFilter
                .Select(a => a.images)
                .FirstOrDefault()
                .FirstOrDefault()
                .url;

            var outputModel = new OwnedAlbumModel
            {
                User = albumModelDTO.User,
                Album = albumModelDTO.Album,
                Artist = albumModelDTO.Artist,
                ImageUrl = imageUrl,
                Rating = albumModelDTO.Rating
            };

            mainOutModel = outputModel;

            _logger.LogDebug($"Model: {outputModel.Artist} - {outputModel.Album} successfully uploaded.");

            return mainOutModel;
        }
    }
}
