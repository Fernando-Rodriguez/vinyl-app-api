using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Authorization.AuthorizationManager;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Shared.Models.UserInterfacingModels;
using VinylAppApi.Helpers;
using VinylAppApi.DataAccess.Repository;
using VinylAppApi.DataAccess.Entities;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger;
        private IAuthService _authService;
        private IUserTokenHelper _helper;
        private readonly IDbUserManager _userManager;
        private readonly IMongoRepo<User> _users;

        public UsersController(
            ILogger<UsersController> logger,
            IAuthService authService,
            IDbUserManager userManager,
            IUserTokenHelper helper,
            IMongoRepo<User> users)
        {
            _logger = logger;
            _authService = authService;
            _userManager = userManager;
            _helper = helper;
            _users = users;
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
        public async Task<IActionResult> CreateNewUser([FromBody] NewUserModelDTO user)
        {
            try
            {
                var newUser = await _userManager.CreateUser(user);

                return Ok(new UserInfoModelDTO
                {
                    UserName = newUser.UserName,
                    UserId = newUser.Id
                });
            }
            catch(Exception err)
            {
                _logger.LogError($"Error in creating new user: {err}");
                return BadRequest();
            }
        }
    }
}
