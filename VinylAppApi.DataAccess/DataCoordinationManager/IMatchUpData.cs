using System.Threading.Tasks;
using VinylAppApi.Shared.Models.DbModels;

namespace VinylAppApi.DataAccess.DataCoordinationManager
{
    public interface IMatchUpData
    {
        Task<OwnedAlbumModel> DataMatcher(OwnedAlbumUpdateModel albumModelDTO);
    }
}