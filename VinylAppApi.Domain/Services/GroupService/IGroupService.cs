using System.Collections.Generic;
using System.Threading.Tasks;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Models.UserInterfacingModels;

namespace VinylAppApi.Domain.Services.GroupService
{
    public interface IGroupService
    {
        Task<List<JoinedGroupsDTO>> RetrieveGroups(string currentUserId, IMongoRepo<GroupModel> userGroup, IMongoRepo<AlbumModel> albums);
    }
}