using System.Threading.Tasks;

namespace VinylAppApi.Domain.Services.MusicInformationService
{
    public interface ITokenManager
    {
        string _pToken { get; set; }

        Task TestContact();
    }
}