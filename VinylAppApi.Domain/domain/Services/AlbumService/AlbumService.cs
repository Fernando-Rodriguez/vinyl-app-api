using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Models.Models;
using VinylAppApi.Domain.Repository.UnitOfWork;
using VinylAppApi.Domain.Services.MusicInformationService;

namespace VinylAppApi.Domain.Services.AlbumService
{
    public class AlbumService : IAlbumService
    {
        private readonly ILogger<AlbumService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpotifyRequest _spotifyRequest;

        public AlbumService(
            ILogger<AlbumService> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateAlbumDb(User user, Album album)
        {
            var checkIfAblumInDB = await _unitOfWork
                .Albums
                .FindOneAsync(
                    album => album.Album == album.Album
                    && album.User == user.UserName);

            if (checkIfAblumInDB == null)
            {
                var newAlbum = await GetAlbumArtwork(album);

                await _unitOfWork.Albums.InsertOneAsync(new AlbumModel
                {
                    User = user.UserName,
                    Album = newAlbum.Name,
                    Artist = newAlbum.Artist,
                    ImageUrl = newAlbum.ImageUrl,
                    Rating = newAlbum.Rating
                });

                user.AddAlbum(newAlbum);

                // _unitOfWork.Users.ReplaceOneAsync();
            }
        }

        public async Task<Album> GetAlbumArtwork(Album album)
        {
            return await DataMatcher(album);
        }

        public async Task<Album> DataMatcher(Album album)
        {
            var spotifyRes = await _spotifyRequest.SpotifySearchManager(album.Name);

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

            return album;
        }
    }
}
