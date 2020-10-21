using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Library.DbManager;
using VinylAppApi.Library.Models.AuthorizationModels;

namespace VinylAppApi.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        public TokenController()
        {
        }

        [HttpGet]
        public string Get()
        {
            return null;
        }

        [HttpPost]
        public async Task Post([FromBody] TokenRequestDTO requestTokenInfo)
        {
            string testClientName = "";
            string testClientSecret = "";

            //checks if the tokens are legit.



        }
    }
}
