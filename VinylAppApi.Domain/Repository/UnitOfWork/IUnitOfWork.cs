using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Models.Entities;

namespace VinylAppApi.Domain.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IMongoRepo<UserModel> Users { get; }
        IMongoRepo<AlbumModel> Albums { get; }
        IMongoRepo<GroupModel> Groups { get; }
        IMongoRepo<RefreshModel> RefreshTokens { get; }
    }
}