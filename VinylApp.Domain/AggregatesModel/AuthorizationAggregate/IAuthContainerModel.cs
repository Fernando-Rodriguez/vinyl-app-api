using System.Security.Claims;

namespace VinylApp.Domain.AggregatesModel.AuthorizationAggregate
{
    public interface IAuthContainerModel
    {
        string SecretKey { get; }
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; set; }
        Claim[] Claims { get; set; }
    }
}