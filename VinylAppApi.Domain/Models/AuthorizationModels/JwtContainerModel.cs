using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace VinylAppApi.Domain.Models.AuthorizationModels
{
    public class JwtContainerModel : IAuthContainerModel
    {
        private readonly IConfiguration _config;

        public JwtContainerModel(IConfiguration config)
        {
            _config = config;
        }
        
        public string SecretKey => _config.GetSection("ServerCredentials").ToString();
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } = 60;
        public Claim[] Claims { get; set; }
    }
}