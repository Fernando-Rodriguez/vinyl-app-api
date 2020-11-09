using System.Collections.Generic;
using System.Threading.Tasks;
using VinylAppApi.Shared.Models.DbModels;

namespace VinylAppApi.DataAccess.DbManager
{
    public interface IDbAccess
    {
        public Task<List<OwnedAlbumModel>> GetAllOwnedAlbumModelsAsync();
        public Task<OwnedAlbumModel> GetAlbumModelByIdAsync(string userId, string id);
        public Task PostAlbumAsync(string userId, OwnedAlbumModelDto userInputAlbum);
        public bool QueryUser(string userName, string userPassword);
    }
}
