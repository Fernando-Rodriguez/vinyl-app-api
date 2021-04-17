using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using VinylAppApi.Shared.Models.DbModels;
using VinylAppApi.Shared.Models.UserInterfacingModels;

namespace VinylAppApi.DataAccess.DbManager
{
    public class DbGroupAccess : IDbGroupAccess
    {
        private readonly IMongoCollection<GroupModel> _userGroup;
        private readonly IDbAccess _dbAccess;
        private readonly ILogger<DbGroupAccess> _logger;

        public DbGroupAccess
        (
            IMongoCollection<GroupModel> userGroup,
            IDbAccess dbAccess,
            ILogger<DbGroupAccess> logger
        )
        {
            _logger = logger;
            _userGroup = userGroup;
            _dbAccess = dbAccess;
        }

        public async Task<List<JoinedGroupsDTO>> GetAllGroupAlbums(string currentUserId)
        {
            var listOfGroupAlbums = new List<JoinedGroupsDTO>();

            _logger.LogDebug("Retrieving group data.");

            var listOfUserGroups = await _userGroup.FindAsync(group => group.Users.Contains(currentUserId));

            foreach (var group in listOfUserGroups.ToList())
            {
                var listOfUserAblums = new List<OwnedAlbumModel>();

                foreach (var user in group.Users)
                {
                    if (user != currentUserId)
                    {
                        // It only makes sense to send the albums that the
                        // user doesn't have, otherwise, they would recieve
                        // a repeated list of data, which wouldn't be useful.
                        var userAlbums = await _dbAccess.GetAlbumByUserId(user);
                        listOfUserAblums.AddRange(userAlbums);
                    }
                }

                var newGroupItem = new JoinedGroupsDTO
                {
                    GroupId = group.Id,
                    GroupName = group.GroupName,
                    GroupAlbums = listOfUserAblums
                };

                _logger.LogDebug("Adding group to output list.");

                listOfGroupAlbums.Add(newGroupItem);
            }

            return listOfGroupAlbums;
        }
    }
}
