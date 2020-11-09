using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Shared.Models.DbModels;

namespace VinylAppApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AlbumByUserController : Controller
    {
        private readonly IDbAccess _dbAccess;
        private readonly ILogger<AlbumByUserController> _logger;

        public AlbumByUserController(ILogger<AlbumByUserController> logger, IDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<List<OwnedAlbumModel>> GetAllAlbumsByUser(string id)
        {
            return null;
        }
    }
}
