using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Services.GroupService;
using VinylAppApi.Helpers;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    [Authorize]
    public class GroupController : Controller
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IUserTokenHelper _helper;
        private readonly IGroupService _groupService;
        private readonly IMongoRepo<GroupModel> _groups;
        private readonly IMongoRepo<AlbumModel> _albums;

        public GroupController(
            ILogger<GroupController> logger,
            IUserTokenHelper helper,
            IGroupService groupService,
            IMongoRepo<AlbumModel> albums)
        {
            _logger = logger;
            _helper = helper;
            _groupService = groupService;
            _albums = albums;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var localCtx = HttpContext;
                var localUser = await _helper.RetrieveUser(localCtx);
                var joinedGroupAlbums = await _groupService.RetrieveGroups(localUser.UserId,_groups, _albums);
                return Ok(joinedGroupAlbums);
            }
            catch (Exception err)
            {
                _logger.LogError($"There was an error getting groups: {err}");
                return StatusCode(500);
            }
        }
    } 
}
