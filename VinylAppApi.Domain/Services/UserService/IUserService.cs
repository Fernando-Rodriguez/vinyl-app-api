using System.Threading.Tasks;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Models.UserInterfacingModels;

namespace VinylAppApi.Domain.Services.UserService
{
    public interface IUserService
    {
        Task<UserInfoModelDTO> CreateNewUser(UserModelDTO newUser, IMongoRepo<UserModel> users);
        Task UpdatePassword(UserModelDTO user, string newPass, IMongoRepo<UserModel> users);
        Task<bool> VerifyUser(UserModelDTO user, IMongoRepo<UserModel> users);
    }
}