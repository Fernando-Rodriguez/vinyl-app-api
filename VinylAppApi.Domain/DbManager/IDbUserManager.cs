//using System.Threading.Tasks;
//using VinylAppApi.Shared.Models.AuthorizationModels;
//using VinylAppApi.Shared.Models.UserInterfacingModels;

//namespace VinylAppApi.Domain.DbManager
//{
//    public interface IDbUserManager
//    {
//        Task<UserModel> QueryUserById(string id);
//        Task<UserModel> QueryUserByName(string userName);
//        Task<UserModel> VerifyUser(string userName, string userPassword);
//        Task<UserModel> CreateUser(UserModelDTO newUser);
//        Task<bool> UpdatePassword(string id, string newPass);
//    }
//}