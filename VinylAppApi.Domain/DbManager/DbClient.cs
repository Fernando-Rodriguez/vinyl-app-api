//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using MongoDB.Driver;
//using VinylAppApi.Shared.Models.AuthorizationModels;
//using VinylAppApi.Shared.Models.DbModels;

//namespace VinylAppApi.Domain.DbManager
//{
//    public class DbClient : IDbClient
//    {
//        private readonly IMongoCollection<OwnedAlbumModel> _ownedAlbums;
//        private readonly IMongoCollection<UserModel> _databaseUser;
//        private readonly IMongoCollection<GroupModel> _userGroups;
//        private ILogger<DbClient> _logger;

//        public DbClient(ILogger<DbClient> logger, IConfiguration configuration)
//        {
//            var config = configuration;
//            _logger = logger;

//            var dbClient = new MongoClient(config.GetConnectionString("MongoDb"));
//            var db = dbClient.GetDatabase("vinyl-db");
//            _ownedAlbums = db.GetCollection<OwnedAlbumModel>("albums");
//            _databaseUser = db.GetCollection<UserModel>("users");
//            _userGroups = db.GetCollection<GroupModel>("user-groups");
//        }

//        public IMongoCollection<OwnedAlbumModel> AlbumCollection()
//        {
//            _logger.LogDebug("Pulling users collection.");
//            return _ownedAlbums;
//        }

//        public IMongoCollection<UserModel> UsersCollection()
//        {
//            return _databaseUser;
//        }

//        public IMongoCollection<GroupModel> GroupCollection()
//        {
//            return _userGroups;
//        }
//    }
//}
