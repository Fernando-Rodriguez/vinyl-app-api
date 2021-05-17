using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Services.UserService;
using VinylAppApi.Helpers;
using VinylAppApi.Domain.Models.UserInterfacingModels;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    [Authorize]
    public class PasswordController : Controller
    {
        private readonly IUserTokenHelper _userHelper;
        private readonly ILogger<PasswordController> _logger;
        private readonly IMongoRepo<UserModel> _users;
        private readonly IUserService _userService;

        public PasswordController(
            ILogger<PasswordController> logger,
            IUserTokenHelper userHelper)
        {
            _logger = logger;
            _userHelper = userHelper;
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] UpdatePassDTO newPass)
        {
            try
            {
                var myContext = HttpContext;
                var localUser = await _userHelper.RetrieveUser(myContext);
                var userReqChange = await _users.FindByIdAsync(localUser.UserId);

                await _userService.UpdatePassword(new UserModelDTO
                {
                    UserName = userReqChange.UserName,
                    UserSecret = userReqChange.UserSecret
                }, newPass.UpdatePass, _users);

                _logger.LogDebug("user password updated.");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error updating password: {e}");
                return NotFound();
            }
        }
    }
}
