using System;
using System.Collections.Generic;

namespace VinylAppApi.Shared.Models.DbModels
{
    public class Albums
    {
        public List<OwnedAlbumModel> Owned_Albums { get; set; } = new List<OwnedAlbumModel>();
    }
}
