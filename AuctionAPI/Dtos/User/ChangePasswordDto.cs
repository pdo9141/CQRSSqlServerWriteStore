using System;

namespace AuctionAPI.Dtos.User
{
    public class ChangePasswordDto
    {
        public Guid UserID { get; set; }
        public string EmailAddress { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}