﻿using System;
using Raven.Client;
using Raven.Client.Document;

namespace AuctionDomainUser.Queries
{
    public class UserRepository : IUserRepository
    {
        public UserQueryModel GetUser(Guid id)
        {
            var connectionFactory = new ConnectionFactory("Users");

            using (var session = connectionFactory.Connect())
            {
                var user = session.Load<UserQueryModel>(id.ToString());
                return user;
            }
        }
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
