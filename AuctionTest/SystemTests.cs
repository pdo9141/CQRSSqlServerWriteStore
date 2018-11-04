using AuctionDomainUser;
using AuctionDomainUser.Commands;
using AuctionFramework;
using AuctionFramework.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AuctionTest
{
    [TestClass]
    public class SystemTests
    {
        [TestMethod]
        public void System_User_Test()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        [TestMethod]
        public void System_Publish_UserCreated_Test()
        {
            var eventName = "AuctionDomainUser.Events.UserCreated";
            var data = "{\"$type\":\"AuctionDomainUser.Events.UserCreated, AuctionDomainUser\"}";

            var redis = RedisStore.RedisCache;
            var publisher = redis.Multiplexer.GetSubscriber();
            var count = publisher.Publish(eventName, data);
            Debug.WriteLine($"Number of listeners for {eventName}: {count}");
        }

        [TestMethod]
        public void System_Publish_UserPasswordChanged_Test()
        {
            var eventName = "AuctionDomainUser.Events.UserPasswordChanged";
            var data = "{\"$type\":\"AuctionDomainUser.Events.UserPasswordChanged, AuctionDomainUser\"}";

            var redis = RedisStore.RedisCache;
            var publisher = redis.Multiplexer.GetSubscriber();
            var count = publisher.Publish(eventName, data);
            Debug.WriteLine($"Number of listeners for {eventName}: {count}");
        }

        static async Task AsyncMain()
        {
            var dispatcher = SetupDispatcher();
            var userID = Guid.NewGuid();
            var registerUserCommand = new RegisterUserCommand(userID, "Phillip", "Do", "714-235-5554", "pdo9141@gmail.com", "Password@1");
            await dispatcher.Dispatch(registerUserCommand);

            var changePasswordCommand = new ChangePasswordCommand(userID, "Password@2");
        }

        static CommandDispatcher SetupDispatcher()
        {
            var repository = new AggregateRepository();
            var commandHandlerMap = new CommandHandlerMap(new Handlers(repository));
            return new CommandDispatcher(commandHandlerMap);
        }
    }

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
