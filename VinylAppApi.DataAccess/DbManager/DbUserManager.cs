using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using VinylAppApi.Shared.Models.AuthorizationModels;
using VinylAppApi.Shared.Models.UserInterfacingModels;
using BC = BCrypt.Net.BCrypt;

namespace VinylAppApi.DataAccess.DbManager
{
    public class DbUserManager : IDbUserManager
    {
        private readonly IMongoCollection<UserModel> _databaseUser;
        private ILogger<DbUserManager> _logger;
        private readonly IDbClient _client;

        public DbUserManager(ILogger<DbUserManager> logger, IDbClient client)
        {
            _logger = logger;
            _client = client;
            _databaseUser = _client.UsersCollection();
        }

        public async Task<UserModel> QueryUserByName(string userName)
        {
            var userQueryName = await _databaseUser.FindAsync(user => user.UserName == userName);

            return userQueryName.FirstOrDefault();
        }

        public async Task<UserModel> QueryUserById(string id)
        {
            var userQueryId = await _databaseUser.FindAsync(user => user.Id == id);

            return userQueryId.FirstOrDefault();
        }

        public async Task<UserModel> VerifyUser(string userName, string userPassword)
        {
            var errorModel = new UserModel
            {
                Id = "",
                UserName = "",
                UserSecret = "",
                UserRole = ""
            };

            var userQuery = (await _databaseUser.FindAsync(user => user.UserName == userName)).First();

            if (userQuery != null)
            {
                string hashed = userQuery.UserSecret;

                var isVerified = BC.Verify(userPassword, hashed);

                if (isVerified)
                {
                    return userQuery;
                }
                else
                {
                    return errorModel;
                }
            }
            else
            {
                return errorModel;
            }
        }

        public async Task<UserModel> CreateUser(NewUserModelDTO user)
        {
            try
            {
                string hashedPass = BC.HashPassword(user.UserSecret);

                Console.WriteLine(hashedPass);

                await _databaseUser.InsertOneAsync(new UserModel
                {
                    UserName = user.UserName,
                    UserSecret = hashedPass,
                    UserRole = "basic"
                });

                var queryUser = await VerifyUser(user.UserName, hashedPass);

                return queryUser;
            }
            catch (Exception err)
            {
                _logger.LogError($"there was an error making a user: {err}");

                return new UserModel
                {
                    Id = "",
                    UserName = "",
                    UserSecret = "",
                    UserRole = ""
                };
            }
        }

        public async Task<bool> UpdatePassword(string id, string newPass)
        {
            try
            {
                var hashedPass = BC.HashPassword(newPass);
                var userRes = await _databaseUser.UpdateOneAsync(
                    p => p.Id == id,
                    Builders<UserModel>
                        .Update
                        .Set("user_secret", hashedPass)
                );

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}
