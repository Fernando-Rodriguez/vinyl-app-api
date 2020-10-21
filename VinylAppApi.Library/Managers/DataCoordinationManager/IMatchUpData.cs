using System.Threading.Tasks;
using VinylAppApi.Library.Models.DbModels;

namespace VinylAppApi.Library.Managers.DataCoordinationManager
{
    public interface IMatchUpData
    {
        Task<OwnedAlbumModel> DataMatcher(OwnedAlbumModelDto albumModelDTO);
    }
}