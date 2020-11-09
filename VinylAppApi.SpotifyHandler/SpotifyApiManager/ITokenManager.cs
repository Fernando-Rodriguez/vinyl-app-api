using System.Threading.Tasks;

namespace VinylAppApi.SpotifyHandler.SpotifyApiManager
{
    public interface ITokenManager
    {
        string _pToken { get; set; }

        Task TestContact();
    }
}