using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Helpers;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IDbAccess _dbAccess;
        private readonly ILogger<GroupController> _logger;
        private readonly IUserTokenHelper _helper;

        public GroupController(ILogger<GroupController> logger, IUserTokenHelper helper, IDbAccess dbAccess)
        {
            _logger = logger;
            _dbAccess = dbAccess;
            _helper = helper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var localCtx = HttpContext;
                var localUser = await _helper.RetrieveUser(localCtx);
                var joinedGroupsAlbums = await _dbAccess.GetAllGroupAlbums(localUser.UserId);
                return Ok(joinedGroupsAlbums);
            }
            catch (Exception err)
            {
                _logger.LogError($"There was an error getting groups: {err}");
                return StatusCode(500);
            }
        }
    } 
}
