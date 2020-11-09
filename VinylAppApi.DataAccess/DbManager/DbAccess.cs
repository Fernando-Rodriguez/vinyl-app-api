using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using VinylAppApi.DataAccess.DataCoordinationManager;
using VinylAppApi.Shared.Models.AuthorizationModels;
using VinylAppApi.Shared.Models.DbModels;
using System.Linq;

namespace VinylAppApi.DataAccess.DbManager
{
    /// <summary>
    /// This methods main edit the MongoDb database.
    /// Just about all of them require the user Id so that a user can only edit their
    /// own owned albums.
    /// </summary>
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
            //This method returns all albums regardless of the user info.

            var response = await _ownedAlbums.FindAsync(user => true);

            return response.ToList();
        }

        public async Task<OwnedAlbumModel> GetAlbumModelByIdAsync(string userId, string id)
        {
            var userDbRes = (await _databaseUser
                .FindAsync(user => user.Id == userId))
                .FirstOrDefault();

            var albumDbRes = (await _ownedAlbums
                .FindAsync(userName => userName.User == userDbRes.UserName))
                .ToList()
                .Where(album => album.Id == id)
                .FirstOrDefault();

            return albumDbRes;
        }

        public async Task PostAlbumAsync(string userId, OwnedAlbumModelDto userInputAlbum)
        {
            var checkIfAblumInDB = _ownedAlbums.Find(album => album.Album == userInputAlbum.Album);

            if (checkIfAblumInDB.CountDocuments() == 0)
            {
                var matchedDataAlbum = await _matchUpData.DataMatcher(userInputAlbum);

                await _ownedAlbums.InsertOneAsync(matchedDataAlbum);
            }
        }

        public async Task UpdateAlbumAsync(string userId, string id, OwnedAlbumModelDto userAlbumChanges)
        {
            await _ownedAlbums.ReplaceOneAsync(album => album.Id == id, new OwnedAlbumModel()
            {
                User = userAlbumChanges.User,
                Album = userAlbumChanges.Album,
                Artist = userAlbumChanges.Artist
            });
        }

        public async Task DeleteAlbumByIdAsync(string userId, string id)
        {
            await _ownedAlbums.DeleteOneAsync(album => album.Id == id);
        }

        public async Task DeleteAlbumByAlbumNameAsync(string userId, OwnedAlbumModelDto userAlbumToDelete)
        {
            await _ownedAlbums.DeleteOneAsync(album => album.Album == userAlbumToDelete.Album);
        }

        public bool QueryUser(string userName, string userPassword)
        {
            var userQuery = _databaseUser.Find(user => user.UserName == userName);

            if (userQuery.CountDocuments() != 0)
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