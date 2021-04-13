using System.Collections.Generic;
using VinylAppApi.Shared.Models.DbModels;

namespace VinylAppApi.Shared.Models.UserInterfacingModels
{
    public class JoinedGroupsDTO
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public List<OwnedAlbumModel> GroupAlbums { get; set; } = new List<OwnedAlbumModel>();
    }
}
