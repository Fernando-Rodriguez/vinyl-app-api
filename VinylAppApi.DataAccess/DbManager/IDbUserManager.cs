using System.Threading.Tasks;
using VinylAppApi.Shared.Models.AuthorizationModels;

namespace VinylAppApi.DataAccess.DbManager
{
    public interface IDbUserManager
    {
        Task<UserModel> QueryUserById(string id);
        Task<UserModel> QueryUserByName(string userName);
        Task<UserModel> VerifyUser(string userName, string userPassword);

    }
}