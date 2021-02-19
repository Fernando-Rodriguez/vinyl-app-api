using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Shared.Models.DbModels;
using VinylAppApi.Shared.Models.UserInterfacingModels;

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
        public async Task<AlbumsDTO> Get()
        {
            var dbResponse = await _dbAccess.GetAllOwnedAlbumModelsAsync();

            _logger.LogDebug("OwnedAlbums has been called");

            return new AlbumsDTO
            {
                Owned_Albums = dbResponse
            };
        }

        [HttpGet("{userId}")]
        public async Task<AlbumsDTO> Get(string userId)
        {
            var dbResponse = await _dbAccess.GetAlbumByUserId(userId);

            return new AlbumsDTO
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
        public async Task Post([FromBody] AlbumUpdateModelDTO userInput)
        {
            try
            {
                await _dbAccess.PostAlbumAsync(userInput);

            }
            catch (Exception err)
            {
                _logger.LogError("Post Failed");
                _logger.LogError(err.ToString());
            }

            _logger.LogDebug("OwnedAlbums has been called");
        }

        [HttpPut("{userId}/{id}")]
        public async Task<HttpResponseMessage> Update([FromBody] AlbumUpdateModelDTO userInput, string id, string userId)
        {
            try
            {
                await _dbAccess.UpdateAlbumAsync(userId, id, userInput);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);

            }
        }

        [HttpDelete("{userId}/{id}")]
        public async Task<HttpResponseMessage> Delete(string userId, string id)
        {
            try
            {
                await _dbAccess.DeleteAlbumByIdAsync(userId, id);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                _logger.LogError("Error Deleting album.");
                _logger.LogError(e.ToString());
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
