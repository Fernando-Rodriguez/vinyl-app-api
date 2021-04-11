using MongoDB.Driver;
using VinylAppApi.Shared.Models.AuthorizationModels;
using VinylAppApi.Shared.Models.DbModels;

namespace VinylAppApi.DataAccess.DbManager
{
    public interface IDbClient
    {
        IMongoCollection<OwnedAlbumModel> AlbumCollection();
        IMongoCollection<UserModel> UsersCollection();
        IMongoCollection<GroupModel> GroupCollection();
    }
}