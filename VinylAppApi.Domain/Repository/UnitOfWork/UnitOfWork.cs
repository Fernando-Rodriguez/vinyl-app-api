using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Models.Entities;

namespace VinylAppApi.Domain.Repository.UnitOfWork
{
    /// <summary>
    /// Not a true UOW. Functions mainly as an abstraction of all
    /// of the repos.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoRepo<UserModel> _users;
        private readonly IMongoRepo<AlbumModel> _albums;
        private readonly IMongoRepo<GroupModel> _groups;
        private readonly IMongoRepo<RefreshModel> _refreshTokens;

        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(
            IMongoRepo<UserModel> users,
            IMongoRepo<AlbumModel> albums,
            IMongoRepo<GroupModel> groups,
            IMongoRepo<RefreshModel> refreshTokens)
        {
            _users = users;
            _albums = albums;
            _groups = groups;
            _refreshTokens = refreshTokens;
        }

        public IMongoRepo<UserModel> Users
        {
            get
            {
                return _users;
            }
        }

        public IMongoRepo<AlbumModel> Albums
        {
            get
            {
                return _albums;
            }
        }

        public IMongoRepo<GroupModel> Groups
        {
            get
            {
                return _groups;
            }
        }

        public IMongoRepo<RefreshModel> RefreshTokens
        {
            get
            {
                return _refreshTokens;
            }
        }
    }
}
