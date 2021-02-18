using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Authorization.AuthorizationManager;
using VinylAppApi.Shared.Models.UserInterfacingModels;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    public class TokenController : Controller
    {
        private IAuthorizationVerification _verify;
        private ILogger<TokenController> _logger;

        public TokenController(IAuthorizationVerification verify, ILogger<TokenController> logger)
        {
            _verify = verify;
            _logger = logger;
        }

        [HttpPost]
        public async Task<object> Post([FromBody] TokenRequestDTO requestTokenInfo)
        {
            if(string.IsNullOrEmpty(requestTokenInfo.ClientName) || string.IsNullOrEmpty(requestTokenInfo.ClientSecret))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                var tokenResponse = await _verify
                    .UserVerifcationWithIdAndSecret(
                        requestTokenInfo.ClientName,
                        requestTokenInfo.ClientSecret
                    );

                _logger.LogDebug($"user {requestTokenInfo.ClientName} request token");

                return tokenResponse;
            }
        }
    }
}
