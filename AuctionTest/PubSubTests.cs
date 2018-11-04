using AuctionDomainUser.Events;
using AuctionFramework.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;
using System;
using System.Diagnostics;

namespace AuctionTest
{
    [TestClass]
    public class PubSubTests
    {
        [TestMethod]
        public void PubSub_Connection_Test()
        {
            var redis = RedisStore.RedisCache;

            if (redis.StringSet("testKey", "testValue"))
            {
                var val = redis.StringGet("testKey");
                Debug.WriteLine(val);
            }
        }

        [TestMethod]
        public void PubSub_Test()
        {
            var redis = RedisStore.RedisCache;
            var sub = redis.Multiplexer.GetSubscriber();

            sub.Subscribe("test", (channel, message) => {
                Debug.WriteLine("Got notification: " + (string)message);
            });

            var pub = redis.Multiplexer.GetSubscriber();

            var count = pub.Publish("test", "Hello there I am a test message");
            Debug.WriteLine($"Number of listeners for test {count}");

            sub.Subscribe(new RedisChannel("a*c", RedisChannel.PatternMode.Pattern), (channel, message) =>
            {
                Debug.WriteLine($"Got pattern a*c notification: {message}");
            });

            count = pub.Publish("a*c", "Hello there I am a a*c message");
            Debug.WriteLine($"Number of listeners for a*c {count}");

            pub.Publish("abc", "hello there I am a abc message");
            pub.Publish("a1234567890c", "hello there I am a a1234567890c message");
            pub.Publish("ab", "hello there I am a lost message");   // never printed

            sub.Subscribe(new RedisChannel("*123", RedisChannel.PatternMode.Literal), (channel, message) =>
            {
                Debug.WriteLine($"Got Literal pattern *123 notification: {message}");
            });

            count = pub.Publish("*123", "Hello there I am a *123 message");
            count = pub.Publish("a123", "Hello there I am a a123 message"); // never received due to literal pattern

            sub.Subscribe(new RedisChannel("zyx*", RedisChannel.PatternMode.Auto), (channel, message) =>
            {
                Debug.WriteLine($"Got Literal pattern zyx* notification: {message}");
            });

            pub.Publish("zyxabc", "hello there I am a zyxabc message");
            pub.Publish("zyx1234", "hello there I am a zyx1234 message");

            sub.Subscribe("test", (channel, message) =>
            {
                Debug.WriteLine($"I am a late subscriber Got notification: {message}");
            });

            sub.Unsubscribe("a*c");
            count = pub.Publish("abc", "Hello there I am a abc message");   // no one listening anymore
            Debug.WriteLine($"Number of listerners for a*c {count}");
        }

        [TestMethod]
        public void PubSub_Json_Test()
        {
            var redis = RedisStore.RedisCache;
            var sub = redis.Multiplexer.GetSubscriber();

            sub.Subscribe("UserCreated", (channel, message) =>
            {
                var uc = (UserCreated)Convert.ToString(message).Deserialize();
                var firstName = uc.FirstName;
                var lastName = uc.LastName;
            });

            var userCreated = new UserCreated(Guid.NewGuid(), "Emma", "Do", "714-235-5554", "edo@gmail.com", "123456789");
            var pub = redis.Multiplexer.GetSubscriber();
            var count = pub.Publish("UserCreated", userCreated.Serialize());
            Debug.WriteLine($"Number of listerners for test {count}");
        }
    }
}
