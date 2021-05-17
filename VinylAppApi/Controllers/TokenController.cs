using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Entities;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Repository;
using VinylAppApi.Domain.Services.AuthorizationService;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    public class TokenController : Controller
    {
        private IAuthorizationVerification _verify;
        private ILogger<TokenController> _logger;
        private readonly IMongoRepo<UserModel> _users;

        public TokenController(
            IAuthorizationVerification verify,
            ILogger<TokenController> logger,
            IMongoRepo<UserModel> users)
        {
            _verify = verify;
            _logger = logger;
            _users = users;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModelDTO requestTokenInfo)
        {
            if (string.IsNullOrEmpty(requestTokenInfo.UserName) || string.IsNullOrEmpty(requestTokenInfo.UserSecret))
            {
                return BadRequest();
            }
            else
            {
                var tokenResponse = await _verify
                    .UserVerifcationWithIdAndSecret(
                        requestTokenInfo.UserName,
                        requestTokenInfo.UserSecret,
                        _users
                    );

                _logger.LogDebug($"user {requestTokenInfo.UserName} request token");

                return Ok(tokenResponse);
            }
        }
    }
}
