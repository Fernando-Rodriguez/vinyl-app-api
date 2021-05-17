using System.Collections.Generic;
using VinylAppApi.Domain.Entities;

namespace VinylAppApi.Domain.Models.UserInterfacingModels
{
    public class JoinedGroupsDTO
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public List<AlbumModel> GroupAlbums { get; set; } = new List<AlbumModel>();
    }
}
