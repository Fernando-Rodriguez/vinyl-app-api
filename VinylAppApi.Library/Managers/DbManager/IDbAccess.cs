using System.Collections.Generic;
using System.Threading.Tasks;
using VinylAppApi.Library.Models.DbModels;

namespace VinylAppApi.Library.DbManager
{
    public interface IDbAccess
    {
        public Task<List<OwnedAlbumModel>> GetAllOwnedAlbumModelsAsync();
        public Task<OwnedAlbumModel> GetAlbumModelByIdAsync(string id);
        public Task PostAlbumAsync(OwnedAlbumModelDto userInputAlbum);
    }
}
