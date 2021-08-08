using System.Collections.Generic;
using VinylApp.Domain.AggregatesModel.InventoryAggregate;

namespace VinylApp.Domain.DTOs
{
    public class AlbumsDTO
    {
        public List<Album> Owned_Albums { get; set; } = new List<Album>();
    }
}
