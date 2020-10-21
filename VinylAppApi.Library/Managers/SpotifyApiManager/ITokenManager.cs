using System.Threading.Tasks;

namespace VinylAppApi.Library.SpotifyApiManager
{
    public interface ITokenManager
    {
        string _pToken { get; set; }

        Task TestContact();
    }
}