namespace VinylAppApi.Shared.Models.AuthorizationModels
{
    public class TokenRequestDTO
    {
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
    }
}