﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VinylAppApi.Authorization.AuthorizationManager;
using VinylAppApi.Shared.Models.AuthorizationModels;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    public class TokenController : Controller
    {
        private IAuthorizationVerification _verify;

        public TokenController(IAuthorizationVerification verify)
        {
            _verify = verify;
        }

        [HttpPost]
        public async Task<object> Post([FromBody] TokenRequestDTO requestTokenInfo)
        {
            //verify that those two fields are good then...

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

                return tokenResponse;
            }
        }
    }
}
