using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using VinylAppApi.DataAccess.DataCoordinationManager;
using VinylAppApi.Shared.Models.AuthorizationModels;
using VinylAppApi.Shared.Models.DbModels;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using BC = BCrypt.Net.BCrypt;

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
        private ILogger<DbAccess> _logger;

        public DbAccess(ILogger<DbAccess> logger, IConfiguration configuration, IMatchUpData matchUpData)
        {
            var config = configuration;
            _matchUpData = matchUpData;
            _logger = logger;

            var dbClient = new MongoClient(config.GetConnectionString("MongoDb"));
            var db = dbClient.GetDatabase("vinyl-db");
            _ownedAlbums = db.GetCollection<OwnedAlbumModel>("albums");
            _databaseUser = db.GetCollection<UserModel>("users");
        }

        public async Task<List<OwnedAlbumModel>> GetAllOwnedAlbumModelsAsync()
        {
            //This method returns all albums regardless of the user info.

            var response = await _ownedAlbums.FindAsync(user => true);

            _logger.LogDebug("<------ All Albums Returned ------>");

            return response.ToList();
        }

        public async Task<List<OwnedAlbumModel>> GetAlbumByUserId(string userId)
        {
            var albumByUserIdList = await _ownedAlbums.FindAsync(album => album.User == userId);

            return albumByUserIdList.ToList();
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

            _logger.LogDebug("<------ Id Albums Returned ------>");

            return albumDbRes;
        }

        public async Task PostAlbumAsync(OwnedAlbumModelDto userInputAlbum)
        {
            var checkIfAblumInDB = await _ownedAlbums.FindAsync(album => album.Album == userInputAlbum.Album);

            var albumsChecked = checkIfAblumInDB.ToList();

            if (albumsChecked.Count() == 0)
            {
                try
                {
                    var matchedDataAlbum = await _matchUpData.DataMatcher(userInputAlbum);
                    await _ownedAlbums.InsertOneAsync(matchedDataAlbum);
                }
                catch
                {
                    _logger.LogInformation("<------- Error Posting Album ------->");
                }   
            }
            else
            {
                _logger.LogDebug("<------ Error Posting Album ------>");
            }
        }

        public async Task UpdateAlbumAsync(string userId, string id, OwnedAlbumModelDto userAlbumChanges)
        {
            var checkIfAlbumInDb = (await _ownedAlbums.FindAsync(album => album.Id == id)).ToList();

            if (checkIfAlbumInDb.Count() != 0)
            {
                await _ownedAlbums.ReplaceOneAsync(album => album.Id == id, new OwnedAlbumModel()
                {
                    User = userAlbumChanges.User,
                    Album = userAlbumChanges.Album,
                    Artist = userAlbumChanges.Artist
                });
            }

            _logger.LogDebug("<------ Update Albums Success ------>");

        }

        public async Task DeleteAlbumByIdAsync(string userId, string id)
        {
            var checkIfAlbumInDb = (await _ownedAlbums.FindAsync(album => album.Id == id)).ToList();

            if (checkIfAlbumInDb.Count() != 0)
            {
                await _ownedAlbums.DeleteOneAsync(album => album.Id == id);
            }

            _logger.LogDebug("<------ Deleted Albums Success ------>");
        }

        public async Task DeleteAlbumByAlbumNameAsync(string userId, OwnedAlbumModelDto userAlbumToDelete)
        {
            var checkIfAlbumInDb = (await _ownedAlbums.FindAsync(album => album.Album == userAlbumToDelete.Album)).ToList();

            if (checkIfAlbumInDb.Count() != 0)
            {
                await _ownedAlbums.DeleteOneAsync(album => album.Album == userAlbumToDelete.Album);
            }

            _logger.LogDebug("<------ Deleted Albums By Id Success ------>");

        }

        public async Task<UserModel> QueryUser(string userName, string userPassword)
        {

            var errorModel = new UserModel
            {
                Id = "",
                UserName = "",
                UserSecret = "",
                UserRole = ""
            };

            var userQuery = (await _databaseUser.FindAsync(user => user.UserName == userName)).ToList().First();

            string hashed = userQuery.UserSecret;

            if (userQuery != null)
            {
                var isVerified = BC.Verify(userPassword, hashed);

                if (isVerified)
                {
                    return userQuery;
                }
                else
                {
                    return errorModel;
                }
            }
            else
            {
                return errorModel;
            }
        }

        public async Task<UserModel> QueryUserByName(string userName)
        {
            var userQueryName = await _databaseUser.FindAsync(user => user.UserName == userName);

            return userQueryName.First();
        }
    }
}