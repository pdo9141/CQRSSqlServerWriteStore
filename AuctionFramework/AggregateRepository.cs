using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AuctionFramework.Helpers;

namespace AuctionFramework
{
    public class AggregateRepository : IAggregateRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["db_connection"].ToString();

        public async Task<T> Get<T>(Guid id) where T : IAggregateRoot
        {
            var aggregateRoot = (T)Activator.CreateInstance(typeof(T), true);
            var events = await GetEvents(StreamName(typeof(T), id));

            events.ForEach(aggregateRoot.Apply);
            aggregateRoot.ClearEvents();

            return aggregateRoot;
        }

        public async Task<List<object>> GetEvents(string streamName)
        {
            var deserializedEvents = new List<object>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var argumentObject = new
                {
                    Name = streamName
                };

                var events = await connection.QueryAsync<Event>("Event_Sel", argumentObject, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                if (events != null && events.Count() > 0)
                    foreach (var e in events)
                        deserializedEvents.Add(e.Data.Deserialize());
            }

            return deserializedEvents;
        }

        public async Task<int> Save(IAggregateRoot aggregateRoot)
        {
            var dt = new DataTable();
            dt.Columns.Add("Data", typeof(string));

            foreach (var e in aggregateRoot.GetEvents())
            {
                dt.Rows.Add(e.Serialize());
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var argumentObject = new
                {
                    AggregateID = aggregateRoot.Id,
                    Name = StreamName(aggregateRoot.GetType(), aggregateRoot.Id),
                    Type = aggregateRoot.GetType().AssemblyQualifiedName,
                    aggregateRoot.Version,
                    Batch = dt.AsTableValuedParameter("EventValueType")
                };

                return await connection.ExecuteAsync("Event_Ins", argumentObject, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Tuple<long, string>> GetEventDataForPublishing() {
            var sequenceID = default(long);
            var data = String.Empty;

            using (var connection = new SqlConnection(_connectionString)) {
                var sequenceIDs = await connection.QueryAsync<Event>("EventPublisher_Sel", commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                if (sequenceIDs != null && sequenceIDs.Count() > 0)
                    sequenceID = ++sequenceIDs.First().SequenceID;
                else
                    sequenceID = 1;

                var argumentObject = new { ID = sequenceID };
                var events = await connection.QueryAsync<Event>("Event_SelByID", argumentObject, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                if (events != null && events.Count() > 0) {
                    foreach (var e in events) {
                        data = e.Data;
                        break;
                    }
                }
            }

            return new Tuple<long, string>(sequenceID, data);
        }

        public async Task<int> CreatePublishingRecord(long sequenceID) {
            using (var connection = new SqlConnection(_connectionString)) {
                var argumentObject = new {
                    SequenceID = sequenceID
                };

                return await connection.ExecuteAsync("EventPublisher_Ins", argumentObject, commandType: CommandType.StoredProcedure);
            }
        }

        private string StreamName(Type aggregate, Guid id)
        {
            return $"{aggregate.Name}+{id}";
        }
    }
}
