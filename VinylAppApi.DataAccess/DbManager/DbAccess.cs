using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using VinylAppApi.DataAccess.DataCoordinationManager;
using VinylAppApi.Shared.Models.AuthorizationModels;
using VinylAppApi.Shared.Models.DbModels;
using System.Linq;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Linq;
using VinylAppApi.Shared.Models.UserInterfacingModels;

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
        private readonly IMongoCollection<GroupModel> _userGroup;

        private readonly IDbUserManager _userManager;

        private readonly IDbClient _dbClient;
        private readonly IMatchUpData _matchUpData;
        private ILogger<DbAccess> _logger;

        public DbAccess(ILogger<DbAccess> logger, IMatchUpData matchUpData, IDbClient dbClient, IDbUserManager userManager)
        {
            _matchUpData = matchUpData;
            _logger = logger;
            _dbClient = dbClient;
            _userManager = userManager;

            _ownedAlbums = _dbClient.AlbumCollection();
            _databaseUser = _dbClient.UsersCollection();
            _userGroup = _dbClient.GroupCollection();
        }

        public async Task<List<OwnedAlbumModel>> GetAllOwnedAlbumModelsAsync(string id)
        {
            //This method returns all albums regardless of the user info.
            var user = await _userManager.QueryUserById(id);

            var response = await _ownedAlbums.FindAsync(album => album.User == user.UserName);

            _logger.LogDebug("<------ All Albums Returned ------>");

            return response.ToList();
        }

        public async Task<List<OwnedAlbumModel>> GetAlbumByUserId(string userId)
        {           
            var user = await _userManager.QueryUserById(userId);

            var albumByUserIdList = await _ownedAlbums.FindAsync(album => album.User == user.UserName);

            return albumByUserIdList.ToList();
        }

        public async Task<OwnedAlbumModel> GetAlbumModelByIdAsync(string userId, string id)
        {
            var albumDbRes = new OwnedAlbumModel();

            var userDbRes = await _userManager.QueryUserById(userId);

            var userItem = userDbRes;

            if(userItem != null)
            {
                var userAlbums = (await _ownedAlbums.FindAsync(album => album.User == userItem.UserName)).ToList();
                albumDbRes = userAlbums
                    .Where(album => album.Id == id)
                    .FirstOrDefault();
            }

            _logger.LogDebug("<------ Id Albums Returned ------>");

            return albumDbRes;
        }

        public async Task PostAlbumAsync(AlbumUpdateModelDTO userInputAlbum)
        {
            var checkIfAblumInDB = await _ownedAlbums.FindAsync(album => album.Album == userInputAlbum.Album && album.User == userInputAlbum.User);

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

        public async Task UpdateAlbumAsync(string userId, string id, AlbumUpdateModelDTO userAlbumChanges)
        {
            var userDbRes = await _userManager.QueryUserById(userId);

            var userItem = userDbRes;

            var checkIfAlbumInDb = (await _ownedAlbums
                .FindAsync(album => (album.Id == id) &&
                                    (album.User == userItem.UserName)))
                .ToList();

            if (checkIfAlbumInDb.Count() != 0)
            {
                await _ownedAlbums.UpdateOneAsync(
                    p => p.Id == id,
                    Builders<OwnedAlbumModel>
                        .Update
                        .Set("user", userAlbumChanges.User)
                        .Set("album", userAlbumChanges.Album)
                        .Set("artist", userAlbumChanges.Artist)
                        .Set("rating", userAlbumChanges.Rating)
                );
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

        public async Task<List<JoinedGroupsDTO>> GetAllGroupAlbums(string currentUserId)
        {
            var listOfGroupAlbums = new List<JoinedGroupsDTO>();
            var listOfUserGroups = (await _userGroup.FindAsync(group => group.Users.Contains(currentUserId))).ToList();

            foreach(var group in listOfUserGroups)
            {
                var userAlbums = await GetAlbumByUserId(group.Id);
                var newGroupItem = new JoinedGroupsDTO
                {
                    GroupName = group.GroupName,
                    GroupAlbums = userAlbums
                };

                listOfGroupAlbums.Add(newGroupItem);
            }

            return listOfGroupAlbums;
        }
    }
}