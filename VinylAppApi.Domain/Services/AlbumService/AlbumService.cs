﻿using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Services.AlbumService.DataCoordinationManager;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Models.UserInterfacingModels;

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

        public async Task AddNewAlbumAsync(AlbumUpdateModelDTO userInputAlbum, MongoRepo<AlbumModel> _albums)
        {
            var checkIfAblumInDB = await _albums
                .FindOneAsync(album => album.Album == userInputAlbum.Album
                    && album.User == userInputAlbum.User);

            if (string.IsNullOrEmpty(checkIfAblumInDB.Id.ToString()))
            {
                try
                {
                    var matchedDataAlbum = await _matchUpData.DataMatcher(userInputAlbum);
                    await _albums.InsertOneAsync(matchedDataAlbum);
                }
                catch
                {
                    _logger.LogInformation("<------- Error Posting Album ------->");
                }
            }
        }
    }
}
