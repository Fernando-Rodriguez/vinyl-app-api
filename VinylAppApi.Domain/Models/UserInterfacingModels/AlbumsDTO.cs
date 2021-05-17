using System.Collections.Generic;
using VinylAppApi.Domain.Entities;

namespace VinylAppApi.Domain.Models.UserInterfacingModels
{
    public class AlbumsDTO
    {
        public List<AlbumModel> Owned_Albums { get; set; } = new List<AlbumModel>();
    }
}
