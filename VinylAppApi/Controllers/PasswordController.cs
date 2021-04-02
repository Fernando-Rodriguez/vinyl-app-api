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
    public class PasswordController : Controller
    {
        private readonly IDbUserManager _user;
        private readonly IUserTokenHelper _userHelper;
        private readonly ILogger<PasswordController> _logger;

        public PasswordController(
            ILogger<PasswordController> logger,
            IDbUserManager user,
            IUserTokenHelper userHelper)
        {
            _logger = logger;
            _user = user;
            _userHelper = userHelper;
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] UpdatePassDTO newPass)
        {
            try
            {
                var myContext = HttpContext;
                var myNewResult = await _userHelper.RetrieveUser(myContext);

                var passUpdate = await _user.UpdatePassword(myNewResult.UserId, newPass.UpdatePass);
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
