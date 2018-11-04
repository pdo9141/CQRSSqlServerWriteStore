using AuctionFramework;
using System;

namespace AuctionDomainUser.Events
{
    public class UserCreated : EventBase
    {
        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public UserCreated(Guid userID, string firstName, string lastName, string phoneNumber, string emailAddress, string password)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Password = password;
        }
    }
}
