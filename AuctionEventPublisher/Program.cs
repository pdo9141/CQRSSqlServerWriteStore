using System;
using System.Configuration;
using System.Threading;
using StackExchange.Redis;
using AuctionFramework;

namespace AuctionEventPublisher {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("######### AuctionEventPublisher Started #########");

            var repository = new AggregateRepository();
            Tuple<long, string> eventData = null;
            var eventName = String.Empty;
            while (true) {
                try {
                    eventData = repository.GetEventDataForPublishing().GetAwaiter().GetResult();
                    if (!String.IsNullOrWhiteSpace(eventData.Item2)) {
                        eventName = GetEventName(eventData.Item2);

                        var redis = RedisStore.RedisCache;
                        var publisher = redis.Multiplexer.GetSubscriber();
                        var count = publisher.Publish(eventName, eventData.Item2);

                        repository.CreatePublishingRecord(eventData.Item1).GetAwaiter().GetResult();
                        Console.WriteLine($"Published SequenceID: {eventData.Item1}, {eventName}");
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }

                Thread.Sleep(9000);
            }

            //var eventName = "AuctionDomainUser.Events.UserCreated";
            //var data = "{\"$type\":\"AuctionDomainUser.Events.UserCreated, AuctionDomainUser\",\"UserID\":\"0fe6807e-c045-4dd7-a16d-d0b772ac3db0\",\"FirstName\":\"Phillip\",\"LastName\":\"Do\",\"PhoneNumber\":\"714-235-5554\",\"EmailAddress\":\"pdo9141@gmail.com\",\"Password\":\"Password@1\",\"Attributes\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[System.String, mscorlib]], mscorlib\"}}";

            //var redis = RedisStore.RedisCache;
            //var publisher = redis.Multiplexer.GetSubscriber();
            //var count = publisher.Publish(eventName, data);

            //var eventName = "AuctionDomainUser.Events.UserPasswordChanged";
            //var data = "{\"$type\":\"AuctionDomainUser.Events.UserPasswordChanged, AuctionDomainUser\",\"UserID\":\"0fe6807e-c045-4dd7-a16d-d0b772ac3db0\",\"Password\":\"Password@2\",\"Attributes\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[System.String, mscorlib]], mscorlib\"}}";

            //var redis = RedisStore.RedisCache;
            //var publisher = redis.Multiplexer.GetSubscriber();
            //var count = publisher.Publish(eventName, data);
            //Debug.WriteLine($"Number of listeners for {eventName}: {count}");
        }

        private static string GetEventName(string eventData) {
            var colonIndex = eventData.IndexOf(":") + 2;
            var commaIndex = eventData.IndexOf(",");
            return eventData.Substring(colonIndex, commaIndex - colonIndex);
        }
    }

    public class RedisStore {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static RedisStore() {
            var configurationOptions = new ConfigurationOptions {
                EndPoints = { ConfigurationManager.AppSettings["redis.connection"] }
            };

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static IDatabase RedisCache => Connection.GetDatabase();
    }
}
