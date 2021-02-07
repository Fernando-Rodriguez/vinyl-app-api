using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Shared.Models.DbModels;

namespace VinylAppApi.Controllers
{
    public class UserAlbumsController : Controller
    {
        private ILogger<UserAlbumsController> _logger;
        private readonly IDbAccess _dbAccess;

        public UserAlbumsController(ILogger<UserAlbumsController> logger, IDbAccess dbAccess)
        {
            _logger = logger;
            _dbAccess = dbAccess;
        }

        [HttpGet]
        public async Task<List<OwnedAlbumModel>> GetAlbumsByUser([FromQuery] string userName)
        {
            var res = await _dbAccess.GetAlbumByUserId(userName);

            return res;
        }
    }
}
