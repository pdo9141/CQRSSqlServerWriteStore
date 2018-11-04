using AuctionDomainUser.Events;
using AuctionFramework;
using System;

namespace AuctionDomainUser
{
    public class User : AggregateRoot
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public User(Guid userID, string firstName, string lastName, string phoneNumber, string emailAddress, string password) : this()
        {
            Raise(new UserCreated(userID, firstName, lastName, phoneNumber, emailAddress, password));
        }

        private User()
        {
            Register<UserCreated>(When);
            Register<UserPasswordChanged>(When);
        }

        public void When(UserCreated e)
        {
            Id = e.UserID;
            FirstName = e.FirstName;
            LastName = e.LastName;
            PhoneNumber = e.PhoneNumber;
            EmailAddress = e.EmailAddress;
            Password = e.Password;
        }

        public void When(UserPasswordChanged e)
        {
            Password = e.Password;
        }

        public void ChangePassword(string password)
        {
            CheckUserCreated();
            Raise(new UserPasswordChanged(Id, password));
        }

        private void CheckUserCreated()
        {
            if (Id == default(Guid))
                throw new DomainException("User needs to be created first");
        }
    }
}
