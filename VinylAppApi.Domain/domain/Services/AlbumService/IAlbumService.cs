using System.Threading.Tasks;
using VinylAppApi.Domain.Models.Models;

namespace VinylAppApi.Domain.Services.AlbumService
{
    public interface IAlbumService
    {
        Task<Album> DataMatcher(Album album);
        Task<Album> GetAlbumArtwork(Album album);
        Task UpdateAlbumDb(User user, Album newAlbum);
    }
}