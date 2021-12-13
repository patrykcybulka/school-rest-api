namespace school_rest_api.Databases
{
    public interface IRedisDbManager
    {
        void SetDataAsync<T>(string key, T data);

        Task<T> GetDataAsync<T>(string key);

        void RemoveOldDataAsync(IEnumerable<string> keys);
    }
}
