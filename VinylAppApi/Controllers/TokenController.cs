using Microsoft.AspNetCore.Mvc;
using VinylAppApi.Library.Managers.AuthorizationManager;
using VinylAppApi.Library.Models.AuthorizationModels;

namespace VinylAppApi.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IAuthContainerModel _authModel;
        private IAuthorizationVerification _verify;

        public TokenController(IAuthContainerModel authModel, IAuthorizationVerification verify)
        {
            _authModel = authModel;
            _verify = verify;
        }

        [HttpGet]
        public string Get()
        {
            return null;
        }

        [HttpPost]
        public object Post([FromBody] TokenRequestDTO requestTokenInfo)
        {
            //verify that those two fields are good then...

            var tokenResponse = _verify.UserVerifcationWithIdAndSecret(requestTokenInfo.ClientName, requestTokenInfo.ClientSecret);

            return tokenResponse;
        }
    }
}
