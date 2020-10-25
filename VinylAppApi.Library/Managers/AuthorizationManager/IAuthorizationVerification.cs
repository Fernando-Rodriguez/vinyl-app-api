namespace VinylAppApi.Library.Managers.AuthorizationManager
{
    public interface IAuthorizationVerification
    {
        object UserVerifcationWithIdAndSecret(string userId, string userSecret);
    }
}