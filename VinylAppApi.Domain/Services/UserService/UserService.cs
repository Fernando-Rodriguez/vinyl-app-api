using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository;
using BC = BCrypt.Net.BCrypt;

namespace VinylAppApi.Domain.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public async Task<UserInfoModelDTO> CreateNewUser(UserModelDTO newUser, IMongoRepo<UserModel> users)
        {
            var exisitingUser = await users.FindOneAsync(dbUser => dbUser.UserName == newUser.UserName);

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

                await users.InsertOneAsync(newUserInDb);

                var userIsVerified = await VerifyUser(new UserModelDTO
                {
                    UserName = newUser.UserName,
                    UserSecret = hashedPass
                }, users);

                var newUserVerified = await users.FindOneAsync(user => user.UserName == newUserInDb.UserName && user.UserSecret == newUserInDb.UserSecret);

                return new UserInfoModelDTO
                {
                    UserId = newUserVerified.Id.ToString(),
                    UserName = newUserVerified.UserName
                };
            }

            return new UserInfoModelDTO { };
        }

        public async Task<bool> VerifyUser(UserModelDTO user, IMongoRepo<UserModel> users)
        {
            var userQuery = await users.FindOneAsync(dbUser => dbUser.UserName == user.UserName);

            bool isVerified = false;

            if (userQuery != null)
            {
                string hashed = userQuery.UserSecret;

                isVerified = BC.Verify(user.UserSecret, hashed);
            }

            return isVerified;
        }

        public async Task UpdatePassword(UserModelDTO user, string newPass, IMongoRepo<UserModel> users)
        {
            var hashedPass = BC.HashPassword(newPass);

            await users.ReplaceOneAsync(new UserModel
            {
                UserName = user.UserName,
                UserSecret = hashedPass
            });
        }
    }
}
