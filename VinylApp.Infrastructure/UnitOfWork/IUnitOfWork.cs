using VinylApp.Infrastructure.Repository.BaseRepository;
using VinylApp.Infrastructure.Repository.MongoModels;

namespace VinylApp.Infrastructure.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IMongoRepo<User> Users { get; }
        IMongoRepo<Album> Albums { get; }
        IMongoRepo<Group> Groups { get; }
    }
}