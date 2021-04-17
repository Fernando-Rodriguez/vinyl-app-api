using System.Collections.Generic;
using System.Threading.Tasks;
using VinylAppApi.Shared.Models.UserInterfacingModels;

namespace VinylAppApi.DataAccess.DbManager
{
    public interface IDbGroupAccess
    {
        Task<List<JoinedGroupsDTO>> GetAllGroupAlbums(string currentUserId);
    }
}