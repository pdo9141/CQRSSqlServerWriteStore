using AuctionDomainUser;
using AuctionDomainUser.Events;
using AuctionFramework.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AuctionTest
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void SerializeDeserialize_Test()
        {
            var userCreated = new UserCreated(Guid.NewGuid(), "Phillip", "Do", "714-235-5554", "pdo9141@gmail.com", "Password@1");
            userCreated.Attributes.Add("NBA Team", "Los Angeles Lakers");
            userCreated.Attributes.Add("MLB Team", "Los Angeles Dodgers");
            userCreated.Attributes.Add("NFL Team", "Dallas Cowboys");

            var serialized = userCreated.Serialize();
            var deserialized = serialized.Deserialize();

            var aggregateRoot = (User)Activator.CreateInstance(typeof(User), true);
            var events = new List<object> { deserialized };

            events.ForEach(aggregateRoot.Apply);
            aggregateRoot.ClearEvents();
        }
    }
}
