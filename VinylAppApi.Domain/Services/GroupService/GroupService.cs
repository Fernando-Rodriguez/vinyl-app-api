using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Models.UserInterfacingModels;

namespace VinylAppApi.Domain.Services.GroupService
{
    public class GroupService : IGroupService
    {
        private readonly ILogger<GroupService> _logger;

        public GroupService(ILogger<GroupService> logger)
        {
            _logger = logger;
        }

        public async Task<List<JoinedGroupsDTO>> RetrieveGroups(string currentUserId, IMongoRepo<GroupModel> userGroup, IMongoRepo<AlbumModel> albums)
        {
            var listOfGroupAlbums = new List<JoinedGroupsDTO>();

            _logger.LogDebug("Retrieving group data.");

            var listOfUserGroups = await userGroup.FilterByAsync(group => group.Users.Contains(currentUserId));

            foreach (var group in listOfUserGroups)
            {
                var listOfUserAblums = new List<AlbumModel>();

                foreach (var user in group.Users)
                {
                    if (user != currentUserId)
                    {
                        // It only makes sense to send the albums that the
                        // user doesn't have, otherwise, they would recieve
                        // a repeated list of data, which wouldn't be useful.
                        var userAlbums = await albums.FindByIdAsync(user);
                        listOfUserAblums.Add(userAlbums);
                    }
                }

                var newGroupItem = new JoinedGroupsDTO
                {
                    GroupId = group.Id.ToString(),
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
