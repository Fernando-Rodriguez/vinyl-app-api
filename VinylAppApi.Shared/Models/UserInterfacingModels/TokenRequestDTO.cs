namespace VinylAppApi.Shared.Models.AuthorizationModels
{

    // This is the format of the JSON that a user will
    // send in order to be confirmed.
    public class TokenRequestDTO
    {
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
    }
}