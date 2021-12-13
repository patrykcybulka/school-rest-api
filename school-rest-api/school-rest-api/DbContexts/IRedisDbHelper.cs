namespace school_rest_api.DbContexts
{
    public interface IRedisDbHelper
    {
        void SetDataAsync<T>(string key, T data);

        Task<T> GetDataAsync<T>(string key);

        void RemoveOldDataAsync(IEnumerable<string> keys);
    }
}
