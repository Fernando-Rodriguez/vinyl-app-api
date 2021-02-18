using System.Collections.Generic;
using VinylAppApi.Shared.Models.DbModels;

namespace VinylAppApi.Shared.Models.UserInterfacingModels
{
    public class AlbumsDTO
    {
        public List<OwnedAlbumModel> Owned_Albums { get; set; } = new List<OwnedAlbumModel>();
    }
}
