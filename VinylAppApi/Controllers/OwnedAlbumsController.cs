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
        public async Task<List<OwnedAlbumModel>> Get()
        {
            var dbResponse = await _dbAccess.GetAllOwnedAlbumModelsAsync();

            _logger.LogDebug("OwnedAlbums has been called");

            return dbResponse;
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
            await _dbAccess.PostAlbumAsync(userInput.UserId, userInput);

            _logger.LogDebug("OwnedAlbums has been called");
        }
    }
}
