using System.Collections.Generic;
using System.Threading.Tasks;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository.UnitOfWork;

namespace VinylAppApi.Domain.Services.GroupService
{
    public interface IGroupService
    {
        Task<List<JoinedGroupsDTO>> RetrieveGroups(string currentUserId, IUnitOfWork unitOfWork);
    }
}