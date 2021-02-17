using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using VinylAppApi.Shared.Models.AuthorizationModels;
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

            string hashed = userQuery.UserSecret;

            if (userQuery != null)
            {
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
    }
}
