using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;


namespace VinylAppApi.Library.Models.AuthorizationModels
{
    public class JwtContainerModel : IAuthContainerModel
    {
        public string SecretKey { get; set; } = "AAA1231231213155252352355";
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } = 5;
        public Claim[] Claims { get; set; }
    }
}