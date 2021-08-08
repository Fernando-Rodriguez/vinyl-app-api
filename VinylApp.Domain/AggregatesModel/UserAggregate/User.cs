using System;

namespace VinylApp.Domain.AggregatesModel.UserAggregate
{
    public class User
    {
        public User(string userName)
        {
            UserName = userName;
        }

        public string Id { get; set; }
        public string UserName { get; private set; }

        public void UpdateUserName(string newUserName)
        {
            if (String.IsNullOrWhiteSpace(newUserName))
            {
                throw new ArgumentException("New username should not be null.");
            }

            UserName = newUserName;
        }
    }
}
