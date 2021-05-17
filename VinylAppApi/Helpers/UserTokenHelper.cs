using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VinylAppApi.Domain.Models.UserInterfacingModels;
using VinylAppApi.Domain.Services.AuthorizationService;

namespace VinylAppApi.Helpers
{
    public class UserTokenHelper : IUserTokenHelper
    {
        private readonly ILogger<UserTokenHelper> _logger;
        private readonly IAuthService _authService;

        public UserTokenHelper(
            ILogger<UserTokenHelper> logger,
            IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserDTO> RetrieveUser(HttpContext context)
        {
            var token = context.Request.Cookies.Where(name => name.Key == "_bearer").FirstOrDefault().Value;
            var result = await Task.Run(() => _authService.GetTokenClaims(token).ToArray());

            var resultUser = new UserDTO
            {
                UserId = result[1].Value,
                UserName = result[0].Value
            };

            return resultUser;
        }
    }
}
