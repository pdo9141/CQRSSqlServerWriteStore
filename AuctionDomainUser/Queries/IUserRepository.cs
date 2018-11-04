using System;

namespace AuctionDomainUser.Queries
{
    public interface IUserRepository
    {
        UserQueryModel GetUser(Guid id);
    }
}
