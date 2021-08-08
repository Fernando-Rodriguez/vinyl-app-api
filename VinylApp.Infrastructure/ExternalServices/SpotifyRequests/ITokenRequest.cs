using System.Threading.Tasks;
using VinylApp.Domain.AggregatesModel.SpotifyAggregate;

namespace VinylApp.Infrastructure.ExternalServices.SpotifyRequests
{
    public interface ITokenRequest
    {
        void AssignBodyContent();
        Task<TokenModel> RequestToken();
        void SetHeaders();
    }
}