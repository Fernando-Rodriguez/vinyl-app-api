using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VinylAppApi.Domain.Models.UserInterfacingModels;

namespace VinylAppApi.Helpers
{
    public interface IUserTokenHelper
    {
        Task<UserInfoModelDTO> RetrieveUser(HttpContext context);
    }
}