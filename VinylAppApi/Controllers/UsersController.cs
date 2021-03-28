using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Authorization.AuthorizationManager;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Shared.Models.UserInterfacingModels;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger;
        private IAuthService _authService;
        private readonly IDbUserManager _userManager;

        public UsersController(ILogger<UsersController> logger, IAuthService authService, IDbUserManager userManager)
        {
            _logger = logger;
            _authService = authService;
            _userManager = userManager;
        }

        [HttpGet]
        public UserInfoModelDTO GetUsersById()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];

            var token = authHeader.ToString()["Bearer ".Length..].Trim();

            var result = _authService.GetTokenClaims(token).ToArray();

            var resultUser = new UserInfoModelDTO
            {
                UserName = result[0].Value,
                UserId = result[1].Value
            };

            return resultUser;
        }

        [AllowAnonymous]
        [HttpPost("new")]
        public async Task<UserInfoModelDTO> CreateNewUser([FromBody] NewUserModelDTO user)
        {
            try
            {
                var newUser = await _userManager.CreateUser(user);

                return new UserInfoModelDTO
                {
                    UserName = newUser.UserName,
                    UserId = newUser.Id
                };
            }
            catch(Exception err)
            {
                _logger.LogError($"Error in creating new user: {err}");
                return new UserInfoModelDTO
                {
                    UserName = "",
                    UserId = ""
                };
            }
        }
    }
}
