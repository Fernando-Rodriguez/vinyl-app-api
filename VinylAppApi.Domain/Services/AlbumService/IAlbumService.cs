using System.Threading.Tasks;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Models.UserInterfacingModels;

namespace VinylAppApi.Domain.Services.AlbumService
{
    public interface IAlbumService
    {
        Task AddNewAlbumAsync(AlbumUpdateModelDTO userInputAlbum, MongoRepo<AlbumModel> _albums);
    }
}