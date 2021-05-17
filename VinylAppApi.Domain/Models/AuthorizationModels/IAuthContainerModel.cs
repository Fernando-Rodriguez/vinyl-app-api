using System.Security.Claims;

namespace VinylAppApi.Shared.Models.AuthorizationModels
{
    public interface IAuthContainerModel
    {
        string SecretKey { get; }
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; set; }
        Claim[] Claims { get; set; }
    }
}