using System.Threading.Tasks;
using VinylApi.Domain.DTOs.SpotifyModels.AlbumModels;
using VinylAppApi.Domain.Models.Models;

namespace VinylApp.Infrastructure.ExternalServices.SpotifyRequests
{
    public interface IAlbumRequest
    {
        Task<AlbumModel> Search(Album album);
    }
}