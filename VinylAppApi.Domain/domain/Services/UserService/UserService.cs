using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository.UnitOfWork;
using VinylAppApi.Domain.Services.AuthorizationService;
using VinylAppApi.Domain.Models.AuthorizationModels;
using BC = BCrypt.Net.BCrypt;

namespace VinylAppApi.Domain.Services.UserService
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IAuthContainerModel _authModel;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            ILogger<UserService> logger,
            IAuthContainerModel authModel,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _authModel = authModel;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDTO> CreateNewUser(LoginDTO newUser)
        {
            var exisitingUser = await _unitOfWork.Users.FindOneAsync(dbUser => dbUser.UserName == newUser.UserName);

            if (string.IsNullOrEmpty(exisitingUser.UserName))
            {
                string hashedPass = BC.HashPassword(newUser.UserSecret);

                var newUserInDb = new UserModel
                {
                    UserName = newUser.UserName,
                    UserSecret = hashedPass,
                    UserRole = "basic"
                };

                await _unitOfWork.Users.InsertOneAsync(newUserInDb);

                var userIsVerified = await VerifyUser(new LoginDTO
                {
                    UserName = newUser.UserName,
                    UserSecret = hashedPass
                });

                var newUserVerified = await _unitOfWork.Users.FindOneAsync(user => user.UserName == newUserInDb.UserName && user.UserSecret == newUserInDb.UserSecret);

                return new UserDTO
                {
                    UserId = newUserVerified.Id.ToString(),
                    UserName = newUserVerified.UserName
                };
            }

            throw new ArgumentException("There is already an existing user with that name");
        }

        
    }
}
