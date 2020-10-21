using System.Linq;
using System.Threading.Tasks;
using VinylAppApi.Library.Models.DbModels;
using VinylAppApi.Library.SpotifyApiManager;

namespace VinylAppApi.Library.Managers.DataCoordinationManager
{
    public class MatchUpData : IMatchUpData
    {
        public ISpotifyRequest _spotifyRequest;

        public MatchUpData(ISpotifyRequest spotifyRequest)
        {
            _spotifyRequest = spotifyRequest;
        }

        public async Task<OwnedAlbumModel> DataMatcher(OwnedAlbumModelDto albumModelDTO)
        {
            string searchAlbum = albumModelDTO.Album;

            var spotifyRes = await _spotifyRequest.SpotifySearchManager(searchAlbum);

            var imageUrl = spotifyRes
                .albums
                .items
                .Select(a => a.images)
                .FirstOrDefault()
                .FirstOrDefault()
                .url;

            return new OwnedAlbumModel
            {
                User = albumModelDTO.User,
                Album = albumModelDTO.Album,
                Artist = albumModelDTO.Artist,
                ImageUrl = imageUrl
            };
        }
    }
}
