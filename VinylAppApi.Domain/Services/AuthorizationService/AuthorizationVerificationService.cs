using System.Security.Claims;
using VinylAppApi.Shared.Models.AuthorizationModels;
using System.Threading.Tasks;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Services.UserService;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Entities;

namespace VinylAppApi.Domain.Services.AuthorizationService
{
    public class AuthorizationVerification : IAuthorizationVerification
    {
        private IAuthContainerModel _authModel;
        private IAuthService _authService;
        private readonly IUserService _userService;

        public AuthorizationVerification(IAuthContainerModel authModel,
            IAuthService authService,
            IUserService userService)
        {
            _authModel = authModel;
            _authService = authService;
            _userService = userService;
        }

        public async Task<object> UserVerifcationWithIdAndSecret(string userName, string userSecret, IMongoRepo<UserModel> users)
        {
            var userResults = await _userService.VerifyUser(new UserModelDTO
            {
                UserName = userName,
                UserSecret = userSecret
            }, users);

            var userInformation = await users.FindOneAsync(user => user.UserName == userName && user.UserSecret == userSecret);

            if(userResults)
            {
                _authModel.Claims = new Claim[]
                {
                    new Claim("user_name", userInformation.UserName),
                    new Claim("user_id", userInformation.Id.ToString()),
                    new Claim("user_role", userInformation.UserRole)
                };

                var userSpecificToken = _authService.TokenGeneration(_authModel);

                return new
                {
                    access_token = userSpecificToken,
                    expires_in = _authModel.ExpireMinutes
                };
            }

            else
            {
                return new
                {
                    error = "login fail"
                };
            }
        }
    }
}
