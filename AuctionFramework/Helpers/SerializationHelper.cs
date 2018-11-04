using Newtonsoft.Json;

namespace AuctionFramework.Helpers
{
    public static class SerializationHelper
    {
        public static string Serialize(this object e)
        {
            return JsonConvert.SerializeObject(e, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        }

        public static object Deserialize(this string eventData)
        {
            return JsonConvert.DeserializeObject(eventData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        }
    }
}
