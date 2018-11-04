using System;

namespace AuctionDomainUser.Commands
{
    public class ChangePasswordCommand
    {
        public Guid UserID { get; set; }
        public string Password { get; set; }

        public ChangePasswordCommand(Guid userID, string password)
        {
            UserID = userID;
            Password = password;
        }
    }
}
