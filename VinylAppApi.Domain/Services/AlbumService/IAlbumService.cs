using System.Threading.Tasks;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository.UnitOfWork;

namespace VinylAppApi.Domain.Services.AlbumService
{
    public interface IAlbumService
    {
        Task AddNewAlbumAsync(NewAlbumDTO userInputAlbum, IUnitOfWork unitOfWork);
    }
}