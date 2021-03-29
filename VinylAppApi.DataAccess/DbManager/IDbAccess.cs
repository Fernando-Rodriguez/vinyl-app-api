using System.Collections.Generic;
using System.Threading.Tasks;
using VinylAppApi.Shared.Models.DbModels;
using VinylAppApi.Shared.Models.UserInterfacingModels;

namespace VinylAppApi.DataAccess.DbManager
{
    public interface IDbAccess
    {
        Task DeleteAlbumByIdAsync(string userId, string id);
        Task<OwnedAlbumModel> GetAlbumModelByIdAsync(string userId, string id);
        Task<List<OwnedAlbumModel>> GetAllOwnedAlbumModelsAsync(string id);
        Task PostAlbumAsync(AlbumUpdateModelDTO userInputAlbum);
        Task UpdateAlbumAsync(string userId, string id, AlbumUpdateModelDTO userAlbumChanges);
        Task<List<OwnedAlbumModel>> GetAlbumByUserId(string userId);
    }
}