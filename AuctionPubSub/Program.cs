using System;
using System.Configuration;
using Raven.Client;
using Raven.Client.Document;
using StackExchange.Redis;
using AuctionDomainUser.Events;
using AuctionDomainUser.Queries;
using AuctionFramework.Helpers;

namespace AuctionEventSubscriber {
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("######### AuctionEventSubscriber Started #########");

            var redis = RedisStore.RedisCache;

            var sub = redis.Multiplexer.GetSubscriber();
            sub.Subscribe("AuctionDomainUser.Events.UserCreated", (channel, message) => {
                var publishedEvent = (UserCreated)Convert.ToString(message).Deserialize();
                var connectionFactory = new ConnectionFactory("Users");

                using (var session = connectionFactory.Connect())
                {
                    session.Store(new UserQueryModel {
                        ID = publishedEvent.UserID,
                        FirstName = publishedEvent.FirstName,
                        LastName = publishedEvent.LastName,
                        PhoneNumber = publishedEvent.PhoneNumber,
                        EmailAddress = publishedEvent.EmailAddress,
                        Password = publishedEvent.Password
                    }, publishedEvent.UserID.ToString());

                    session.SaveChanges();
                }
                Console.WriteLine($"Synced UserCreated event for {publishedEvent.FirstName} {publishedEvent.LastName}");
            });

            sub.Subscribe("AuctionDomainUser.Events.UserPasswordChanged", (channel, message) => {
                var publishedEvent = (UserPasswordChanged)Convert.ToString(message).Deserialize();
                var connectionFactory = new ConnectionFactory("Users");

                using (var session = connectionFactory.Connect())
                {
                    var user = session.Load<UserQueryModel>(publishedEvent.UserID.ToString());
                    user.Password = publishedEvent.Password;
                    session.SaveChanges();
                }

                Console.WriteLine($"Synced UserPasswordChanged event for UserID: {publishedEvent.UserID}");
            });

            Console.ReadLine();
        }
    }

    public class RedisStore
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static RedisStore()
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { ConfigurationManager.AppSettings["redis.connection"] }
            };

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static IDatabase RedisCache => Connection.GetDatabase();
    }

    public class ConnectionFactory
    {
        private readonly IDocumentStore _store;

        public ConnectionFactory(string database)
        {
            _store = new DocumentStore
            {
                Url = "http://localhost:8080/",
                DefaultDatabase = database
            };

            _store.Initialize();
        }

        public IDocumentSession Connect()
        {
            return _store.OpenSession();
        }
    }
}
