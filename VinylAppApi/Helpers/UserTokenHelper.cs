using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VinylAppApi.Authorization.AuthorizationManager;
using VinylAppApi.DataAccess.DbManager;
using VinylAppApi.Shared.Models.UserInterfacingModels;

namespace VinylAppApi.Helpers
{
    public class UserTokenHelper : IUserTokenHelper
    {
        private ILogger<UserTokenHelper> _logger;
        private IAuthService _authService;
        private readonly IDbUserManager _userManager;

        public UserTokenHelper(
            ILogger<UserTokenHelper> logger,
            IAuthService authService,
            IDbUserManager userManager)
        {
            _logger = logger;
            _authService = authService;
            _userManager = userManager;
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
