using System.Threading.Tasks;

namespace VinylAppApi.Authorization.AuthorizationManager
{
    public interface IAuthorizationVerification
    {
        Task<object> UserVerifcationWithIdAndSecret(string userId, string userSecret);
    }
}