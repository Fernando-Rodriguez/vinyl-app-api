using Microsoft.AspNetCore.Mvc;
using VinylAppApi.Library.Managers.AuthorizationManager;
using VinylAppApi.Library.Models.AuthorizationModels;

//---------------------------//     
//                           //
//         ( ͡° ͜ʖ ͡°)          // 
//                           //
// Author: Fernando          //
// Project: Family Vinyl Api //
//---------------------------//

namespace VinylAppApi.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IAuthorizationVerification _verify;

        public TokenController(IAuthorizationVerification verify)
        {
            _verify = verify;
        }

        [HttpPost]
        public object Post([FromBody] TokenRequestDTO requestTokenInfo)
        {
            //verify that those two fields are good then...

            var tokenResponse = _verify
                .UserVerifcationWithIdAndSecret(requestTokenInfo.ClientName, requestTokenInfo.ClientSecret);

            return tokenResponse;
        }
    }
}
