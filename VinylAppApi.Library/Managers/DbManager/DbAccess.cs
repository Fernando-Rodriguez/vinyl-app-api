using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using VinylAppApi.Library.Managers.DataCoordinationManager;
using VinylAppApi.Library.Models.AuthorizationModels;
using VinylAppApi.Library.Models.DbModels;

namespace VinylAppApi.Library.DbManager
{
    public class DbAccess : IDbAccess
    {
        private readonly IMongoCollection<OwnedAlbumModel> _ownedAlbums;
        private readonly IMongoCollection<UserModel> _databaseUser;
        private readonly IMatchUpData _matchUpData;

        public DbAccess(IConfiguration configuration, IMatchUpData matchUpData)
        {
            var config = configuration;

            _matchUpData = matchUpData;

            var dbClient = new MongoClient(config.GetConnectionString("MongoDb"));
            var db = dbClient.GetDatabase("vinyl-db");
            _ownedAlbums = db.GetCollection<OwnedAlbumModel>("albums");

            _databaseUser = db.GetCollection<UserModel>("users");
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
            var checkIfAblumInDB = _ownedAlbums.Find(album => album.Album == userInputAlbum.Album);

            if(checkIfAblumInDB.CountDocuments() == 0)
            {
                var matchedDataAlbum = await _matchUpData.DataMatcher(userInputAlbum);

                await _ownedAlbums.InsertOneAsync(matchedDataAlbum);
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

        public bool QueryUser(string userName, string userPassword)
        {
            var userQuery = _databaseUser.Find(user => user.UserName == userName);

            if(userQuery.CountDocuments() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
