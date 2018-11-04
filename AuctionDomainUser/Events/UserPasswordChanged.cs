using AuctionFramework;
using System;

namespace AuctionDomainUser.Events
{
    public class UserPasswordChanged : EventBase
    {
        public Guid UserID { get; set; }
        public string Password { get; set; }

        public UserPasswordChanged(Guid userID, string password)
        {
            UserID = userID;
            Password = password;
        }
    }
}
