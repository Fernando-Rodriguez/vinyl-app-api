using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Helpers;
using VinylAppApi.Shared.Models.UserInterfacingModels;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    [Authorize]
    public class OwnedAlbumsController : Controller
    {
        private readonly IDbAccess _dbAccess;
        private readonly ILogger<OwnedAlbumsController> _logger;
        private IUserTokenHelper _helper;

        public OwnedAlbumsController(
            ILogger<OwnedAlbumsController> logger,
            IDbAccess dbAccess,
            IUserTokenHelper helper)
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

                var dbResponse = await _dbAccess.GetAllOwnedAlbumModelsAsync(localUser.UserId);

                _logger.LogDebug("OwnedAlbums has been called");

                return Ok(new AlbumsDTO
                {
                    Owned_Albums = dbResponse
                });
            }
            catch (Exception err)
            {
                _logger.LogError($"Error getting albums: {err}");
                return BadRequest();
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var dbResponse = await _dbAccess.GetAlbumByUserId(userId);

            return Ok(new AlbumsDTO
            {
                Owned_Albums = dbResponse
            });
        }

        [HttpGet("{userId}/{id}")]
        public async Task<IActionResult> Get(string userId, string id)
        {
            try
            {
                var response = await _dbAccess.GetAlbumModelByIdAsync(userId, id);

                _logger.LogDebug("OwnedAlbums has been called");

                return Ok(response);
            }
            catch (Exception err)
            {
                _logger.LogError($"Error getting album by ID: {err}");

                return BadRequest();
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AlbumUpdateModelDTO userInput)
        {
            try
            {
                await _dbAccess.PostAlbumAsync(userInput);
                return Ok();

            }
            catch (Exception err)
            {
                _logger.LogError("Post Failed");
                _logger.LogError(err.ToString());
                return BadRequest();
            }
        }

        [HttpPut("{userId}/{id}")]
        public async Task<IActionResult> Update([FromBody] AlbumUpdateModelDTO userInput, string id, string userId)
        {
            try
            {
                await _dbAccess.UpdateAlbumAsync(userId, id, userInput);

                return Ok();
            }
            catch (Exception e)
            {
                Console.Write(e);
                return BadRequest();

            }
        }

        [HttpDelete("{userId}/{id}")]
        public async Task<IActionResult> Delete(string userId, string id)
        {
            try
            {
                await _dbAccess.DeleteAlbumByIdAsync(userId, id);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError("Error Deleting album.");
                _logger.LogError(e.ToString());
                return BadRequest();
            }
        }
    }
}
