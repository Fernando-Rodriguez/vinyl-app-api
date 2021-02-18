using System.Security.Claims;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Shared.Models.AuthorizationModels;
using System.Threading.Tasks;

namespace VinylAppApi.Authorization.AuthorizationManager
{
    public class AuthorizationVerification : IAuthorizationVerification
    {
        private IAuthContainerModel _authModel;
        private IAuthService _authService;
        private IDbUserManager _userManager;

        public AuthorizationVerification(IAuthContainerModel authModel,
            IAuthService authService,
            IDbUserManager userManager)
        {
            _authModel = authModel;
            _authService = authService;
            _userManager = userManager;
        }

        public async Task<object> UserVerifcationWithIdAndSecret(string userName, string userSecret)
        {
            var userResults = await _userManager.VerifyUser(userName, userSecret);

            if(userResults.Id != "")
            {
                _authModel.Claims = new Claim[]
                {
                    new Claim("user_name", userResults.UserName),
                    new Claim("user_id", userResults.Id),
                    new Claim("user_role", userResults.UserRole)
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
