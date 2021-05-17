using System.Collections.Generic;
using System.Security.Claims;
using VinylAppApi.Domain.Models.AuthorizationModels;

namespace VinylAppApi.Domain.Services.AuthorizationService
{
    public interface IAuthService
    {
        string SecretKey { get; set; }
        bool IsTokenValid(string token);
        string TokenGeneration(IAuthContainerModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
