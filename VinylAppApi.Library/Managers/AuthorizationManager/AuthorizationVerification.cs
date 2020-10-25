using System.Security.Claims;
using VinylAppApi.Library.AuthorizationManager;
using VinylAppApi.Library.Models.AuthorizationModels;

namespace VinylAppApi.Library.Managers.AuthorizationManager
{
    public class AuthorizationVerification : IAuthorizationVerification
    {
        private IAuthContainerModel _authModel;
        private IAuthService _authService;

        public AuthorizationVerification(IAuthContainerModel authModel, IAuthService authService)
        {
            _authModel = authModel;
            _authService = authService;
        }

        public object UserVerifcationWithIdAndSecret(string userId, string userSecret)
        {
            //first confirm if the user is actually in the db.

            _authModel.Claims = new Claim[]
            {
                new Claim("user_id", userId ),
                new Claim("user_secret", userSecret)
            };

            var userSpecificToken = _authService.TokenGeneration(_authModel);

            return new
            {
                access_token = userSpecificToken,
                expires_in = _authModel.ExpireMinutes
            };
        }
    }
}
