using System.Collections.Generic;

namespace VinylApp.Domain.AggregatesModel.InventoryAggregate
{
    public class Group
    {
        public Group(string name)
        {
            GroupName = name;
        }

        public string Id { get; set; }
        public string GroupName { get; set; }
        public List<string> UserIds { get; set; } = new List<string>();

        public void AddUserToGroup(string userId)
        {
            UserIds.Add(userId);
        }

        public void RemoveUserFromGroup(string userId)
        {
            UserIds.Remove(userId);
        }
    }
}
