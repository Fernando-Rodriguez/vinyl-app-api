using System.Threading.Tasks;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Models.UserInterfacingModels;

namespace VinylAppApi.Domain.Services.AlbumService.DataCoordinationManager
{
    public interface IMatchUpData
    {
        Task<AlbumModel> DataMatcher(AlbumUpdateModelDTO albumModelDTO);
    }
}