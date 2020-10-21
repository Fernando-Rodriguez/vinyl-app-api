using System.Collections.Generic;
using VinylAppApi.Library.Models.SpotifyModels.AlbumModels;

namespace VinylAppApi.Library.Models.SpotifyModels
{
    public class SpotifyErrorModel
    {
        public AlbumModel CreateAlbumError()
        {
            return new AlbumModel
            {
                albums = new Albums
                {
                    items = new List<Item>
                       {
                           new Item
                           {
                               name = "Hey this failed"
                           }
                       }
                }
            };
        }
    }
}
