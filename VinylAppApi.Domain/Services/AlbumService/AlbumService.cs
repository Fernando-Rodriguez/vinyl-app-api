using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository.UnitOfWork;
using VinylAppApi.Domain.Services.AlbumService.DataCoordinationManager;

namespace VinylAppApi.Domain.Services.AlbumService
{
    public class AlbumService : IAlbumService
    {
        private readonly ILogger<AlbumService> _logger;
        private readonly IMatchUpData _matchUpData;

        public AlbumService(ILogger<AlbumService> logger, IMatchUpData matchUpData)
        {
            _logger = logger;
            _matchUpData = matchUpData;
        }

        public async Task AddNewAlbumAsync(NewAlbumDTO userInputAlbum, IUnitOfWork unitOfWork)
        {
            var checkIfAblumInDB = await unitOfWork
                .Albums
                .FindOneAsync(
                    album => album.Album == userInputAlbum.Album
                    && album.User == userInputAlbum.User);

            if (checkIfAblumInDB == null)
            {
                try
                {
                    var matchedDataAlbum = await _matchUpData.DataMatcher(userInputAlbum);
                    await unitOfWork.Albums.InsertOneAsync(matchedDataAlbum);
                }
                catch
                {
                    _logger.LogInformation("<------- Error Posting Album ------->");
                }
            }
        }
    }
}
