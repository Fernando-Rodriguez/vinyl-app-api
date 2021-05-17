using System.Threading.Tasks;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Repository;

namespace VinylAppApi.Domain.Services.AuthorizationService
{
    public interface IAuthorizationVerification
    {
        Task<object> UserVerifcationWithIdAndSecret(string userName, string userSecret, IMongoRepo<UserModel> users);
    }
}