using System.Collections.Generic;
using VinylApp.Domain.AggregatesModel.InventoryAggregate;

namespace VinylApp.Domain.DTOs
{
    public class JoinedGroupsDTO
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public List<Album> GroupAlbums { get; set; } = new List<Album>();
    }
}