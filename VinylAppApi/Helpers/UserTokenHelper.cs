using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Services.AuthorizationService;
using VinylAppApi.Domain.Models.UserInterfacingModels;

namespace VinylAppApi.Helpers
{
    public class UserTokenHelper : IUserTokenHelper
    {
        private ILogger<UserTokenHelper> _logger;
        private IAuthService _authService;

        public UserTokenHelper(
            ILogger<UserTokenHelper> logger,
            IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserInfoModelDTO> RetrieveUser(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"];
            var token = authHeader.ToString()["Bearer ".Length..].Trim();
            var result = await Task.Run(() => _authService.GetTokenClaims(token).ToArray());

            var resultUser = new UserInfoModelDTO
            {
                UserName = result[0].Value,
                UserId = result[1].Value
            };

            return resultUser;
        }
    }
}
