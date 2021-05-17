using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Services.AlbumService;
using VinylAppApi.Helpers;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    [Authorize]
    public class OwnedAlbumsController : Controller
    {
        private readonly ILogger<OwnedAlbumsController> _logger;
        private IUserTokenHelper _helper;
        private readonly IMongoRepo<AlbumModel> _albums;
        private readonly IMongoRepo<UserModel> _users;
        private readonly IAlbumService _albumService;

        public OwnedAlbumsController(
            ILogger<OwnedAlbumsController> logger,
            IUserTokenHelper helper,
            IMongoRepo<AlbumModel> albums,
            IMongoRepo<UserModel> users,
            IAlbumService albumService)
        {
            _logger = logger;
            _helper = helper;
            _albums = albums;
            _users = users;
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var localCtx = HttpContext;
                var localUser = await _helper.RetrieveUser(localCtx);

                var dbRes = await _albums.FilterByAsync(album => album.User == localUser.UserName);
                
                _logger.LogDebug("OwnedAlbums has been called");

                return Ok(new AlbumsDTO
                {
                    Owned_Albums = dbRes
                });
            }
            catch (Exception err)
            {
                _logger.LogError($"Error getting albums: {err}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var localCtx = HttpContext;
                var localUser = await _helper.RetrieveUser(localCtx);

                var resDb = await _albums.FindByIdAsync(id);

                _logger.LogDebug("OwnedAlbums has been called");

                return Ok(resDb);
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
            if (string.IsNullOrEmpty(userInput.Id)
                || string.IsNullOrEmpty(userInput.Album)
                || string.IsNullOrEmpty(userInput.Artist)
                || string.IsNullOrEmpty(userInput.User))
            {
                return BadRequest();
            }

            try
            {
                await _albumService.AddNewAlbumAsync(userInput, _albums);
                return Ok();
            }
            catch (Exception err)
            {
                _logger.LogError("Post Failed");
                _logger.LogError(err.ToString());
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AlbumUpdateModelDTO userInput)
        {
            if (string.IsNullOrEmpty(userInput.Id)
                || string.IsNullOrEmpty(userInput.Album)
                || string.IsNullOrEmpty(userInput.Artist)
                || string.IsNullOrEmpty(userInput.User))
            {
                return BadRequest();
            }

            try
            {
                var albumToUpdate = await _albums.FindByIdAsync(userInput.Id);

                albumToUpdate.Album = userInput.Album;
                albumToUpdate.Artist = userInput.Artist;
                albumToUpdate.Rating = userInput.Rating;

                await _albums.ReplaceOneAsync(albumToUpdate);

                return Ok();
            }
            catch (Exception e)
            {
                Console.Write(e);
                return BadRequest();

            }
        }

        /// <summary>
        /// This method should not require any user input because it doesn't
        /// make sense for a random user to be able to alter someone else's
        /// list of albums by manipulating the url. It should pull the user
        /// id from the token!
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            try
            {
                var localCtx = HttpContext;
                var localUser = await _helper.RetrieveUser(localCtx);

                var albumToDelete = await _albums.FindOneAsync(album => album.Id.ToString() == id && album.User == localUser.UserName);

                await _albums.DeleteOneAsync(album => album == albumToDelete);

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
