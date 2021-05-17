using System.Threading.Tasks;
using VinylAppApi.Shared.Models.SpotifyModels.AlbumModels;

namespace VinylAppApi.Domain.Services.MusicInformationService
{
    public interface ISpotifyRequest
    {
        AlbumModel QueryModel { get; set; }

        Task<AlbumModel> SpotifySearchManager(string searchAlbum);
    }
}