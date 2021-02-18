using System.Threading.Tasks;
using VinylAppApi.Shared.Models.DbModels;
using VinylAppApi.Shared.Models.UserInterfacingModels;

namespace VinylAppApi.DataAccess.DataCoordinationManager
{
    public interface IMatchUpData
    {
        Task<OwnedAlbumModel> DataMatcher(AlbumUpdateModelDTO albumModelDTO);
    }
}