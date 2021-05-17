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
    [Route("v1/api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserTokenHelper _helper;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(
            ILogger<UsersController> logger,
            IUserTokenHelper helper,
            IUserService userService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _helper = helper;
            _userService = userService;
            _unitOfWork = unitOfWork;
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
        public async Task<IActionResult> CreateNewUser([FromBody] LoginDTO user)
        {
            try
            {
                if(!string.IsNullOrEmpty(user.UserName) || !string.IsNullOrEmpty(user.UserSecret))
                {
                   var finalizedNewUser = await _userService.CreateNewUser(user, _unitOfWork);
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
