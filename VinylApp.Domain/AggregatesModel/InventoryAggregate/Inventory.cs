using System.Collections.Generic;
using VinylApp.Domain.AggregatesModel.UserAggregate;

namespace VinylApp.Domain.AggregatesModel.InventoryAggregate
{
    public class Inventory
    {
        public Inventory(User user)
        {
            User = user;
        }

        public User User { get; private set; }
        public List<Album> Albums { get; private set; }
        public List<Group> Groups { get; private set; }

        public void AddAlbum(string name, string artist)
        {
            Albums.Add(new Album(name, artist));
        }

        public void AddGroup(string name)
        {
            Groups.Add(new Group(name));
        }
    }
}
