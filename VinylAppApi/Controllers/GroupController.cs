using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Repository.UnitOfWork;
using VinylAppApi.Domain.Services.GroupService;
using VinylAppApi.Helpers;

namespace VinylAppApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IUserTokenHelper _helper;
        private readonly IGroupService _groupService;
        private readonly IUnitOfWork _unitOfWork;

        public GroupController(
            ILogger<GroupController> logger,
            IUserTokenHelper helper,
            IGroupService groupService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _helper = helper;
            _groupService = groupService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var localCtx = HttpContext;
                var localUser = await _helper.RetrieveUser(localCtx);
                var joinedGroupAlbums = await _groupService.RetrieveGroups(localUser.UserId, _unitOfWork);
                return Ok(joinedGroupAlbums);
            }
            catch (Exception err)
            {
                _logger.LogError($"There was an error getting groups: {err}");
                return StatusCode(500);
            }
        }
        // TODO: add methods to delete groups, add members to groups, remove members from groups.
    } 
}
