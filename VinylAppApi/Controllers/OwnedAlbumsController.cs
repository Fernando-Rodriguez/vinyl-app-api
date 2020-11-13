using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Shared.Models.DbModels;

//---------------------------//     
//                           //
//         ( ͡° ͜ʖ ͡°)          // 
//                           //
// Author: Fernando          //
// Project: Family Vinyl Api //
//---------------------------//

namespace VinylAppApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class OwnedAlbumsController : Controller
    {
        private readonly IDbAccess _dbAccess;
        private readonly ILogger<OwnedAlbumsController> _logger;

        public OwnedAlbumsController(ILogger<OwnedAlbumsController> logger,
            IDbAccess dbAccess)
        {
            _logger = logger;
            _dbAccess = dbAccess;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            var dbResponse = await _dbAccess.GetAllOwnedAlbumModelsAsync();

            _logger.LogDebug("OwnedAlbums has been called");

            var albums = new List<OwnedAlbumModel>();

            return new
            {
                albums = dbResponse
            };
        }

        [HttpGet("{id}")]
        public async Task<OwnedAlbumModel> Get([FromBody] string userId, string id)
        {
            var response = await _dbAccess.GetAlbumModelByIdAsync(userId, id);

            _logger.LogDebug("OwnedAlbums has been called");

            return response;
        }

        [HttpPost]
        public async Task Post([FromBody] OwnedAlbumModelDto userInput)
        {
            await _dbAccess.PostAlbumAsync(userInput);

            _logger.LogDebug("OwnedAlbums has been called");
        }

        [HttpPut]
        public async Task Update([FromBody] OwnedAlbumModelDto userInput, string id)
        {
            // string userId, string id, OwnedAlbumModelDto userAlbumChanges
            await _dbAccess.UpdateAlbumAsync(userInput.UserId, id, userInput);
        }
    }
}
