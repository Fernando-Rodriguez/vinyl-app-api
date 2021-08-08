using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylApp.Domain.DTOs;
using VinylAppApi.Domain.Repository.UnitOfWork;
using VinylAppApi.Domain.Services.UserService;
using VinylAppApi.Helpers;

namespace VinylAppApi.Controllers
{
    [Route("api/v1/[controller]")]
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
            try
            {
                var context = HttpContext;

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

                context.Response.Cookies.Append(
                    "_bearer",
                    tokenResponse[0],
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        IsEssential = true,
                        Expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(10))
                    });

                context.Response.Cookies.Append(
                    "_refresh",
                    refreshResponse[1],
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        IsEssential = true,
                        Expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(1))
                    });

                

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> GetTokenFromRefresh()
        {
            try
            {
                var context = HttpContext;
                var token = context.Request.Cookies.Where(name => name.Key == "_refresh").FirstOrDefault().Value;
                var newToken = await _userService.GenerateTokenWithRefreshToken(token, _unitOfWork);

                context.Response.Cookies.Append(
                    "_bearer",
                    newToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        IsEssential = true,
                    });

                return Ok();
            }
            catch
            {
                return BadRequest("error with refresh.");
            }
        }

        [HttpGet("logout")]
        public void LogoutUser()
        {
            var context = HttpContext;

            context.Response.Cookies.Append(
                    "_bearer",
                    "",
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        IsEssential = true,
                        Expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(-1))
                    });
        }
    }
}
