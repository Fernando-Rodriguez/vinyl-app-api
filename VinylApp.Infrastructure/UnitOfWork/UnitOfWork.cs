using Microsoft.Extensions.Logging;
using VinylApp.Infrastructure.Repository.BaseRepository;
using VinylApp.Infrastructure.Repository.MongoModels;

namespace VinylApp.Infrastructure.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoRepo<User> _users;
        private readonly IMongoRepo<Album> _albums;
        private readonly IMongoRepo<Group> _groups;

        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(
            ILogger<UnitOfWork> logger,
            IMongoRepo<User> users,
            IMongoRepo<Album> albums,
            IMongoRepo<Group> groups)
        {
            _logger = logger;
            _users = users;
            _albums = albums;
            _groups = groups;
        }

        public IMongoRepo<User> Users
        {
            get
            {
                return _users;
            }
        }

        public IMongoRepo<Album> Albums
        {
            get
            {
                return _albums;
            }
        }

        public IMongoRepo<Group> Groups
        {
            get
            {
                return _groups;
            }
        }
    }
}
