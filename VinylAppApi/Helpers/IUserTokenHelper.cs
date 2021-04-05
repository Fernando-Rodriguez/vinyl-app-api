using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VinylAppApi.Shared.Models.UserInterfacingModels;

namespace VinylAppApi.Helpers
{
    public interface IUserTokenHelper
    {
        Task<UserInfoModelDTO> RetrieveUser(HttpContext context);
    }
}