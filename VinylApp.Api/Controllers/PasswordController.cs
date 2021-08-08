using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository.UnitOfWork;
using VinylAppApi.Domain.Services.UserService;
using VinylAppApi.Helpers;

namespace VinylAppApi.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class PasswordController : Controller
    {
        private readonly IUserTokenHelper _userHelper;
        private readonly ILogger<PasswordController> _logger;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public PasswordController(
            ILogger<PasswordController> logger,
            IUserTokenHelper userHelper,
            IUnitOfWork unitOfWork,
            IUserService userService)
        {
            _logger = logger;
            _userHelper = userHelper;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] UpdatePassDTO newPass)
        {
            try
            {
                var myContext = HttpContext;
                var localUser = await _userHelper.RetrieveUser(myContext);
                var userReqChange = await _unitOfWork.Users.FindByIdAsync(localUser.UserId);

                await _userService.UpdatePassword(new LoginDTO
                {
                    UserName = userReqChange.UserName,
                    UserSecret = userReqChange.UserSecret
                }, newPass.UpdatePass, _unitOfWork);

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
