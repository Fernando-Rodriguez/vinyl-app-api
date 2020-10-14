using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using VinylAppApi.Library.DbModels;

namespace VinylAppApi.Library.DbManager
{
    public interface IDbAccess
    {
        public Task<List<OwnedAlbumModel>> GetAllOwnedAlbumModelsAsync();
        public Task<OwnedAlbumModel> GetAlbumModelByIdAsync(string id);
        public Task PostAlbumAsync(OwnedAlbumModelDto userInputAlbum);
    }

    public class DbAccess : IDbAccess
    {
        private readonly IMongoCollection<OwnedAlbumModel> _ownedAlbums;

        public DbAccess(IConfiguration configuration)
        {
            var config = configuration;
            var dbClient = new MongoClient(config.GetConnectionString("MongoDb"));
            var db = dbClient.GetDatabase("vinyl-db");
            _ownedAlbums = db.GetCollection<OwnedAlbumModel>("albums"); 
        }

        public async Task<List<OwnedAlbumModel>> GetAllOwnedAlbumModelsAsync()
        {
            var response = await _ownedAlbums.FindAsync(user => true);

            return response.ToList();
        }

        public async Task<OwnedAlbumModel> GetAlbumModelByIdAsync(string id)
        {
            var response = await _ownedAlbums.FindAsync(item => item.Id == id);

            return response.FirstOrDefault();
        }

        public async Task PostAlbumAsync(OwnedAlbumModelDto userInputAlbum)
        {
            if(userInputAlbum != null)
            {
                await _ownedAlbums.InsertOneAsync(new OwnedAlbumModel
                {
                    User = userInputAlbum.User,
                    Album = userInputAlbum.Album,
                    Artist = userInputAlbum.Artist
                });
            }
        }

        public async Task UpdateAlbumAsync(string id, OwnedAlbumModelDto userAlbumChanges)
        {
            await _ownedAlbums.ReplaceOneAsync(album => album.Id == id, new OwnedAlbumModel()
            {
                User = userAlbumChanges.User,
                Album = userAlbumChanges.Album,
                Artist = userAlbumChanges.Artist
            });
        }

        public async Task DeleteAlbumByIdAsync(string id)
        {
            await _ownedAlbums.DeleteOneAsync(album => album.Id == id);
        }

        public async Task DeleteAlbumByAlbumNameAsync(OwnedAlbumModelDto userAlbumToDelete)
        {
            await _ownedAlbums.DeleteOneAsync(album => album.Album == userAlbumToDelete.Album);
        }
    }
}
