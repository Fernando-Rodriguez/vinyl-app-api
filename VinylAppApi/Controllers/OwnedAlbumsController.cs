using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Shared.Models.DbModels;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
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
        public async Task<Albums> Get()
        {
            var dbResponse = await _dbAccess.GetAllOwnedAlbumModelsAsync();

            _logger.LogDebug("OwnedAlbums has been called");

            return new Albums
            {
                Owned_Albums = dbResponse
            };
        }

        [HttpGet("{userId}/{id}")]
        public async Task<OwnedAlbumModel> Get(string userId, string id)
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

        [HttpPut("{userId}/{id}")]
        public async Task Update([FromBody] OwnedAlbumUpdateModel userInput, string id, string userId)
        {
            await _dbAccess.UpdateAlbumAsync(userId, id, userInput);
        }
    }
}
