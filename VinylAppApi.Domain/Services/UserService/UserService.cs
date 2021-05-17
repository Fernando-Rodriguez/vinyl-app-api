using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository.UnitOfWork;
using VinylAppApi.Domain.Services.AuthorizationService;
using VinylAppApi.Domain.Models.AuthorizationModels;
using BC = BCrypt.Net.BCrypt;
using System.Security.Claims;
using VinylAppApi.Domain.Models.Entities;

namespace VinylAppApi.Domain.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IAuthContainerModel _authModel;
        private readonly IAuthService _authService;

        public UserService(
            ILogger<UserService> logger,
            IAuthContainerModel authModel,
            IAuthService authService)
        {
            _logger = logger;
            _authModel = authModel;
            _authService = authService;
        }

        public async Task<UserDTO> CreateNewUser(LoginDTO newUser, IUnitOfWork unitOfWork)
        {
            var exisitingUser = await unitOfWork.Users.FindOneAsync(dbUser => dbUser.UserName == newUser.UserName);

            if (string.IsNullOrEmpty(exisitingUser.UserName))
            {
                string hashedPass = BC.HashPassword(newUser.UserSecret);

                Console.WriteLine(hashedPass);

                var newUserInDb = new UserModel
                {
                    UserName = newUser.UserName,
                    UserSecret = hashedPass,
                    UserRole = "basic"
                };

                await unitOfWork.Users.InsertOneAsync(newUserInDb);

                var userIsVerified = await VerifyUser(new LoginDTO
                {
                    UserName = newUser.UserName,
                    UserSecret = hashedPass
                }, unitOfWork);

                var newUserVerified = await unitOfWork.Users.FindOneAsync(user => user.UserName == newUserInDb.UserName && user.UserSecret == newUserInDb.UserSecret);

                return new UserDTO
                {
                    UserId = newUserVerified.Id.ToString(),
                    UserName = newUserVerified.UserName
                };
            }

            throw new ArgumentException("There is already an existing user with that name");
        }

        public async Task<bool> VerifyUser(LoginDTO user, IUnitOfWork unitOfWork)
        {
            var userQuery = await unitOfWork.Users.FindOneAsync(filter => filter.UserName == user.UserName);

            bool isVerified = false;

            if (userQuery != null)
            {
                string hashed = userQuery.UserSecret;

                isVerified = BC.Verify(user.UserSecret, hashed);
            }

            return isVerified;
        }

        public async Task UpdatePassword(LoginDTO user, string newPass, IUnitOfWork unitOfWork)
        {
            var hashedPass = BC.HashPassword(newPass);

            await unitOfWork.Users.ReplaceOneAsync(new UserModel
            {
                UserName = user.UserName,
                UserSecret = hashedPass
            });
        }

        public async Task<string[]> GenerateTokenWithUserNameAndPassword(string userName, string userSecret, IUnitOfWork unitOfWork)
        {
            var userResults = await VerifyUser(new LoginDTO
            {
                UserName = userName,
                UserSecret = userSecret
            }, unitOfWork);

            var userInformation = await unitOfWork.Users.FindOneAsync(user => user.UserName == userName);

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

                await unitOfWork.RefreshTokens.InsertOneAsync(new RefreshModel
                {
                    Refresh = userSpecificRefresh,
                    UserId = userInformation.IdString
                });

                return new string[] { userSpecificToken, userSpecificRefresh };
            }

            throw new ArgumentException("Usename/Password unable to be verified.");
        }

        public async Task<string> GenerateTokenWithRefreshToken(string refreshToken, IUnitOfWork unitOfWork)
        {
            var userResults = await unitOfWork.RefreshTokens.FindOneAsync(filter => filter.Refresh == refreshToken);

            if (userResults != null)
            {
                var userFull = await unitOfWork.Users.FindByIdAsync(userResults.UserId);

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
