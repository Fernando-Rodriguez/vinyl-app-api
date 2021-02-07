using System.Security.Claims;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Shared.Models.AuthorizationModels;
using System;
using System.Threading.Tasks;

namespace VinylAppApi.Authorization.AuthorizationManager
{
    public class AuthorizationVerification : IAuthorizationVerification
    {
        private IAuthContainerModel _authModel;
        private IAuthService _authService;
        private IDbAccess _dbAccess;

        public AuthorizationVerification(IAuthContainerModel authModel,
            IAuthService authService,
            IDbAccess dbAccess)
        {
            _authModel = authModel;
            _authService = authService;
            _dbAccess = dbAccess;
        }

        public async Task<object> UserVerifcationWithIdAndSecret(string userId, string userSecret)
        {
            var userResults = await _dbAccess.QueryUser(userId, userSecret);

            if(userResults.Id != "")
            {
                _authModel.Claims = new Claim[]
                {
                    new Claim("user_id", userId)
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
