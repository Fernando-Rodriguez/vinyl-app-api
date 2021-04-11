using System.Collections.Generic;
using VinylAppApi.Shared.Models.DbModels;

namespace VinylAppApi.Shared.Models.UserInterfacingModels
{
    public class JoinedGroupsDTO
    {
        public string GroupName { get; set; }
        public List<OwnedAlbumModel> GroupAlbums { get; set; }
    }
}
