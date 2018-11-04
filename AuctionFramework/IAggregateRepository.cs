using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionFramework
{
    public interface IAggregateRepository
    {
        Task<T> Get<T>(Guid id) where T : IAggregateRoot;

        Task<int> Save(IAggregateRoot aggregate);

        Task<List<object>> GetEvents(string streamName);
    }
}
