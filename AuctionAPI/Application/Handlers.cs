using AuctionDomainUser;
using AuctionDomainUser.Commands;
using AuctionFramework;
using AuctionFramework.Commands;

namespace AuctionAPI.Application
{
    public class Handlers : CommandHandler
    {
        public Handlers(AggregateRepository repository)
        {
            Register<RegisterUserCommand>(async c =>
            {
                var user = new User(c.UserID, c.FirstName, c.LastName, c.PhoneNumber, c.EmailAddress, c.Password);
                await repository.Save(user);
            });

            Register<ChangePasswordCommand>(async c =>
            {
                var user = await repository.Get<User>(c.UserID);
                user.ChangePassword(c.Password);
                await repository.Save(user);
            });
        }
    }
}