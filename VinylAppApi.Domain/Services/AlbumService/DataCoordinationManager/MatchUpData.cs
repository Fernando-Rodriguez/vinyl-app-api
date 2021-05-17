using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Services.MusicInformationService;

namespace VinylAppApi.Domain.Services.AlbumService.DataCoordinationManager
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

        public async Task<AlbumModel> DataMatcher(AlbumUpdateModelDTO albumModelDTO)
        {
            string searchAlbum = albumModelDTO.Album;

            var spotifyRes = await _spotifyRequest.SpotifySearchManager(searchAlbum);

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

            var outputModel = new AlbumModel
            {
                User = albumModelDTO.User,
                Album = albumModelDTO.Album,
                Artist = albumModelDTO.Artist,
                ImageUrl = imageUrl,
                Rating = 0
            };

            _logger.LogInformation($"Model: {outputModel.Artist} - {outputModel.Album} successfully uploaded.");

            return outputModel;
        }
    }
}
