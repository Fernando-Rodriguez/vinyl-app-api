using System.Threading.Tasks;
using VinylAppApi.Shared.Models.SpotifyModels.AlbumModels;

namespace VinylAppApi.SpotifyHandler.SpotifyApiManager
{
    public interface ISpotifyRequest
    {
        AlbumModel QueryModel { get; set; }

        Task<AlbumModel> SpotifySearchManager(string searchAlbum);
    }
}