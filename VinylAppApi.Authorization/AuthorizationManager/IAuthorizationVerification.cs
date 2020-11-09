namespace VinylAppApi.Authorization.AuthorizationManager
{
    public interface IAuthorizationVerification
    {
        object UserVerifcationWithIdAndSecret(string userId, string userSecret);
    }
}