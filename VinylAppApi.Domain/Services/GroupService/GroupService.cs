using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository.UnitOfWork;

namespace VinylAppApi.Domain.Services.GroupService
{
    public class GroupService : IGroupService
    {
        private readonly ILogger<GroupService> _logger;

        public GroupService(ILogger<GroupService> logger)
        {
            _logger = logger;
        }

        public async Task<List<JoinedGroupsDTO>> RetrieveGroups(string currentUserId, IUnitOfWork unitOfWork)
        {
            var listOfGroupAlbums = new List<JoinedGroupsDTO>();

            _logger.LogDebug("Retrieving group data.");

            var listOfUserGroups = await unitOfWork.Groups.FilterByAsync(group => group.Users.Contains(currentUserId));

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
                        var specificUser = await unitOfWork.Users.FindByIdAsync(user);
                        var userAlbums = await unitOfWork.Albums.FilterByAsync(filter => filter.User == specificUser.UserName);
                        listOfUserAblums.AddRange(userAlbums);
                    }
                }

                var newGroupItem = new JoinedGroupsDTO
                {
                    GroupId = group.IdString,
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
