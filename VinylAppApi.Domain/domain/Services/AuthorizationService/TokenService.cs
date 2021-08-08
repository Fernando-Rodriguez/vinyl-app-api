using System;
using System.Security.Claims;
using System.Threading.Tasks;
using VinylAppApi.Domain.Models.AuthorizationModels;
using VinylAppApi.Domain.Models.MongoModels;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository.UnitOfWork;
using VinylAppApi.Domain.Services.AuthorizationService;
using BC = BCrypt.Net.BCrypt;

namespace VinylAppApi.Domain.Services.TokenService
{
    public class TokenService
    {
        private readonly IAuthContainerModel _authModel;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(
            IAuthContainerModel authModel,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _authModel = authModel;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> VerifyUser(LoginDTO user)
        {
            var userQuery = await _unitOfWork.Users.FindOneAsync(filter => filter.Auth.UserName == user.UserName);
            bool isVerified = false;

            if (userQuery != null)
            {
                string hashed = userQuery.Auth.UserSecret;

                isVerified = BC.Verify(user.UserSecret, hashed);
            }

            return isVerified;
        }

        public async Task UpdatePassword(LoginDTO user, string newPass)
        {
            var hashedPass = BC.HashPassword(newPass);

            var updateUser = await _unitOfWork.Users.FindOneAsync(u => u.Auth.UserName == user.UserName && u.Auth.UserSecret == user.UserSecret);

            updateUser.Auth.UserSecret = hashedPass;

            await _unitOfWork.Users.ReplaceOneAsync(updateUser);
        }

        public async Task<string[]> GenerateTokenWithUserNameAndPassword(string userName, string userSecret)
        {
            var userResults = await VerifyUser(new LoginDTO
            {
                UserName = userName,
                UserSecret = userSecret
            });

            var userInformation = await _unitOfWork.Users.FindOneAsync(user => user.UserName == userName);

            if (userResults)
            {
                _authModel.Claims = new Claim[]
                {
                    new Claim("user_name", userInformation.UserName),
                    new Claim("user_id", userInformation.Id.ToString()),
                    new Claim("user_role", userInformation.UserRole)
                };

                var userSpecificToken = _authService.TokenGeneration(_authModel);

                _authModel.Claims = new Claim[]
                {
                    new Claim("user_name", userInformation.UserName),
                    new Claim("user_id", userInformation.IdString),
                    new Claim("refresh_token", "family_vinyl_app")
                };

                var userSpecificRefresh = _authService.TokenGeneration(_authModel);

                await _unitOfWork.RefreshTokens.InsertOneAsync(new RefreshModel
                {
                    Refresh = userSpecificRefresh,
                    UserId = userInformation.IdString
                });

                return new string[] { userSpecificToken, userSpecificRefresh };
            }

            throw new ArgumentException("Usename/Password unable to be verified.");
        }

        public async Task<string> GenerateTokenWithRefreshToken(string refreshToken)
        {
            var userResults = await _unitOfWork.RefreshTokens.FindOneAsync(filter => filter.Refresh == refreshToken);

            if (userResults != null)
            {
                var userFull = await _unitOfWork.Users.FindByIdAsync(userResults.UserId);

                _authModel.Claims = new Claim[]
                {
                    new Claim("user_name", userFull.UserName),
                    new Claim("user_id", userFull.IdString),
                    new Claim("user_role", userFull.UserRole)
                };

                var userSpecificToken = _authService.TokenGeneration(_authModel);

                return userSpecificToken;
            }

            throw new ArgumentException("Refresh token not found.");
        }
    }
}
