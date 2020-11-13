﻿using System.Collections.Generic;
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
        Task PostAlbumAsync(OwnedAlbumModelDto userInputAlbum);
        bool QueryUser(string userName, string userPassword);
        Task UpdateAlbumAsync(string userId, string id, OwnedAlbumModelDto userAlbumChanges);
    }
}