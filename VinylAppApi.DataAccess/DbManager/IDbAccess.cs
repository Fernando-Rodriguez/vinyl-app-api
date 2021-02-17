using System.Collections.Generic;
using System.Threading.Tasks;
using VinylAppApi.Shared.Models.DbModels;

namespace VinylAppApi.DataAccess.DbManager
{
    public interface IDbAccess
    {
        Task DeleteAlbumByAlbumNameAsync(string userId, OwnedAlbumModelDto userAlbumToDelete);
        Task DeleteAlbumByIdAsync(string userId, string id);
        Task<OwnedAlbumModel> GetAlbumModelByIdAsync(string userId, string id);
        Task<List<OwnedAlbumModel>> GetAllOwnedAlbumModelsAsync();
        Task PostAlbumAsync(OwnedAlbumUpdateModel userInputAlbum);
        Task UpdateAlbumAsync(string userId, string id, OwnedAlbumUpdateModel userAlbumChanges);
        Task<List<OwnedAlbumModel>> GetAlbumByUserId(string userId);
    }
}