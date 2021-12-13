using Newtonsoft.Json;
using StackExchange.Redis;

namespace school_rest_api.Databases
{
    public class RedisDbManager : IRedisDbManager
    {
        private readonly IConnectionMultiplexer _multiplexer;

        public RedisDbManager(IConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        public async void SetDataAsync<T>(string key, T data)
        {
            var db = _multiplexer.GetDatabase();

            await db.StringSetAsync(key, JsonConvert.SerializeObject(data));
        }

        public async Task<T> GetDataAsync<T>(string key)
        {
            var db = _multiplexer.GetDatabase();

            var result = await db.StringGetAsync(key);

            if (result.IsNullOrEmpty)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async void RemoveOldDataAsync(IEnumerable<string> keys)
        {
            var db = _multiplexer.GetDatabase();

            foreach (var key in keys)
            {
              await db.KeyDeleteAsync(key);
            }
        }
    }
}
