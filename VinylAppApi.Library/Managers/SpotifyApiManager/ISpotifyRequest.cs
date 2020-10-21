using System.Threading.Tasks;
using VinylAppApi.Library.Models.SpotifyModels.AlbumModels;

namespace VinylAppApi.Library.SpotifyApiManager
{
    public interface ISpotifyRequest
    {
        AlbumModel QueryModel { get; set; }

        Task<AlbumModel> SpotifySearchManager(string searchAlbum);
    }
}