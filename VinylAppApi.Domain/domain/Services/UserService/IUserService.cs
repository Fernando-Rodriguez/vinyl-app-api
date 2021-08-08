using System.Threading.Tasks;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository.UnitOfWork;

namespace VinylAppApi.Domain.Services.UserService
{
    public interface IUserService
    {
        Task<UserDTO> CreateNewUser(LoginDTO newUser, IUnitOfWork unitOfWork);
        Task UpdatePassword(LoginDTO user, string newPass, IUnitOfWork unitOfWork);
        Task<bool> VerifyUser(LoginDTO user, IUnitOfWork unitOfWork);
        Task<string[]> GenerateTokenWithUserNameAndPassword(string userName, string userSecret, IUnitOfWork unitOfWork);
        Task<string> GenerateTokenWithRefreshToken(string refreshToken, IUnitOfWork unitOfWork);
    }
}