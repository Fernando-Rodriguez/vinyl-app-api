using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Services.UserService;
using VinylAppApi.Helpers;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger;
        private IUserTokenHelper _helper;
        private readonly IMongoRepo<UserModel> _users;
        private readonly IUserService _userService;

        public UsersController(
            ILogger<UsersController> logger,
            IUserTokenHelper helper,
            IMongoRepo<UserModel> users,
            IUserService userService)
        {
            _logger = logger;
            _helper = helper;
            _users = users;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersById()
        {
            try
            {
                var myContext = HttpContext;
                var myNewResult = await _helper.RetrieveUser(myContext);
           
                return Ok(myNewResult);
            }
            catch (Exception err)
            {
                _logger.LogError($"Error getting user: {err}");
                return StatusCode(500);
            }   
        }

        [AllowAnonymous]
        [HttpPost("new")]
        public async Task<IActionResult> CreateNewUser([FromBody] UserModelDTO user)
        {
            try
            {
                if(!string.IsNullOrEmpty(user.UserName) || !string.IsNullOrEmpty(user.UserSecret))
                {
                   var finalizedNewUser = await _userService.CreateNewUser(user, _users);
                   return Ok(finalizedNewUser);
                }

                return BadRequest();
            }
            catch(Exception err)
            {
                _logger.LogError($"Error in creating new user: {err}");
                return BadRequest();
            }
        }
    }
}
