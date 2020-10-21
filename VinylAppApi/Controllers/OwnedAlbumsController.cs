using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Library.DbManager;
using VinylAppApi.Library.Models.DbModels;

namespace VinylAppApi.Controllers
{
    [Route("api/[controller]")]
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

            return dbResponse;
        }

        [HttpGet("{id}")]
        public async Task<OwnedAlbumModel> Get(string id)
        {
            var response = await _dbAccess.GetAlbumModelByIdAsync(id);

            return response;
        }

        [HttpPost]
        public async Task Post([FromBody] OwnedAlbumModelDto userInput)
        {
            await _dbAccess.PostAlbumAsync(userInput);
        }
    }
}
