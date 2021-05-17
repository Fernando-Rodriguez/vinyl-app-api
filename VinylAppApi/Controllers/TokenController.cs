using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository.UnitOfWork;
using VinylAppApi.Domain.Services.UserService;
using VinylAppApi.Helpers;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    public class TokenController : Controller
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenHelper _userHelper;

        public TokenController(
            ILogger<TokenController> logger,
            IUnitOfWork unitOfWork,
            IUserService userService,
            IUserTokenHelper helper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _userHelper = helper;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] LoginDTO requestTokenInfo)
        {
            if (string.IsNullOrEmpty(requestTokenInfo.UserName) || string.IsNullOrEmpty(requestTokenInfo.UserSecret))
            {
                return BadRequest();
            }
            else
            {
                var tokenResponse = await _userService
                    .GenerateTokenWithUserNameAndPassword(
                        requestTokenInfo.UserName,
                        requestTokenInfo.UserSecret,
                        _unitOfWork
                    );

                var refreshResponse = await _userService
                    .GenerateTokenWithUserNameAndPassword(
                        requestTokenInfo.UserName,
                        requestTokenInfo.UserSecret,
                        _unitOfWork
                    );

                _logger.LogDebug($"user {requestTokenInfo.UserName} request token");

                Response.Cookies.Append(
                    "_bearer",
                    tokenResponse[0],
                    new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                    });
                Response.Cookies.Append(
                    "_refresh",
                    refreshResponse[1],
                    new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                    });

                return Ok();
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> GetTokenFromRefresh()
        {
            var context = HttpContext;
            var token = context.Request.Cookies.Where(name => name.Key == "_refresh").FirstOrDefault().Value;
            var newToken = await _userService.GenerateTokenWithRefreshToken(token, _unitOfWork);

            Response.Cookies.Append(
                "_bearer",
                newToken,
                new Microsoft.AspNetCore.Http.CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                });

            return Ok();
        }
    }
}
